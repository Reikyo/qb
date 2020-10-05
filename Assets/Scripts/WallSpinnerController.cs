using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using MyFunctions;

public class WallSpinnerController : MonoBehaviour
{
    private bool bRotate = false;
    private int iRotationDirection;
    private float fDegreesPerSec = 180f;
    private float fDegreesPerFrame;
    private float fDegreesToRotate = 90f;
    private float fDegreesRotated = 0f;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {

    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        // if (!bNextEulerAngleY)
        // {
        //     var tuple = MyFunctions.Move.Rotate(gameObject, v3EulerAngles, "y", fDegreesPerFrame, v3EulerAngles.y, v3NextEulerAngles.y, bNextEulerAngleY);
        //     bNextLevelEulerAngleY = tuple.Item1;
        //     v3EulerAngles = tuple.Item2;
        // }

        // if (bRotate)
        // {
        //     fDegreesPerFrame = fDegreesPerSec * Time.deltaTime;
        //
        //     if ((fDegreesRotated + fDegreesPerFrame) > fDegreesToRotate)
        //     {
        //         fDegreesPerFrame = fDegreesToRotate - fDegreesRotated;
        //         bRotate = false;
        //     }
        //
        //     transform.Rotate(iRotationDirection * fDegreesPerFrame * Vector3.up);
        //     fDegreesRotated += fDegreesPerFrame;
        //
        //     if (!bRotate)
        //     {
        //         transform.eulerAngles = new Vector3(
        //             Mathf.Round(transform.eulerAngles.x),
        //             Mathf.Round(transform.eulerAngles.y),
        //             Mathf.Round(transform.eulerAngles.z)
        //         );
        //         fDegreesRotated = 0f;
        //     }
        // }

        if (bRotate)
        {
            fDegreesPerFrame = fDegreesPerSec * Time.deltaTime;
            fDegreesRotated += fDegreesPerFrame;

            if (fDegreesRotated > fDegreesToRotate)
            {
                fDegreesRotated -= fDegreesPerFrame;
                fDegreesPerFrame = fDegreesToRotate - fDegreesRotated;
                bRotate = false;
            }

            transform.Rotate(iRotationDirection * fDegreesPerFrame * Vector3.up);

            if (!bRotate)
            {
                transform.eulerAngles = new Vector3(
                    Mathf.Round(transform.eulerAngles.x),
                    Mathf.Round(transform.eulerAngles.y),
                    Mathf.Round(transform.eulerAngles.z)
                );
                fDegreesRotated = 0f;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Rotate(int iRotationDirectionGiven)
    {
        // Only trigger rotation if not already rotating
        if (!bRotate)
        {
            bRotate = true;
            iRotationDirection = iRotationDirectionGiven;
        }
    }

    // ------------------------------------------------------------------------------------------------

}
