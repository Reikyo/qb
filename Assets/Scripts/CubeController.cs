using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeController : MonoBehaviour
{
    // private Vector3 v3InstantiatePosition = new Vector3(0f, -210f, 72f);
    private Vector3 v3InstantiatePosition = new Vector3(0f, -230f, 19f);
    private Vector3 v3FirstLevelPosition = new Vector3(0f, -25f, 0f);

    // private Vector3 v3InstantiateEulerAngles = new Vector3(237.5f, 0f, 0f);
    private Vector3 v3InstantiateEulerAngles = new Vector3(253f, 0f, 0f);
    private Vector3 v3FirstLevelEulerAngles = new Vector3(0f, 0f, 0f);
    private Vector3 v3NextLevelEulerAngles = new Vector3(0f, 0f, 90f);
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

    public bool bFirstLevelStart = false;
    public bool bNextLevelStart = false;

    private bool bFirstLevelPositionY = false;
    private bool bFirstLevelPositionZ = false;
    private bool bFirstLevelEulerAngleX = false;
    private bool bNextLevelEulerAngleX = false;
    private bool bNextLevelEulerAngleZ = false;

    private string sNextLevelStartRotationAxis = "z";

    private int iLevel = 0;
    public TextMeshProUGUI guiLevel;
    public TextMeshProUGUI guiNumProjectile;
    public GameObject[] goLevels;
    private GameObject[] goWallsDestructible;
    private GameObject[] goWallsMoveable;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        transform.position = v3InstantiatePosition;
        transform.eulerAngles = v3InstantiateEulerAngles;
        v3EulerAngles = v3InstantiateEulerAngles;

        fFirstLevelStartMetresPerSecY = (v3FirstLevelPosition.y - v3InstantiatePosition.y) / fFirstLevelStartTime;
        fFirstLevelStartMetresPerFrameY = fFirstLevelStartMetresPerSecY * Time.deltaTime;
        fFirstLevelStartMetresPerSecZ = (v3FirstLevelPosition.z - v3InstantiatePosition.z) / fFirstLevelStartTime;
        fFirstLevelStartMetresPerFrameZ = fFirstLevelStartMetresPerSecZ * Time.deltaTime;

        fFirstLevelStartDegreesPerSec = (v3FirstLevelEulerAngles.x - v3InstantiateEulerAngles.x) / fFirstLevelStartTime;
        fFirstLevelStartDegreesPerFrame = fFirstLevelStartDegreesPerSec * Time.deltaTime;
        fNextLevelStartDegreesPerSec = 90f / fNextLevelStartTime;
        fNextLevelStartDegreesPerFrame = fNextLevelStartDegreesPerSec * Time.deltaTime;
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
                bFirstLevelPositionY = Translate("y", fFirstLevelStartMetresPerFrameY, transform.position.y, v3FirstLevelPosition.y, bFirstLevelPositionY);
            }
            if (!bFirstLevelPositionZ)
            {
                bFirstLevelPositionZ = Translate("z", fFirstLevelStartMetresPerFrameZ, transform.position.z, v3FirstLevelPosition.z, bFirstLevelPositionZ);
            }
            if (!bFirstLevelEulerAngleX)
            {
                bFirstLevelEulerAngleX = Rotate("x", fFirstLevelStartDegreesPerFrame, v3EulerAngles.x, v3FirstLevelEulerAngles.x, bFirstLevelEulerAngleX);
            }
            if (bFirstLevelPositionY
            &&  bFirstLevelPositionZ
            &&  bFirstLevelEulerAngleX)
            {
                bFirstLevelPositionY = false;
                bFirstLevelPositionZ = false;
                bFirstLevelEulerAngleX = false;
                bFirstLevelStart = false;
                Activate();
            }
        }

        // ------------------------------------------------------------------------------------------------

        if (bNextLevelStart)
        {
            if (sNextLevelStartRotationAxis == "z")
            {
                if (!bNextLevelEulerAngleZ)
                {
                    bNextLevelEulerAngleZ = Rotate("z", fNextLevelStartDegreesPerFrame, v3EulerAngles.z, v3NextLevelEulerAngles.z, bNextLevelEulerAngleZ);
                }
                if (bNextLevelEulerAngleZ)
                {
                    sNextLevelStartRotationAxis = "x";
                    v3NextLevelEulerAngles.x += 90f;
                    v3NextLevelEulerAngles.x = Mathf.Round(v3NextLevelEulerAngles.x);
                    bNextLevelEulerAngleZ = false;
                    bNextLevelStart = false;
                    Activate();
                }
            }
            else if (sNextLevelStartRotationAxis == "x")
            {
                if (!bNextLevelEulerAngleX)
                {
                    bNextLevelEulerAngleX = Rotate("x", fNextLevelStartDegreesPerFrame, v3EulerAngles.x, v3NextLevelEulerAngles.x, bNextLevelEulerAngleX);
                }
                if (bNextLevelEulerAngleX)
                {
                    sNextLevelStartRotationAxis = "z";
                    v3NextLevelEulerAngles.z += 90f;
                    v3NextLevelEulerAngles.z = Mathf.Round(v3NextLevelEulerAngles.z);
                    bNextLevelEulerAngleX = false;
                    bNextLevelStart = false;
                    Activate();
                }
            }
        }

        // ------------------------------------------------------------------------------------------------

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

    private bool Rotate(
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
                    transform.Rotate(fDegreesPerFrame, 0f, 0f, Space.World);
                    v3EulerAngles.x += fDegreesPerFrame;
                    break;
                case "y":
                    transform.Rotate(0f, fDegreesPerFrame, 0f, Space.World);
                    v3EulerAngles.y += fDegreesPerFrame;
                    break;
                case "z":
                    transform.Rotate(0f, 0f, fDegreesPerFrame, Space.World);
                    v3EulerAngles.z += fDegreesPerFrame;
                    break;
            }
        }
        else
        {
            switch(sAxis)
            {
                case "x":
                    transform.Rotate(fTargetRotation - fCurrentRotation, 0f, 0f, Space.World);
                    v3EulerAngles.x = fTargetRotation;
                    break;
                case "y":
                    transform.Rotate(0f, fTargetRotation - fCurrentRotation, 0f, Space.World);
                    v3EulerAngles.y = fTargetRotation;
                    break;
                case "z":
                    transform.Rotate(0f, 0f, fTargetRotation - fCurrentRotation, Space.World);
                    v3EulerAngles.z = fTargetRotation;
                    break;
            }
            transform.eulerAngles = new Vector3(
                Mathf.Round(transform.eulerAngles.x),
                Mathf.Round(transform.eulerAngles.y),
                Mathf.Round(transform.eulerAngles.z)
            );
            bTargetRotation = true;
        }
        return(bTargetRotation);
    }

    // ------------------------------------------------------------------------------------------------

    public void FirstLevelStart()
    {
        bFirstLevelStart = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void NextLevelStart()
    {
        goLevels[iLevel].GetComponent<LevelController>().LevelFinish();
        iLevel += 1;
    }

    // ------------------------------------------------------------------------------------------------

    public void ThisLevelRestart()
    {
        foreach (GameObject goWallDestructible in goWallsDestructible)
        {
            goWallDestructible.SetActive(true);
        }
        foreach (GameObject goWallMoveable in goWallsMoveable)
        {
            goWallMoveable.GetComponent<WallController>().Reset();
        }
        goLevels[iLevel].GetComponent<LevelController>().Deactivate();
        goLevels[iLevel].GetComponent<LevelController>().Activate();
    }

    // ------------------------------------------------------------------------------------------------

    private void Activate()
    {
        guiLevel.text = (iLevel + 1).ToString();
        guiNumProjectile.text = "0";
        goLevels[iLevel].GetComponent<LevelController>().LevelStart();
        goWallsDestructible = GameObject.FindGameObjectsWithTag("WallDestructible");
        goWallsMoveable = GameObject.FindGameObjectsWithTag("WallMoveable");
    }

    // ------------------------------------------------------------------------------------------------

    public void SwitchWallsMoveable()
    {
        foreach (GameObject goWallMoveable in goWallsMoveable)
        {
            goWallMoveable.GetComponent<WallController>().Switch();
        }
    }

    // ------------------------------------------------------------------------------------------------

    public bool GetbProjectilePathDependentLevel()
    {
        return(goLevels[iLevel].GetComponent<LevelController>().bProjectilePathDependentLevel);
    }

    // ------------------------------------------------------------------------------------------------

}
