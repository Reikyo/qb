using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyFunctions;

public class TranslatorController : MonoBehaviour
{
    private GameManager gameManager;
    private NavMeshSurface navNavMesh;

    public enum activator {both, projectile, switcher};
    public activator activatorType;

    public enum switcherTrigger {both, state1to2, state2to1};
    public switcherTrigger switcherTriggerType;

    public enum axis {x, y, z};
    public axis axisType;

    public enum position {lower, upper};
    public position posPositionStart;
    private position posPosition;
    private position posPositionTarget;
    private float fMetresPositionStart;
    private float fMetresPositionTarget;
    public float fMetresPositionLower = 2f;
    public float fMetresPositionUpper = 6f;

    private float fMetresPerSec;
    private float fMetresPerFrame;

    public float fTransitionTime = 0.5f;

    private bool bChangeState;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        navNavMesh = GameObject.Find("Nav Mesh").GetComponent<NavMeshSurface>();

        if (posPositionStart == position.lower)
        {
            fMetresPositionStart = fMetresPositionLower;
        }
        else
        {
            fMetresPositionStart = fMetresPositionUpper;
        }

        fMetresPerSec = (fMetresPositionUpper - fMetresPositionLower) / fTransitionTime;

        Reset();
    }

    // ------------------------------------------------------------------------------------------------

    public void Reset()
    {
        // We do the following to reset the position rather than something simpler, that would look
        // something like "transform.position = v3PositionStart", in order to cater for the fact that the
        // level start position is different from the level play position. This is not an issue for
        // rotations as the level start rotation is the same as the level play rotation.
        switch (axisType)
        {
            case axis.x:
                transform.position = new Vector3(
                    transform.parent.transform.position.x + fMetresPositionStart,
                    transform.position.y,
                    transform.position.z
                );
                break;
            case axis.y:
                transform.position = new Vector3(
                    transform.position.x,
                    transform.parent.transform.position.y + fMetresPositionStart,
                    transform.position.z
                );
                break;
            case axis.z:
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.parent.transform.position.z + fMetresPositionStart
                );
                break;
        }

        posPosition = posPositionStart;
        posPositionTarget = posPositionStart;
        bChangeState = false;
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (bChangeState)
        {
            fMetresPerFrame = fMetresPerSec * Time.deltaTime;
            switch (axisType)
            {
                case axis.x:
                    bChangeState = MyFunctions.Move.Translate(
                        gameObject,
                        "x",
                        fMetresPerFrame,
                        fMetresPositionTarget - gameObject.transform.position.x
                    );
                    break;
                case axis.y:
                    bChangeState = MyFunctions.Move.Translate(
                        gameObject,
                        "y",
                        fMetresPerFrame,
                        fMetresPositionTarget - gameObject.transform.position.y
                    );
                    break;
                case axis.z:
                    bChangeState = MyFunctions.Move.Translate(
                        gameObject,
                        "z",
                        fMetresPerFrame,
                        fMetresPositionTarget - gameObject.transform.position.z
                    );
                    break;
            }
            if (!bChangeState)
            {
                // We need to build the navmesh when a translator moves if it has a surface which is supposed to be
                // part of the navmesh, so let's just do it in all cases anyway:
                navNavMesh.BuildNavMesh();
                posPosition = posPositionTarget;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(string sActivator="", string sSwitcherTrigger="", bool bSfx=true)
    {
        if (    (   (sActivator == "projectile")
                &&  ((activatorType == activator.projectile) || (activatorType == activator.both)) )
            ||  (   (sActivator == "switcher")
                &&  ((activatorType == activator.switcher) || (activatorType == activator.both))
                &&  (   (   (sSwitcherTrigger == "state1to2")
                        &&  ((switcherTriggerType == switcherTrigger.state1to2) || (switcherTriggerType == switcherTrigger.both)) )
                    ||  (   (sSwitcherTrigger == "state2to1")
                        &&  ((switcherTriggerType == switcherTrigger.state2to1) || (switcherTriggerType == switcherTrigger.both)) ) ) ) )
        {
            bChangeState = true;
            if (bSfx)
            {
                gameManager.SfxclpPlay("sfxclpTranslator");
            }
            if (posPositionTarget == position.lower)
            {
                posPositionTarget = position.upper;
                fMetresPositionTarget = fMetresPositionUpper;
            }
            else
            {
                posPositionTarget = position.lower;
                fMetresPositionTarget = fMetresPositionLower;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
