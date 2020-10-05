using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFunctions
{
    public class Move
    {

        // ------------------------------------------------------------------------------------------------

        public static bool Translate(
            GameObject go,
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
                        go.transform.Translate(fMetresPerFrame, 0f, 0f, Space.World);
                        break;
                    case "y":
                        go.transform.Translate(0f, fMetresPerFrame, 0f, Space.World);
                        break;
                    case "z":
                        go.transform.Translate(0f, 0f, fMetresPerFrame, Space.World);
                        break;
                }
            }
            else
            {
                switch(sAxis)
                {
                    case "x":
                        go.transform.Translate(fTargetPosition - fCurrentPosition, 0f, 0f, Space.World);
                        break;
                    case "y":
                        go.transform.Translate(0f, fTargetPosition - fCurrentPosition, 0f, Space.World);
                        break;
                    case "z":
                        go.transform.Translate(0f, 0f, fTargetPosition - fCurrentPosition, Space.World);
                        break;
                }
                bTargetPosition = true;
            }
            return(bTargetPosition);
        }

        // ------------------------------------------------------------------------------------------------

        public static Tuple<bool, Vector3> Rotate(
            GameObject go,
            Vector3 v3EulerAngles,
            string sAxis,
            float fDegreesPerFrame,
            float fCurrentRotation,
            float fTargetRotation,
            bool bTargetRotation
        )
        {
            if (((fDegreesPerFrame > 0f) && (fTargetRotation > (fCurrentRotation + fDegreesPerFrame)))
            ||  ((fDegreesPerFrame < 0f) && (fTargetRotation < (fCurrentRotation + fDegreesPerFrame))))
            {
                switch(sAxis)
                {
                    case "x":
                        go.transform.Rotate(fDegreesPerFrame, 0f, 0f, Space.World);
                        v3EulerAngles.x += fDegreesPerFrame;
                        break;
                    case "y":
                        go.transform.Rotate(0f, fDegreesPerFrame, 0f, Space.World);
                        v3EulerAngles.y += fDegreesPerFrame;
                        break;
                    case "z":
                        go.transform.Rotate(0f, 0f, fDegreesPerFrame, Space.World);
                        v3EulerAngles.z += fDegreesPerFrame;
                        break;
                }
            }
            else
            {
                switch(sAxis)
                {
                    case "x":
                        go.transform.Rotate(fTargetRotation - fCurrentRotation, 0f, 0f, Space.World);
                        v3EulerAngles.x = fTargetRotation;
                        break;
                    case "y":
                        go.transform.Rotate(0f, fTargetRotation - fCurrentRotation, 0f, Space.World);
                        v3EulerAngles.y = fTargetRotation;
                        break;
                    case "z":
                        go.transform.Rotate(0f, 0f, fTargetRotation - fCurrentRotation, Space.World);
                        v3EulerAngles.z = fTargetRotation;
                        break;
                }
                go.transform.eulerAngles = new Vector3(
                    Mathf.Round(go.transform.eulerAngles.x),
                    Mathf.Round(go.transform.eulerAngles.y),
                    Mathf.Round(go.transform.eulerAngles.z)
                );
                bTargetRotation = true;
            }
            return(new Tuple<bool, Vector3>(bTargetRotation, v3EulerAngles));
        }

        // ------------------------------------------------------------------------------------------------

    }
}
