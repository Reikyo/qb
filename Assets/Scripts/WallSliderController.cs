using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyFunctions;

public class WallSliderController : MonoBehaviour
{
    private GameManager gameManager;
    private NavMeshSurface navNavMesh;

    public enum activator {both, projectile, switcher};
    public activator activatorType;

    public enum switcherTrigger {both, state1to2, state2to1};
    public switcherTrigger switcherTriggerType;

    public enum direction {x, y, z};
    public direction directionType;

    public enum positionY {lower, upper};
    public positionY posPositionYStart = positionY.lower;
    private positionY posPositionYCurrent = positionY.lower;

    public float fMetresPositionYLower = 2f;
    public float fMetresPositionYUpper = 6f;
    private float fMetresPositionYTarget;

    private float fMetresPerSecY;
    private float fMetresPerFrameY;

    public float fTransitionTime = 0.5f;

    private int iDirection = -1;

    private bool bChangeState = false;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        navNavMesh = GameObject.Find("Nav Mesh").GetComponent<NavMeshSurface>();

        fMetresPerSecY = (fMetresPositionYUpper - fMetresPositionYLower) / fTransitionTime;

        Reset();
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (bChangeState)
        {
            fMetresPerFrameY = fMetresPerSecY * Time.deltaTime;
            switch (directionType)
            {
                case direction.x:
                    bChangeState = MyFunctions.Move.Translate(
                        gameObject,
                        "x",
                        iDirection * fMetresPerFrameY,
                        transform.position.x,
                        fMetresPositionYTarget
                    );
                    break;
                case direction.y:
                    bChangeState = MyFunctions.Move.Translate(
                        gameObject,
                        "y",
                        iDirection * fMetresPerFrameY,
                        transform.position.y,
                        fMetresPositionYTarget
                    );
                    break;
                case direction.z:
                    bChangeState = MyFunctions.Move.Translate(
                        gameObject,
                        "z",
                        iDirection * fMetresPerFrameY,
                        transform.position.z,
                        fMetresPositionYTarget
                    );
                    break;
            }
            if (!bChangeState)
            {
                // We need to build the navmesh when a translator moves if it has a surface which is supposed to be
                // part of the navmesh, so let's just do it in all cases anyway:
                navNavMesh.BuildNavMesh();
                if (iDirection == 1)
                {
                    posPositionYCurrent = positionY.upper;
                }
                else
                {
                    posPositionYCurrent = positionY.lower;
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(string sActivator="", string sSwitcherTrigger="", bool bSfx=true)
    {
        if (    (activatorType == activator.both)
            ||  ((activatorType == activator.projectile) && (sActivator == "projectile"))
            ||  ((activatorType == activator.switcher) && (sActivator == "switcher")) )
        {
            if (    (sActivator == "projectile")
                ||  (   (sActivator == "switcher")
                    &&  (switcherTriggerType == switcherTrigger.both)
                    ||  ((switcherTriggerType == switcherTrigger.state1to2) && (sSwitcherTrigger == "state1to2"))
                    ||  ((switcherTriggerType == switcherTrigger.state2to1) && (sSwitcherTrigger == "state2to1")) ) )
            {
                if (bSfx)
                {
                    gameManager.SfxclpPlay("sfxclpTranslator");
                }
                bChangeState = true;
                if (iDirection == -1)
                {
                    iDirection = 1;
                    fMetresPositionYTarget = fMetresPositionYUpper;
                }
                else
                {
                    iDirection = -1;
                    fMetresPositionYTarget = fMetresPositionYLower;
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Reset()
    {
        if (posPositionYCurrent != posPositionYStart)
        {
            bChangeState = true;
            if (iDirection == -1)
            {
                iDirection = 1;
                fMetresPositionYTarget = fMetresPositionYUpper;
            }
            else
            {
                iDirection = -1;
                fMetresPositionYTarget = fMetresPositionYLower;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
