using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    // private float fStartPositionY;
    private float fLowerPositionY = 2f;
    private float fUpperPositionY = 6f;

    private float fTransitionTime = 0.5f;

    private float fMetresPerSecY;
    private float fMetresPerFrameY;

    public bool bTargetPositionY = true;
    private bool bTranslateUp = true;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // fStartPositionY = transform.position.y;

        fMetresPerSecY = (fUpperPositionY - fLowerPositionY) / fTransitionTime;
        fMetresPerFrameY = fMetresPerSecY * Time.deltaTime;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (!bTargetPositionY)
        {
            if (bTranslateUp)
            {
                bTargetPositionY = Translate("y", fMetresPerFrameY, transform.position.y, fUpperPositionY, bTargetPositionY);
                if (bTargetPositionY)
                {
                    bTranslateUp = false;
                }
            }
            else
            {
                bTargetPositionY = Translate("y", -fMetresPerFrameY, transform.position.y, fLowerPositionY, bTargetPositionY);
                if (bTargetPositionY)
                {
                    bTranslateUp = true;
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private bool Translate(
        string sAxis,
        float fMetresPerFrame,
        float fCurrentPosition,
        float fTargetPosition,
        bool bTargetPosition
    )
    {
        if (((fMetresPerFrame > 0f) && (fTargetPosition > (fCurrentPosition + fMetresPerFrame)))
        ||  ((fMetresPerFrame < 0f) && (fTargetPosition < (fCurrentPosition + fMetresPerFrame))))
        {
            switch(sAxis)
            {
                case "x":
                    transform.Translate(fMetresPerFrame, 0f, 0f, Space.World);
                    break;
                case "y":
                    transform.Translate(0f, fMetresPerFrame, 0f, Space.World);
                    break;
                case "z":
                    transform.Translate(0f, 0f, fMetresPerFrame, Space.World);
                    break;
            }
        }
        else
        {
            switch(sAxis)
            {
                case "x":
                    transform.Translate(fTargetPosition - fCurrentPosition, 0f, 0f, Space.World);
                    break;
                case "y":
                    transform.Translate(0f, fTargetPosition - fCurrentPosition, 0f, Space.World);
                    break;
                case "z":
                    transform.Translate(0f, 0f, fTargetPosition - fCurrentPosition, Space.World);
                    break;
            }
            bTargetPosition = true;
        }
        return(bTargetPosition);
    }

    // ------------------------------------------------------------------------------------------------

}
