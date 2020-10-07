using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;

public class LevelController : MonoBehaviour
{
    private Vector3 v3InstantiatePosition = new Vector3(0f, -5f, 1f);
    private Vector3 v3LevelPosition = new Vector3(0f, 0f, 0f);

    private float fLevelStartTime = 0.5f;

    private float fLevelStartMetresPerSecY;
    private float fLevelStartMetresPerFrameY;
    private float fLevelStartMetresPerSecZ;
    private float fLevelStartMetresPerFrameZ;

    public bool bProjectilePathDependentLevel = false;
    public bool bLevelStart = false;
    public bool bLevelFinish = false;

    private bool bLevelPositionY = false;
    private bool bLevelPositionZ = false;

    private CubeController cubeController;
    private SpawnManager spawnManager;
    public GameObject[] golistSpawns;
    public Vector3[] v3listSpawnPositions;

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
        transform.position = v3InstantiatePosition;

        fLevelStartMetresPerSecY = (v3LevelPosition.y - v3InstantiatePosition.y) / fLevelStartTime;
        fLevelStartMetresPerFrameY = fLevelStartMetresPerSecY * Time.deltaTime;
        fLevelStartMetresPerSecZ = (v3LevelPosition.z - v3InstantiatePosition.z) / fLevelStartTime;
        fLevelStartMetresPerFrameZ = fLevelStartMetresPerSecZ * Time.deltaTime;

        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        cubeController = GameObject.Find("Cube").GetComponent<CubeController>();
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

        // ------------------------------------------------------------------------------------------------

        if (bLevelStart)
        {
            if (!bLevelPositionY)
            {
                bLevelPositionY = MyFunctions.Move.Translate(gameObject, "y", fLevelStartMetresPerFrameY, transform.position.y, v3LevelPosition.y, bLevelPositionY);
            }
            if (!bLevelPositionZ)
            {
                bLevelPositionZ = MyFunctions.Move.Translate(gameObject, "z", fLevelStartMetresPerFrameZ, transform.position.z, v3LevelPosition.z, bLevelPositionZ);
            }
            if (bLevelPositionY
            &&  bLevelPositionZ)
            {
                bLevelPositionY = false;
                bLevelPositionZ = false;
                bLevelStart = false;
                Activate();
            }
        }

        // ------------------------------------------------------------------------------------------------

        if (bLevelFinish)
        {
            if (!bLevelPositionY)
            {
                bLevelPositionY = MyFunctions.Move.Translate(gameObject, "y", -fLevelStartMetresPerFrameY, transform.position.y, v3InstantiatePosition.y, bLevelPositionY);
            }
            if (!bLevelPositionZ)
            {
                bLevelPositionZ = MyFunctions.Move.Translate(gameObject, "z", -fLevelStartMetresPerFrameZ, transform.position.z, v3InstantiatePosition.z, bLevelPositionZ);
            }
            if (bLevelPositionY
            &&  bLevelPositionZ)
            {
                bLevelPositionY = false;
                bLevelPositionZ = false;
                bLevelFinish = false;
                gameObject.SetActive(false);
                cubeController.bNextLevelStart = true;
            }
        }

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

    // private bool Translate(
    //     string sAxis,
    //     float fMetresPerFrame,
    //     float fCurrentPosition,
    //     float fTargetPosition,
    //     bool bTargetPosition
    // )
    // {
    //     if (((fMetresPerFrame > 0f) && (fTargetPosition > (fCurrentPosition + fMetresPerFrame)))
    //     ||  ((fMetresPerFrame < 0f) && (fTargetPosition < (fCurrentPosition + fMetresPerFrame))))
    //     {
    //         switch(sAxis)
    //         {
    //             case "x":
    //                 transform.Translate(fMetresPerFrame, 0f, 0f, Space.World);
    //                 break;
    //             case "y":
    //                 transform.Translate(0f, fMetresPerFrame, 0f, Space.World);
    //                 break;
    //             case "z":
    //                 transform.Translate(0f, 0f, fMetresPerFrame, Space.World);
    //                 break;
    //         }
    //     }
    //     else
    //     {
    //         switch(sAxis)
    //         {
    //             case "x":
    //                 transform.Translate(fTargetPosition - fCurrentPosition, 0f, 0f, Space.World);
    //                 break;
    //             case "y":
    //                 transform.Translate(0f, fTargetPosition - fCurrentPosition, 0f, Space.World);
    //                 break;
    //             case "z":
    //                 transform.Translate(0f, 0f, fTargetPosition - fCurrentPosition, Space.World);
    //                 break;
    //         }
    //         bTargetPosition = true;
    //     }
    //     return(bTargetPosition);
    // }

    // ------------------------------------------------------------------------------------------------

    public void LevelStart()
    {
        gameObject.SetActive(true);
        bLevelStart = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelFinish()
    {
        // spawnManager.Destroy();
        Deactivate();
        bLevelFinish = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void Activate()
    {
        // spawnManager.Instantiate(golistSpawns, v3listSpawnPositions);

        for (int golistSpawnsIdx=0; golistSpawnsIdx<golistSpawns.Length; golistSpawnsIdx++)
        {
            Instantiate(
                golistSpawns[golistSpawnsIdx],
                new Vector3(
                    v3listSpawnPositions[golistSpawnsIdx].x,
                    golistSpawns[golistSpawnsIdx].transform.position.y,
                    v3listSpawnPositions[golistSpawnsIdx].z
                ),
                golistSpawns[golistSpawnsIdx].transform.rotation
            );
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Deactivate()
    {
        foreach (string sTag in new List<string>() {"Player", "Enemy", "Target", "PowerUp", "SafeZonePlayer", "SafeZoneTarget"})
        {
            GameObject go = GameObject.FindWithTag(sTag);
            if (go)
            {
                go.SetActive(false); // We must deactivate all game objects or they will not be found by the FindWithTag method on reload
                Destroy(go);
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
