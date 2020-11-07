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
    private string sAxis;

    public enum position {lower, upper};
    public position posPositionStart;
    public position posPositionTarget;
    public position posPositionCurrent;

    public float fMetresPositionLower = 2f;
    public float fMetresPositionUpper = 6f;
    private float fMetresPositionStart;
    private float fMetresPositionTarget;

    private float fMetresPerSec;
    private float fMetresPerFrame;

    public float fTransitionTime = 0.5f;

    private bool bChangeState = false;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        navNavMesh = GameObject.Find("Nav Mesh").GetComponent<NavMeshSurface>();

        posPositionTarget = posPositionStart;
        posPositionCurrent = posPositionStart;

        if (posPositionStart == position.lower)
        {
            fMetresPositionStart = fMetresPositionLower;
        }
        else
        {
            fMetresPositionStart = fMetresPositionUpper;
        }

        fMetresPerSec = (fMetresPositionUpper - fMetresPositionLower) / fTransitionTime;

        switch (axisType)
        {
            case axis.x:
                sAxis = "x";
                break;
            case axis.y:
                sAxis = "y";
                break;
            case axis.z:
                sAxis = "z";
                break;
        }
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (bChangeState)
        {
            fMetresPerFrame = fMetresPerSec * Time.deltaTime;
            bChangeState = MyFunctions.Move.Translate(
                gameObject,
                sAxis,
                fMetresPerFrame,
                fMetresPositionTarget
            );
            if (!bChangeState)
            {
                // We need to build the navmesh when a translator moves if it has a surface which is supposed to be
                // part of the navmesh, so let's just do it in all cases anyway:
                navNavMesh.BuildNavMesh();
                posPositionCurrent = posPositionTarget;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(string sActivator="", string sSwitcherTrigger="", bool bSfx=true)
    {
        if (    (sActivator == "reset")
            ||  (   (sActivator == "projectile")
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

    public void Reset()
    {
        // switch (axisType)
        // {
        //     case axis.x:
        //         fMetresPositionLower += transform.parent.transform.position.x;
        //         fMetresPositionUpper += transform.parent.transform.position.x;
        //         fMetresPositionStart += transform.parent.transform.position.x;
        //         break;
        //     case axis.y:
        //         fMetresPositionLower += transform.parent.transform.position.y;
        //         fMetresPositionUpper += transform.parent.transform.position.y;
        //         fMetresPositionStart += transform.parent.transform.position.y;
        //         break;
        //     case axis.z:
        //         fMetresPositionLower += transform.parent.transform.position.z;
        //         fMetresPositionUpper += transform.parent.transform.position.z;
        //         fMetresPositionStart += transform.parent.transform.position.z;
        //         break;
        // }
        if (posPositionCurrent != posPositionStart)
        {
            posPositionCurrent = posPositionStart;
            posPositionTarget = posPositionStart;
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
        }
    }

    // ------------------------------------------------------------------------------------------------

}
