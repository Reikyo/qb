using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;

public class WallSliderController : MonoBehaviour
{
    private GameManager gameManager;

    public enum positionY {down, up};
    public positionY posPositionYStart = positionY.down;
    private positionY posPositionYCurrent = positionY.down;

    private float fMetresPositionYLower = 2f;
    private float fMetresPositionYUpper = 6f;
    private float fMetresPositionYTarget;

    private float fMetresPerSecY;
    private float fMetresPerFrameY;

    private float fTransitionTime = 0.5f;

    private int iDirection = -1;

    private bool bChangeState = false;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        fMetresPerSecY = (fMetresPositionYUpper - fMetresPositionYLower) / fTransitionTime;

        Reset();
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (bChangeState)
        {
            fMetresPerFrameY = fMetresPerSecY * Time.deltaTime;
            bChangeState = MyFunctions.Move.Translate(
                gameObject,
                "y",
                iDirection * fMetresPerFrameY,
                transform.position.y,
                fMetresPositionYTarget
            );
            if (!bChangeState)
            {
                if (iDirection == 1)
                {
                    posPositionYCurrent = positionY.up;
                }
                else
                {
                    posPositionYCurrent = positionY.down;
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger()
    {
        gameManager.SfxclpPlay("sfxclpWallSlider");
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

    // ------------------------------------------------------------------------------------------------

    public void Reset()
    {
        if (posPositionYCurrent != posPositionYStart)
        {
            Trigger();
        }
    }

    // ------------------------------------------------------------------------------------------------

}
