using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;

public class CreditsController : MonoBehaviour
{
    private GameManager gameManager;
    private Camera camMainCamera;
    private GameObject goEndcap;

    // For world-space:
    // private float fSpeed = 2.5f;
    // private float fPositionYStop = 60f; // World-space value

    // For screen-space overlay:
    private float fMetresPerSec = 50f;
    private float fMetresPerFrame;
    private float fMetresPositionDeltaCurrent;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        camMainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        goEndcap = transform.Find("Endcap").gameObject;
        // Debug.Log(camMainCamera.pixelWidth);
        // Debug.Log(camMainCamera.pixelHeight);
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        fMetresPerFrame = fMetresPerSec * Time.deltaTime;
        fMetresPositionDeltaCurrent = camMainCamera.pixelHeight - goEndcap.transform.position.y;

        if (Math.Abs(fMetresPositionDeltaCurrent) > fMetresPerFrame)
        {
            transform.Translate(fMetresPerFrame * Vector3.up);
        }
        else
        {
            // These lines were used when the credits screen was in world-space and incorporated the restart
            // button. This method was abandoned in favour of using screen-space overlay for the credits, as
            // previously we were seeing certain entities appear brightly through the transparent panel,
            // unknown reason.
                // transform.parent.Find("Button : Restart").gameObject.SetActive(true);
                // gameManager.bActiveScreenButton = true;
            transform.Translate(fMetresPositionDeltaCurrent * Vector3.up);
            gameManager.goScreenRestart.transform.Find("Button : Restart").gameObject.SetActive(true);
            gameManager.bActiveScreenButton = true;
            gameManager.bCompleted = true;
            gameManager.goScreenCredits.SetActive(false);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
