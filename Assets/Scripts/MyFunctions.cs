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
            float fTargetPosition
        )
        {
            if (    ((fMetresPerFrame > 0f) && (fTargetPosition > (fCurrentPosition + fMetresPerFrame)))
                ||  ((fMetresPerFrame < 0f) && (fTargetPosition < (fCurrentPosition + fMetresPerFrame))) )
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
                return(true);
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
                return(false);
            }
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
