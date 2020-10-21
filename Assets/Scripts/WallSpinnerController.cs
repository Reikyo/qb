using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;

public class WallSpinnerController : MonoBehaviour
{
    private GameManager gameManager;

    public enum rotationY {horizontal, vertical};
    public rotationY rotRotationYStart = rotationY.horizontal;
    private rotationY rotRotationYCurrent = rotationY.horizontal;

    private float fDegreesRotationY = 0f;
    private float fDegreesRotationYLower = 0f;
    private float fDegreesRotationYUpper = 90f;
    private float fDegreesRotationYTarget = 0f;

    private float fDegreesPerSecY;
    private float fDegreesPerFrameY;

    private float fTransitionTime = 0.5f;

    private int iDirection = -1;
    private int iDirectionSum = 0;

    private bool bChangeState = false;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        fDegreesPerSecY = (fDegreesRotationYUpper - fDegreesRotationYLower) / fTransitionTime;

        Reset();
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
                if (iDirectionSum % 2 != 0)
                {
                    if (rotRotationYCurrent == rotationY.horizontal)
                    {
                        rotRotationYCurrent = rotationY.vertical;
                    }
                    else
                    {
                        rotRotationYCurrent = rotationY.horizontal;
                    }
                }
                if (rotRotationYCurrent == rotationY.horizontal)
                {
                    fDegreesRotationY = fDegreesRotationYLower;
                    fDegreesRotationYTarget = fDegreesRotationYLower;
                    transform.eulerAngles = new Vector3(0f, fDegreesRotationYLower, 0f);
                }
                else
                {
                    fDegreesRotationY = fDegreesRotationYUpper;
                    fDegreesRotationYTarget = fDegreesRotationYUpper;
                    transform.eulerAngles = new Vector3(0f, fDegreesRotationYUpper, 0f);
                }
                iDirectionSum = 0;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(int iDirectionGiven=0, bool bSfx=true)
    {
        if (bSfx)
        {
            gameManager.SfxclpPlay("sfxclpWallSpinner");
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
        iDirectionSum += iDirection;
        fDegreesRotationYTarget += iDirection * fDegreesRotationYUpper;
    }

    // ------------------------------------------------------------------------------------------------

    public void Reset()
    {
        if (rotRotationYCurrent != rotRotationYStart)
        {
            Trigger(0, false);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
