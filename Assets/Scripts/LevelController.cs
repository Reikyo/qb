using System.Collections;
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

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        cubeController = GameObject.Find("Cube").GetComponent<CubeController>();
        navNavMesh = GameObject.Find("Nav Mesh").GetComponent<NavMeshSurface>();

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

    public void StartLevel(int iLevelGiven)
    {
        iLevel = iLevelGiven;
        gameObject.SetActive(true);
        goArrWallDestructible = GameObject.FindGameObjectsWithTag("WallDestructible");
        goArrWallTimed = GameObject.FindGameObjectsWithTag("WallTimed");
        goArrTranslator = GameObject.FindGameObjectsWithTag("Translator");
        goArrRotator = GameObject.FindGameObjectsWithTag("Rotator");
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

    public void Reset()
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
        // Deactivate();
        // Activate();
        spawnManager.Destroy();
        spawnManager.Instantiate(iLevel);
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
