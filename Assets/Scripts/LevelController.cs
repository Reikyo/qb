using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyFunctions;

public class LevelController : MonoBehaviour
{
    private int iLevel;
    private SpawnManager spawnManager;
    private CubeController cubeController;
    private NavMeshSurface navNavMesh;

    public bool bProjectilePathDependentLevel = false;
    // public GameObject[] goArrSpawns;
    // public Vector3[] v3ArrSpawnPositions;
    private GameObject[] goArrWallDestructible;
    private GameObject[] goArrWallTimed;
    private GameObject[] goArrTranslator;
    private GameObject[] goArrRotator;
    private GameObject[] goArrSwitcher;
    private GameObject[] goArrExchanger;
    private GameObject goSafeZonePlayer;
    private GameObject goSafeZoneTarget;
    private Vector3 v3PositionSafeZonePlayerOrig;
    private Vector3 v3PositionSafeZoneTargetOrig;
    private Color colSafeZonePlayer;
    private Color colSafeZoneTarget;

    private Vector3 v3PositionInstantiate = new Vector3(0f, -10f, 1f);
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

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        cubeController = GameObject.Find("Cube").GetComponent<CubeController>();
        navNavMesh = GameObject.Find("Nav Mesh").GetComponent<NavMeshSurface>();

        goArrWallDestructible = GameObject.FindGameObjectsWithTag("WallDestructible");
        goArrWallTimed = GameObject.FindGameObjectsWithTag("WallTimed");
        goArrTranslator = GameObject.FindGameObjectsWithTag("Translator");
        goArrRotator = GameObject.FindGameObjectsWithTag("Rotator");
        goArrSwitcher = GameObject.FindGameObjectsWithTag("Switcher");
        goArrExchanger = GameObject.FindGameObjectsWithTag("Exchanger");
        goSafeZonePlayer = GameObject.FindWithTag("SafeZonePlayer");
        goSafeZoneTarget = GameObject.FindWithTag("SafeZoneTarget");

        if (    goSafeZonePlayer
            &&  goSafeZoneTarget )
        {
            v3PositionSafeZonePlayerOrig = goSafeZonePlayer.transform.position;
            v3PositionSafeZoneTargetOrig = goSafeZoneTarget.transform.position;
            colSafeZonePlayer = goSafeZonePlayer.GetComponent<SpriteRenderer>().color;
            colSafeZoneTarget = goSafeZoneTarget.GetComponent<SpriteRenderer>().color;
        }

        transform.position = v3PositionInstantiate;

