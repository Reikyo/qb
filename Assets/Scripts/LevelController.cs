﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;

public class LevelController : MonoBehaviour
{
    // private SpawnManager spawnManager;
    private CubeController cubeController;

    public bool bProjectilePathDependentLevel = false;
    public GameObject[] goArrSpawns;
    public Vector3[] v3ArrSpawnPositions;
    private GameObject[] goArrWallsDestructible;
    private GameObject[] goArrWallsSlider;

    private Vector3 v3PositionInstantiate = new Vector3(0f, -5f, 1f);
    private Vector3 v3PositionPlay = new Vector3(0f, 0f, 0f);

    private float fMetresPerSecY;
    private float fMetresPerFrameY;
    private float fMetresPerSecZ;
    private float fMetresPerFrameZ;

    private float fTransitionTime = 0.5f;

    private bool bChangeStateStartLevel = false;
    private bool bChangeStateFinishLevel = false;

    private bool bChangeStatePositionY = false;
    private bool bChangeStatePositionZ = false;

// Level 1
//   Player         (0, 0, -20)
//   SafeZonePlayer (0, 0, 20)
// Level 2
//   Player         (-20, 0, -20)
//   Enemy          (-5, 0, 0)
//   SafeZonePlayer (0, 0, 0)
//   PowerUp        (20, 0, 20)
// Level 3
//   Player         (-22, 0, -23)
//   SafeZonePlayer (-17, 0, -2)
//   PowerUp        (0, 0, -23)
// Level 4
//   Player         (-21, 0, -21)
//   Target         (0, 0, -21)
//   Enemy          (21, 0, -21)
//   SafeZoneTarget (21, 0, 0)
//   PowerUp        (-21, 0, 20)
// Level 5
//   Player         (-22.5, 0, 22.5)
//   Target         (22.5, 0, -22.5)
//   SafeZonePlayer (0, 0, -8)
//   SafeZoneTarget (-7.5, 0, 21.5)
//   PowerUp        (-2.5, 0, 0)
// Level 6
//   Player         (0, 0, -20)
//   SafeZonePlayer (0, 0, 20)

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        cubeController = GameObject.Find("Cube").GetComponent<CubeController>();

        transform.position = v3PositionInstantiate;

        fMetresPerSecY = (v3PositionPlay.y - v3PositionInstantiate.y) / fTransitionTime;
        fMetresPerSecZ = (v3PositionPlay.z - v3PositionInstantiate.z) / fTransitionTime;

    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

        // ------------------------------------------------------------------------------------------------

        if (bChangeStateStartLevel)
        {
            if (bChangeStatePositionY)
            {
                fMetresPerFrameY = fMetresPerSecY * Time.deltaTime;
                bChangeStatePositionY = MyFunctions.Move.Translate(
                    gameObject,
                    "y",
                    fMetresPerFrameY,
                    transform.position.y,
                    v3PositionPlay.y
                );
            }
            if (bChangeStatePositionZ)
            {
                fMetresPerFrameZ = fMetresPerSecZ * Time.deltaTime;
                bChangeStatePositionZ = MyFunctions.Move.Translate(
                    gameObject,
                    "z",
                    fMetresPerFrameZ,
                    transform.position.z,
                    v3PositionPlay.z
                );
            }
            if (!bChangeStatePositionY
            &&  !bChangeStatePositionZ)
            {
                bChangeStateStartLevel = false;
                Activate();
            }
        }

        // ------------------------------------------------------------------------------------------------

        if (bChangeStateFinishLevel)
        {
            if (bChangeStatePositionY)
            {
                fMetresPerFrameY = fMetresPerSecY * Time.deltaTime;
                bChangeStatePositionY = MyFunctions.Move.Translate(
                    gameObject,
                    "y",
                    -fMetresPerFrameY,
                    transform.position.y,
                    v3PositionInstantiate.y
                );
            }
            if (bChangeStatePositionZ)
            {
                fMetresPerFrameZ = fMetresPerSecZ * Time.deltaTime;
                bChangeStatePositionZ = MyFunctions.Move.Translate(
                    gameObject,
                    "z",
                    -fMetresPerFrameZ,
                    transform.position.z,
                    v3PositionInstantiate.z
                );
            }
            if (!bChangeStatePositionY
            &&  !bChangeStatePositionZ)
            {
                bChangeStateFinishLevel = false;
                gameObject.SetActive(false);
                cubeController.StartNextLevelContinue();
            }
        }

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

    public void StartLevel()
    {
        gameObject.SetActive(true);
        goArrWallsDestructible = GameObject.FindGameObjectsWithTag("WallDestructible");
        goArrWallsSlider = GameObject.FindGameObjectsWithTag("WallSlider");
        bChangeStateStartLevel = true;
        bChangeStatePositionY = true;
        bChangeStatePositionZ = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void FinishLevel()
    {
        // spawnManager.Destroy();
        Deactivate();
        bChangeStateFinishLevel = true;
        bChangeStatePositionY = true;
        bChangeStatePositionZ = true;
    }

    // ------------------------------------------------------------------------------------------------

    private void Activate()
    {
        // spawnManager.Instantiate(goArrSpawns, v3ArrSpawnPositions);

        for (int goArrSpawnsIdx=0; goArrSpawnsIdx<goArrSpawns.Length; goArrSpawnsIdx++)
        {
            Instantiate(
                goArrSpawns[goArrSpawnsIdx],
                new Vector3(
                    v3ArrSpawnPositions[goArrSpawnsIdx].x,
                    goArrSpawns[goArrSpawnsIdx].transform.position.y,
                    v3ArrSpawnPositions[goArrSpawnsIdx].z
                ),
                goArrSpawns[goArrSpawnsIdx].transform.rotation
            );
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void Deactivate()
    {
        foreach (string sTag in new List<string>() {
            "Player",
            "Target",
            "Enemy",
            "SafeZonePlayer",
            "SafeZoneTarget",
            "PowerUp"
        })
        {
            GameObject go = GameObject.FindWithTag(sTag);
            if (go)
            {
                // We must deactivate all game objects or they will not be found by the FindWithTag method on reload
                go.SetActive(false);
                Destroy(go);
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Reset()
    {
        foreach (GameObject goWallDestructible in goArrWallsDestructible)
        {
            goWallDestructible.SetActive(true);
        }
        foreach (GameObject goWallSlider in goArrWallsSlider)
        {
            goWallSlider.GetComponent<WallSliderController>().Reset();
        }
        Deactivate();
        Activate();
    }

    // ------------------------------------------------------------------------------------------------

    public void TriggerWallsSlider()
    {
        foreach (GameObject goWallSlider in goArrWallsSlider)
        {
            goWallSlider.GetComponent<WallSliderController>().Trigger();
        }
    }

    // ------------------------------------------------------------------------------------------------

}
