using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;

public class RotatorController : MonoBehaviour
{
    private GameManager gameManager;

    // public enum rotationY {horizontal, vertical};
    // public rotationY rotRotationYStart = rotationY.horizontal;
    // private rotationY rotRotationYCurrent = rotationY.horizontal;

    public enum activator {both, projectile, switcher};
    public activator activatorType;

    public enum switcherTrigger {both, state1to2, state2to1};
    public switcherTrigger switcherTriggerType;

    public enum direction {clockwise, anticlockwise};
    public direction directionTypeStart;
    private int iDirection;
    // private int iDirectionSum;

    private Quaternion quatRotationStart;
    private float fDegreesRotationY;
    private float fDegreesRotationYTarget;
    private float fDegreesRotationYLower = 0f;
    private float fDegreesRotationYUpper = 90f;

    private float fDegreesPerSecY;
    private float fDegreesPerFrameY;

    public float fTransitionTime = 0.5f;

    private bool bChangeState;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        fDegreesPerSecY = (fDegreesRotationYUpper - fDegreesRotationYLower) / fTransitionTime;
        quatRotationStart = transform.rotation;

        Reset();
    }

    // ------------------------------------------------------------------------------------------------

    public void Reset()
    {
        if (directionTypeStart == direction.clockwise)
        {
            iDirection = -1;
        }
        else
        {
            iDirection = 1;
        }
        transform.rotation = quatRotationStart;
        fDegreesRotationY = 0f;
        fDegreesRotationYTarget = 0f;
        bChangeState = false;

        // if (rotRotationYCurrent != rotRotationYStart)
        // {
        //     Trigger(0, false);
        // }
        // if (iDirectionSum != 0)
        // {
        //     bChangeState = true;
        //     iDirectionSum = 0;
        //     fDegreesRotationYTarget = -iDirectionSum * fDegreesRotationYUpper;
        // }
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (bChangeState)
        {
            fDegreesPerFrameY = fDegreesPerSecY * Time.deltaTime;
            var tuple = MyFunctions.Move.Rotate(
                gameObject,
                "y",
                iDirection * fDegreesPerFrameY,
                fDegreesRotationY,
                fDegreesRotationYTarget
            );
            bChangeState = tuple.Item1;
            fDegreesRotationY = tuple.Item2;
            if (!bChangeState)
            {
                // if (iDirectionSum % 2 != 0)
                // {
                //     if (rotRotationYCurrent == rotationY.horizontal)
                //     {
                //         rotRotationYCurrent = rotationY.vertical;
                //     }
                //     else
                //     {
                //         rotRotationYCurrent = rotationY.horizontal;
                //     }
                // }
                // if (rotRotationYCurrent == rotationY.horizontal)
                // {
                //     fDegreesRotationY = fDegreesRotationYLower;
                //     fDegreesRotationYTarget = fDegreesRotationYLower;
                //     transform.eulerAngles = new Vector3(0f, fDegreesRotationYLower, 0f);
                // }
                // else
                // {
                //     fDegreesRotationY = fDegreesRotationYUpper;
                //     fDegreesRotationYTarget = fDegreesRotationYUpper;
                //     transform.eulerAngles = new Vector3(0f, fDegreesRotationYUpper, 0f);
                // }
                // iDirectionSum = 0;
                fDegreesRotationY = 0f;
                fDegreesRotationYTarget = 0f;
                // if (iDirectionSum % 4 == 0)
                // {
                //     iDirectionSum = 0;
                // }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(string sActivator="", string sSwitcherTrigger="", int iDirectionGiven=0, bool bSfx=true)
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
                    gameManager.SfxclpPlay("sfxclpRotator");
                }
                bChangeState = true;
                if (iDirectionGiven == 0)
                {
                    if (iDirection == -1)
                    {
                        iDirection = 1;
                    }
                    else
                    {
                        iDirection = -1;
                    }
                }
                else
                {
                    iDirection = iDirectionGiven;
                }
                // iDirectionSum += iDirection;
                fDegreesRotationYTarget += iDirection * fDegreesRotationYUpper;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