        fMetresPerSecY = Math.Abs((v3PositionPlay.y - v3PositionInstantiate.y) / fTransitionTime);
        fMetresPerSecZ = Math.Abs((v3PositionPlay.z - v3PositionInstantiate.z) / fTransitionTime);
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
                    v3PositionPlay.y - gameObject.transform.position.y
                );
            }
            if (bChangeStatePositionZ)
            {
                fMetresPerFrameZ = fMetresPerSecZ * Time.deltaTime;
                bChangeStatePositionZ = MyFunctions.Move.Translate(
                    gameObject,
                    "z",
                    fMetresPerFrameZ,
                    v3PositionPlay.z - gameObject.transform.position.z
                );
            }
            if (    !bChangeStatePositionY
                &&  !bChangeStatePositionZ )
            {
                bChangeStateStartLevel = false;
                // Reset(true, false);
                // Activate();
                navNavMesh.BuildNavMesh();
                spawnManager.Instantiate(iLevel);
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
                    fMetresPerFrameY,
                    v3PositionInstantiate.y - gameObject.transform.position.y
                );
            }
            if (bChangeStatePositionZ)
            {
                fMetresPerFrameZ = fMetresPerSecZ * Time.deltaTime;
                bChangeStatePositionZ = MyFunctions.Move.Translate(
                    gameObject,
                    "z",
                    fMetresPerFrameZ,
                    v3PositionInstantiate.z - gameObject.transform.position.z
                );
            }
            if (    !bChangeStatePositionY
                &&  !bChangeStatePositionZ )
            {
                bChangeStateFinishLevel = false;
                Reset(true, false); // Reset the level environment in case the player plays through all levels again after finishing the game
                gameObject.SetActive(false);
                cubeController.StartNextLevelContinue();
            }
        }

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

    public void StartLevel(int iLevelGiven)
    {
        iLevel = iLevelGiven;
        gameObject.SetActive(true);
        bChangeStateStartLevel = true;
        bChangeStatePositionY = true;
        bChangeStatePositionZ = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void FinishLevel()
    {
        spawnManager.Destroy();
        // Deactivate();
        bChangeStateFinishLevel = true;
        bChangeStatePositionY = true;
        bChangeStatePositionZ = true;
    }

    // ------------------------------------------------------------------------------------------------

    // private void Activate()
    // {
    //     // spawnManager.Instantiate(goArrSpawns, v3ArrSpawnPositions);
    //
    //     for (int goArrSpawnsIdx=0; goArrSpawnsIdx<goArrSpawns.Length; goArrSpawnsIdx++)
    //     {
    //         Instantiate(
    //             goArrSpawns[goArrSpawnsIdx],
    //             new Vector3(
    //                 v3ArrSpawnPositions[goArrSpawnsIdx].x,
    //                 goArrSpawns[goArrSpawnsIdx].transform.position.y,
    //                 v3ArrSpawnPositions[goArrSpawnsIdx].z
    //             ),
    //             goArrSpawns[goArrSpawnsIdx].transform.rotation
    //         );
    //     }
    // }

    // ------------------------------------------------------------------------------------------------

    // private void Deactivate()
    // {
    //     foreach (string sTag in new List<string>() {
    //         "Player",
    //         "Target",
    //         "Enemy",
    //         "SafeZonePlayer",
    //         "SafeZoneTarget",
    //         "PowerUp"
    //     })
    //     {
    //         GameObject go = GameObject.FindWithTag(sTag);
    //         if (go)
    //         {
    //             // We must deactivate all game objects or they will not be found by the FindWithTag method on reload
    //             go.SetActive(false);
    //             Destroy(go);
    //         }
    //     }
    // }

    // ------------------------------------------------------------------------------------------------

    public void Reset(
        bool bResetEnvironment=true,
        bool bResetCharacters=true
    )
    {
        if (bResetEnvironment)
        {
            foreach (GameObject goWallDestructible in goArrWallDestructible)
            {
                goWallDestructible.SetActive(true);
            }
            foreach (GameObject goWallTimed in goArrWallTimed)
            {
                goWallTimed.SetActive(true);
            }
            foreach (GameObject goTranslator in goArrTranslator)
            {
                goTranslator.GetComponent<TranslatorController>().Reset();
            }
            foreach (GameObject goRotator in goArrRotator)
            {
                goRotator.GetComponent<RotatorController>().Reset();
            }
            foreach (GameObject goSwitcher in goArrSwitcher)
            {
                goSwitcher.GetComponent<SwitcherController>().Reset();
            }
            foreach (GameObject goExchanger in goArrExchanger)
            {
                goExchanger.GetComponent<PlayerBuddySwitchController>().bEngagedByTarget = false;
            }
            if (    goSafeZonePlayer
                &&  goSafeZoneTarget )
            {
                goSafeZonePlayer.transform.position = goSafeZonePlayer.transform.parent.transform.position + v3PositionSafeZonePlayerOrig;
                goSafeZoneTarget.transform.position = goSafeZoneTarget.transform.parent.transform.position + v3PositionSafeZoneTargetOrig;
                goSafeZonePlayer.GetComponent<SpriteRenderer>().color = colSafeZonePlayer;
                goSafeZoneTarget.GetComponent<SpriteRenderer>().color = colSafeZoneTarget;
            }
        }
        if (bResetCharacters)
        {
            // Deactivate();
            // Activate();
            spawnManager.Destroy();
            spawnManager.Instantiate(iLevel);
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void TriggerAllTranslator()
    {
        foreach (GameObject goTranslator in goArrTranslator)
        {
            goTranslator.GetComponent<TranslatorController>().Trigger("", "", false);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
