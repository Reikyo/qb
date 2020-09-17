using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Vector3 v3TitleScreenPosition = new Vector3(0f, -210f, 72f);
    private Vector3 v3FirstLevelPosition = new Vector3(0f, -25f, 0f);

    private Vector3 v3TitleScreenEulerAngles = new Vector3(237.5f, 0f, 0f);
    private Vector3 v3FirstLevelEulerAngles = new Vector3(0f, 0f, 0f);
    private Vector3 v3NextLevelEulerAngles = new Vector3(0f, 0f, 0f);
    private Vector3 v3EulerAngles;

    private float fFirstLevelStartTime = 1f;
    private float fNextLevelStartTime = 0.5f;

    private float fFirstLevelStartMetresPerSecY;
    private float fFirstLevelStartMetresPerFrameY;
    private float fFirstLevelStartMetresPerSecZ;
    private float fFirstLevelStartMetresPerFrameZ;

    private float fFirstLevelStartDegreesPerSec;
    private float fFirstLevelStartDegreesPerFrame;
    private float fNextLevelStartDegreesPerSec;
    private float fNextLevelStartDegreesPerFrame;

    private bool bFirstLevelStart = false;
    private bool bNextLevelStart = false;

    private bool bFirstLevelPositionY = false;
    private bool bFirstLevelPositionZ = false;
    private bool bFirstLevelEulerAngleX = false;

    private string sNextLevelStartRotationAxis = "z";

    public GameObject goSpawnManager;
    public GameObject[] goObstacles;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        fFirstLevelStartMetresPerSecY = (v3FirstLevelPosition.y - v3TitleScreenPosition.y) / fFirstLevelStartTime;
        fFirstLevelStartMetresPerFrameY = fFirstLevelStartMetresPerSecY * Time.deltaTime;
        fFirstLevelStartMetresPerSecZ = (v3FirstLevelPosition.z - v3TitleScreenPosition.z) / fFirstLevelStartTime;
        fFirstLevelStartMetresPerFrameZ = fFirstLevelStartMetresPerSecZ * Time.deltaTime;

        fFirstLevelStartDegreesPerSec = (v3FirstLevelEulerAngles.x - v3TitleScreenEulerAngles.x) / fFirstLevelStartTime;
        fFirstLevelStartDegreesPerFrame = fFirstLevelStartDegreesPerSec * Time.deltaTime;
        fNextLevelStartDegreesPerSec = 90f / fNextLevelStartTime;
        fNextLevelStartDegreesPerFrame = fNextLevelStartDegreesPerSec * Time.deltaTime;

        transform.position = v3TitleScreenPosition;
        transform.eulerAngles = v3TitleScreenEulerAngles;
        v3EulerAngles = v3TitleScreenEulerAngles;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

        // ------------------------------------------------------------------------------------------------

        if (bFirstLevelStart)
        {
            if (!bFirstLevelPositionY)
            {
                if (((fFirstLevelStartMetresPerFrameY > 0f) && (v3FirstLevelPosition.y > (transform.position.y + fFirstLevelStartMetresPerFrameY)))
                ||  ((fFirstLevelStartMetresPerFrameY < 0f) && (v3FirstLevelPosition.y < (transform.position.y + fFirstLevelStartMetresPerFrameY))))
                {
                    transform.Translate(0f, fFirstLevelStartMetresPerFrameY, 0f, Space.World);
                }
                else
                {
                    transform.Translate(0f, v3FirstLevelPosition.y - transform.position.y, 0f, Space.World);
                    bFirstLevelPositionY = true;
                }
            }
            if (!bFirstLevelPositionZ)
            {
                if (((fFirstLevelStartMetresPerFrameZ > 0f) && (v3FirstLevelPosition.z > (transform.position.z + fFirstLevelStartMetresPerFrameZ)))
                ||  ((fFirstLevelStartMetresPerFrameZ < 0f) && (v3FirstLevelPosition.z < (transform.position.z + fFirstLevelStartMetresPerFrameZ))))
                {
                    transform.Translate(0f, 0f, fFirstLevelStartMetresPerFrameZ, Space.World);
                }
                else
                {
                    transform.Translate(0f, 0f, v3FirstLevelPosition.z - transform.position.z, Space.World);
                    bFirstLevelPositionZ = true;
                }
            }
            if (!bFirstLevelEulerAngleX)
            {
                if (((fFirstLevelStartDegreesPerFrame > 0f) && (v3FirstLevelEulerAngles.x > (v3EulerAngles.x + fFirstLevelStartDegreesPerFrame)))
                ||  ((fFirstLevelStartDegreesPerFrame < 0f) && (v3FirstLevelEulerAngles.x < (v3EulerAngles.x + fFirstLevelStartDegreesPerFrame))))
                {
                    transform.Rotate(fFirstLevelStartDegreesPerFrame, 0f, 0f, Space.World);
                    v3EulerAngles.x += fFirstLevelStartDegreesPerFrame;
                }
                else
                {
                    transform.Rotate(v3FirstLevelEulerAngles.x - v3EulerAngles.x, 0f, 0f, Space.World);
                    v3EulerAngles.x = v3FirstLevelEulerAngles.x;
                    bFirstLevelEulerAngleX = true;
                }
            }
            if (bFirstLevelPositionY && bFirstLevelPositionZ && bFirstLevelEulerAngleX)
            {
                transform.eulerAngles = new Vector3(Mathf.Round(transform.eulerAngles.x), Mathf.Round(transform.eulerAngles.y), Mathf.Round(transform.eulerAngles.z));
                bFirstLevelStart = false;
                Activate();
            }
        }

        // ------------------------------------------------------------------------------------------------

        if (bNextLevelStart)
        {
            if (sNextLevelStartRotationAxis == "z")
            {
                if (((fNextLevelStartDegreesPerFrame > 0f) && (v3NextLevelEulerAngles.z > (v3EulerAngles.z + fNextLevelStartDegreesPerFrame)))
                ||  ((fNextLevelStartDegreesPerFrame < 0f) && (v3NextLevelEulerAngles.z < (v3EulerAngles.z + fNextLevelStartDegreesPerFrame))))
                {
                    transform.Rotate(0f, 0f, fNextLevelStartDegreesPerFrame, Space.World);
                    v3EulerAngles.z += fNextLevelStartDegreesPerFrame;
                }
                else
                {
                    transform.Rotate(0f, 0f, v3NextLevelEulerAngles.z - v3EulerAngles.z, Space.World);
                    transform.eulerAngles = new Vector3(Mathf.Round(transform.eulerAngles.x), Mathf.Round(transform.eulerAngles.y), Mathf.Round(transform.eulerAngles.z));
                    v3EulerAngles.z = v3NextLevelEulerAngles.z;
                    sNextLevelStartRotationAxis = "x";
                    bNextLevelStart = false;
                    Activate();
                }
            }
            else if (sNextLevelStartRotationAxis == "x")
            {
                if (((fNextLevelStartDegreesPerFrame > 0f) && (v3NextLevelEulerAngles.x > (v3EulerAngles.x + fNextLevelStartDegreesPerFrame)))
                ||  ((fNextLevelStartDegreesPerFrame < 0f) && (v3NextLevelEulerAngles.x < (v3EulerAngles.x + fNextLevelStartDegreesPerFrame))))
                {
                    transform.Rotate(fNextLevelStartDegreesPerFrame, 0f, 0f, Space.World);
                    v3EulerAngles.x += fNextLevelStartDegreesPerFrame;
                }
                else
                {
                    transform.Rotate(v3NextLevelEulerAngles.x - v3EulerAngles.x, 0f, 0f, Space.World);
                    transform.eulerAngles = new Vector3(Mathf.Round(transform.eulerAngles.x), Mathf.Round(transform.eulerAngles.y), Mathf.Round(transform.eulerAngles.z));
                    v3EulerAngles.x = v3NextLevelEulerAngles.x;
                    sNextLevelStartRotationAxis = "z";
                    bNextLevelStart = false;
                    Activate();
                }
            }
        }

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

    public void FirstLevelStart()
    {
        bFirstLevelStart = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void NextLevelStart()
    {
        bNextLevelStart = true;

        foreach (GameObject goObstacle in goObstacles)
        {
            goObstacle.SetActive(false);
        }

        if (sNextLevelStartRotationAxis == "z")
        {
            v3NextLevelEulerAngles.z += 90f;
            v3NextLevelEulerAngles.z = Mathf.Round(v3NextLevelEulerAngles.z);
        }
        else if (sNextLevelStartRotationAxis == "x")
        {
            v3NextLevelEulerAngles.x += 90f;
            v3NextLevelEulerAngles.x = Mathf.Round(v3NextLevelEulerAngles.x);
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void Activate()
    {
        foreach (GameObject goObstacle in goObstacles)
        {
            goObstacle.SetActive(true);
        }
        goSpawnManager.GetComponent<SpawnManager>().Instantiate();
    }

    // ------------------------------------------------------------------------------------------------

}
