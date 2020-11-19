// using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using MyFunctions;

public class CreditsController : MonoBehaviour
{
    private GameManager gameManager;
    private Camera camMainCamera;
    // private GameObject goEndcap;

    // For world-space:
    // private float fSpeed = 2.5f;
    // private float fPositionYStop = 60f; // World-space value

    // For screen-space overlay:
    private float fMetresPositionYStartReference = -500f;
    private float fMetresPositionYStart;
    private float fMetresPositionYFinishReference = 2500f;
    private float fMetresPositionYFinish;
    private float fMetresPositionYTargetNow;
    private float fMetresPerSecReference = 50f;
    private float fMetresPerSec;
    // private float fMetresPerFrame;
    // private float fMetresPositionDeltaCurrent;

    private int iMainCameraPixelWidthReference = 1000;
    private int iMainCameraPixelWidthLastFrame;

    private int iDirection;
    private bool bChangeState;
    private float fScale;
    private float fTimeStart;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        camMainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        // goEndcap = transform.Find("Endcap").gameObject;

        if (fMetresPositionYFinishReference > fMetresPositionYStartReference)
        {
            iDirection = 1;
        }
        else if (fMetresPositionYFinishReference < fMetresPositionYStartReference)
        {
            iDirection = -1;
        }
        else
        {
            iDirection = 0;
        }

        bChangeState = (iDirection != 0);

        Scale();
        transform.localPosition = new Vector3(0f, fMetresPositionYStart, 0f);
        fTimeStart = Time.time;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        // fMetresPerFrame = fMetresPerSec * Time.deltaTime;
        // fMetresPositionDeltaCurrent = camMainCamera.pixelHeight - goEndcap.transform.position.y;
        //
        // if (Math.Abs(fMetresPositionDeltaCurrent) > fMetresPerFrame)
        // {
        //     transform.Translate(fMetresPerFrame * Vector3.up);
        // }
        // else
        // {
        //     // These lines were used when the credits screen was in world-space and incorporated the restart
        //     // button. This method was abandoned in favour of using screen-space overlay for the credits, as
        //     // previously we were seeing certain entities appear brightly through the transparent panel,
        //     // unknown reason.
        //         // transform.parent.Find("Button : Restart").gameObject.SetActive(true);
        //         // gameManager.bActiveScreenButton = true;
        //     transform.Translate(fMetresPositionDeltaCurrent * Vector3.up);
        //     gameManager.goScreenRestart.transform.Find("Button : Restart").gameObject.SetActive(true);
        //     gameManager.bActiveScreenButton = true;
        //     gameManager.bCompleted = true;
        //     gameManager.goScreenCredits.SetActive(false);
        // }

        // fMetresPerFrame = fMetresPerSec * Time.deltaTime;
        // bChangeState = MyFunctions.Move.Translate(
        //     gameObject,
        //     "y",
        //     fMetresPerFrame,
        //     camMainCamera.pixelHeight - goEndcap.transform.position.y
        // );

        // This latest active version of the code intentionally does not use the general translate
        // function, but rather uses a slightly more convoluted approach in order to cater for possible
        // screen rescaling as the credits are rolling. This is needed as the credits are in screen-space
        // rather than world-space (necessary to avoid a display glitch), so things don't rescale
        // automatically if the display size is adjusted. Although this is perhaps unlikely, it could
        // happen nonetheless, so it has been catered for herein.

        if (iMainCameraPixelWidthLastFrame != camMainCamera.pixelWidth)
        {
            Scale();
        }
        Translate();

        // When the credits reach the target destination, then activate the restart button and mark the
        // game as completed. The credits will then not be activated again upon completing further
        // play-throughs. Even if they were, they wouldn't roll as we aren't resetting their position.

        if (!bChangeState)
        {
            gameManager.goScreenRestart.transform.Find("Button : Restart").gameObject.SetActive(true);
            gameManager.bActiveScreenButton = true;
            gameManager.bCompleted = true;
            gameManager.goScreenCredits.SetActive(false);
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void Scale()
    {
        fScale = (float)camMainCamera.pixelWidth / (float)iMainCameraPixelWidthReference;
        fMetresPositionYStart = fScale * fMetresPositionYStartReference;
        fMetresPositionYFinish = fScale * fMetresPositionYFinishReference;
        fMetresPerSec = fScale * fMetresPerSecReference;
        transform.localScale = new Vector3(fScale, fScale, 1f);
        iMainCameraPixelWidthLastFrame = camMainCamera.pixelWidth;
    }

    // ------------------------------------------------------------------------------------------------

    private void Translate()
    {
        // Determine where the game object should be were it to have been translated at the current scale
        // settings since the start, then translate to this target point. This caters for both the regular
        // rolling credits motion and also any dynamic display size adjustment.

        fMetresPositionYTargetNow = fMetresPositionYStart + (iDirection * fMetresPerSec * (Time.time - fTimeStart));

        if (    ((iDirection == 1) && (fMetresPositionYTargetNow > fMetresPositionYFinish))
            ||  ((iDirection == -1) && (fMetresPositionYTargetNow < fMetresPositionYFinish)) )
        {
            fMetresPositionYTargetNow = fMetresPositionYFinish;
            bChangeState = false;
        }

        transform.Translate((fMetresPositionYTargetNow - transform.localPosition.y) * Vector3.up, Space.Self);
    }

    // ------------------------------------------------------------------------------------------------

}
