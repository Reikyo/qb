using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFunctions
{
    public class Move
    {

        // ------------------------------------------------------------------------------------------------

        // public static bool Translate(
        //     string sAxis,
        //     float fMetresPerFrame,
        //     float fMetresPositionTarget,
        //     GameObject goMove,
        //     GameObject goMeasure = null
        // )
        // {
        //     if (!goMeasure)
        //     {
        //         goMeasure = goMove;
        //     }
        //
        //     // ------------------------------------------------------------------------------------------------
        //
        //     int iDirection;
        //     float fMetresPositionDeltaCurrent = 0f;
        //
        //     // ------------------------------------------------------------------------------------------------
        //
        //     switch(sAxis)
        //     {
        //         case "x":
        //             fMetresPositionDeltaCurrent = fMetresPositionTarget - goMeasure.transform.position.x;
        //             break;
        //         case "y":
        //             fMetresPositionDeltaCurrent = fMetresPositionTarget - goMeasure.transform.position.y;
        //             break;
        //         case "z":
        //             fMetresPositionDeltaCurrent = fMetresPositionTarget - goMeasure.transform.position.z;
        //             break;
        //     }
        //
        //     // ------------------------------------------------------------------------------------------------
        //
        //     if (Math.Abs(fMetresPositionDeltaCurrent) > fMetresPerFrame)
        //     {
        //         if (fMetresPositionDeltaCurrent > 0f)
        //         {
        //             iDirection = 1;
        //         }
        //         else
        //         {
        //             iDirection = -1;
        //         }
        //         switch(sAxis)
        //         {
        //             case "x":
        //                 goMove.transform.Translate(iDirection * fMetresPerFrame, 0f, 0f, Space.World);
        //                 break;
        //             case "y":
        //                 goMove.transform.Translate(0f, iDirection * fMetresPerFrame, 0f, Space.World);
        //                 break;
        //             case "z":
        //                 goMove.transform.Translate(0f, 0f, iDirection * fMetresPerFrame, Space.World);
        //                 break;
        //         }
        //         return(true);
        //     }
        //     else
        //     {
        //         switch(sAxis)
        //         {
        //             case "x":
        //                 goMove.transform.Translate(fMetresPositionDeltaCurrent, 0f, 0f, Space.World);
        //                 break;
        //             case "y":
        //                 goMove.transform.Translate(0f, fMetresPositionDeltaCurrent, 0f, Space.World);
        //                 break;
        //             case "z":
        //                 goMove.transform.Translate(0f, 0f, fMetresPositionDeltaCurrent, Space.World);
        //                 break;
        //         }
        //         return(false);
        //     }
        //
        //     // ------------------------------------------------------------------------------------------------
        //
        // }
        //
        // // ------------------------------------------------------------------------------------------------

        public static bool Translate(
            GameObject go,
            string sAxis,
            float fMetresPerFrame,
            float fMetresPositionDeltaCurrent
        )
        {
            int iDirection;

            // ------------------------------------------------------------------------------------------------

            if (Math.Abs(fMetresPositionDeltaCurrent) > fMetresPerFrame)
            {
                if (fMetresPositionDeltaCurrent > 0f)
                {
                    iDirection = 1;
                }
                else
                {
                    iDirection = -1;
                }
                switch(sAxis)
                {
                    case "x":
                        go.transform.Translate(iDirection * fMetresPerFrame, 0f, 0f, Space.World);
                        break;
                    case "y":
                        go.transform.Translate(0f, iDirection * fMetresPerFrame, 0f, Space.World);
                        break;
                    case "z":
                        go.transform.Translate(0f, 0f, iDirection * fMetresPerFrame, Space.World);
                        break;
                }
                return(true);
            }
            else
            {
                switch(sAxis)
                {
                    case "x":
                        go.transform.Translate(fMetresPositionDeltaCurrent, 0f, 0f, Space.World);
                        break;
                    case "y":
                        go.transform.Translate(0f, fMetresPositionDeltaCurrent, 0f, Space.World);
                        break;
                    case "z":
                        go.transform.Translate(0f, 0f, fMetresPositionDeltaCurrent, Space.World);
                        break;
                }
                return(false);
            }

            // ------------------------------------------------------------------------------------------------

        }

        // ------------------------------------------------------------------------------------------------

        public static Tuple<bool, float> Rotate(
            GameObject go,
            string sAxis,
            float fDegreesPerFrame,
            float fCurrentRotation,
            float fTargetRotation
        )
        {
            if (    ((fDegreesPerFrame > 0f) && (fTargetRotation > (fCurrentRotation + fDegreesPerFrame)))
                ||  ((fDegreesPerFrame < 0f) && (fTargetRotation < (fCurrentRotation + fDegreesPerFrame))) )
            {
                switch(sAxis)
                {
                    case "x":
                        go.transform.Rotate(fDegreesPerFrame, 0f, 0f, Space.World);
                        fCurrentRotation += fDegreesPerFrame;
                        break;
                    case "y":
                        go.transform.Rotate(0f, fDegreesPerFrame, 0f, Space.World);
                        fCurrentRotation += fDegreesPerFrame;
                        break;
                    case "z":
                        go.transform.Rotate(0f, 0f, fDegreesPerFrame, Space.World);
                        fCurrentRotation += fDegreesPerFrame;
                        break;
                }
                return(new Tuple<bool, float>(true, fCurrentRotation));
            }
            else
            {
                switch(sAxis)
                {
                    case "x":
                        go.transform.Rotate(fTargetRotation - fCurrentRotation, 0f, 0f, Space.World);
                        fCurrentRotation = fTargetRotation;
                        break;
                    case "y":
                        go.transform.Rotate(0f, fTargetRotation - fCurrentRotation, 0f, Space.World);
                        fCurrentRotation = fTargetRotation;
                        break;
                    case "z":
                        go.transform.Rotate(0f, 0f, fTargetRotation - fCurrentRotation, Space.World);
                        fCurrentRotation = fTargetRotation;
                        break;
                }
                go.transform.eulerAngles = new Vector3(
                    Mathf.Round(go.transform.eulerAngles.x),
                    Mathf.Round(go.transform.eulerAngles.y),
                    Mathf.Round(go.transform.eulerAngles.z)
                );
                return(new Tuple<bool, float>(false, fCurrentRotation));
            }
        }

        // ------------------------------------------------------------------------------------------------

    }
}
