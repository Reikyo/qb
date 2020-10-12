using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;

public class WallSpinnerController : MonoBehaviour
{
    private GameManager gameManager;

    private float fDegreesRotationY = 0f;
    private float fDegreesRotationYLower = 0f;
    private float fDegreesRotationYUpper = 90f;
    private float fDegreesRotationYTarget = 0f;

    private float fDegreesPerSecY;
    private float fDegreesPerFrameY;

    private float fTransitionTime = 0.5f;

    private int iDirection = -1;

    private bool bChangeState = false;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        fDegreesPerSecY = (fDegreesRotationYUpper - fDegreesRotationYLower) / fTransitionTime;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
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
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(int iDirectionGiven=0)
    {
        gameManager.SfxclpPlay("sfxclpWallSpinner");
        bChangeState = true;
        if (iDirectionGiven == 0)
        {
            if (iDirection == -1)
            {
                iDirection = 1;
                fDegreesRotationYTarget = fDegreesRotationYUpper;
            }
            else
            {
                iDirection = -1;
                fDegreesRotationYTarget = fDegreesRotationYLower;
            }
        }
        else
        {
            iDirection = iDirectionGiven;
            fDegreesRotationYTarget += iDirection * fDegreesRotationYUpper;
        }
    }

    // ------------------------------------------------------------------------------------------------

}
