using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;

public class WallController : MonoBehaviour
{
    public enum positionY {down, up};
    public positionY posStartPositionY = positionY.down;
    private positionY posCurrentPositionY = positionY.down;
    private bool bTargetPositionY;

    private float fLowerPositionY = 2f;
    private float fUpperPositionY = 6f;
    private float fTransitionTime = 0.5f;
    private float fMetresPerSecY;
    private float fMetresPerFrameY;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        fMetresPerSecY = (fUpperPositionY - fLowerPositionY) / fTransitionTime;
        fMetresPerFrameY = fMetresPerSecY * Time.deltaTime;

        Reset();
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (!bTargetPositionY)
        {
            if (posCurrentPositionY == positionY.down)
            {
                bTargetPositionY = MyFunctions.Move.Translate(gameObject, "y", fMetresPerFrameY, transform.position.y, fUpperPositionY, bTargetPositionY);
                if (bTargetPositionY)
                {
                    posCurrentPositionY = positionY.up;
                }
            }
            else
            {
                bTargetPositionY = MyFunctions.Move.Translate(gameObject, "y", -fMetresPerFrameY, transform.position.y, fLowerPositionY, bTargetPositionY);
                if (bTargetPositionY)
                {
                    posCurrentPositionY = positionY.down;
                }
            }
        }
    }

    // // ------------------------------------------------------------------------------------------------
    //
    // private bool Translate(
    //     string sAxis,
    //     float fMetresPerFrame,
    //     float fCurrentPosition,
    //     float fTargetPosition,
    //     bool bTargetPosition
    // )
    // {
    //     if (((fMetresPerFrame > 0f) && (fTargetPosition > (fCurrentPosition + fMetresPerFrame)))
    //     ||  ((fMetresPerFrame < 0f) && (fTargetPosition < (fCurrentPosition + fMetresPerFrame))))
    //     {
    //         switch(sAxis)
    //         {
    //             case "x":
    //                 transform.Translate(fMetresPerFrame, 0f, 0f, Space.World);
    //                 break;
    //             case "y":
    //                 transform.Translate(0f, fMetresPerFrame, 0f, Space.World);
    //                 break;
    //             case "z":
    //                 transform.Translate(0f, 0f, fMetresPerFrame, Space.World);
    //                 break;
    //         }
    //     }
    //     else
    //     {
    //         switch(sAxis)
    //         {
    //             case "x":
    //                 transform.Translate(fTargetPosition - fCurrentPosition, 0f, 0f, Space.World);
    //                 break;
    //             case "y":
    //                 transform.Translate(0f, fTargetPosition - fCurrentPosition, 0f, Space.World);
    //                 break;
    //             case "z":
    //                 transform.Translate(0f, 0f, fTargetPosition - fCurrentPosition, Space.World);
    //                 break;
    //         }
    //         bTargetPosition = true;
    //     }
    //     return(bTargetPosition);
    // }
    //
    // // ------------------------------------------------------------------------------------------------

    public void Reset()
    {
        bTargetPositionY = (posCurrentPositionY == posStartPositionY);
    }

    // ------------------------------------------------------------------------------------------------

    public void Switch()
    {
        bTargetPositionY = false;
    }

    // ------------------------------------------------------------------------------------------------

}
