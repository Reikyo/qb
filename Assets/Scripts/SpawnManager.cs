using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject goPrefabPlayer;
    public GameObject goPrefabTarget;
    public GameObject goPrefabEnemy;
    // public GameObject goPrefabSafeZonePlayer;
    // public GameObject goPrefabSafeZoneTarget;
    public GameObject goPrefabPowerUp;

    private GameObject goPlayer;
    private GameObject goTarget;
    private GameObject goEnemy;
    // private GameObject goSafeZonePlayer;
    // private GameObject goSafeZoneTarget;
    private GameObject goPowerUp;

    // private List<int> iListLevel3SpawnPositionX = new List<int>() {-18, -14, -10, -6, -2, 2, 6, 10, 14, 18};
    // private List<int> iListLevel3SpawnPositionX = Enumerable.Range(-20, 40).ToList();
    private List<int> iListLevel3SpawnPositionX = new List<int>();
    private List<int> iListLevel3SpawnPositionXCopy;
    private List<int> iListLevel3SpawnPositionXSelection;
    private float fLevel3SpawnPositionZLower = 15.5f;
    private float fLevel3SpawnPositionZUpper = 22.5f;
    private List<string> sListLevel3WallTimed = new List<string>() {
        // "Wall Horizontal (2) (1)",
        // "Wall Horizontal (5) (1)",
        // "Wall Horizontal (6) (1)",
        // "Wall Horizontal (9) (1)",
        // "Wall Vertical (2) (1)",
        // "Wall Vertical (2) (11)",
        "Wall Vertical (3) (1)",
        "Wall Vertical (5) (1)",
        "Wall Vertical (6) (4)", // Gateway wall
        "Wall Vertical (6) (8)", // Gateway wall
        "Wall Vertical (3) (11)",
        "Wall Vertical (5) (11)"
    };
    private int iNumLevel3Enemy;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        for (int i=-20; i<21; i+=2)
        {
            iListLevel3SpawnPositionX.Add(i);
        }
        iNumLevel3Enemy = sListLevel3WallTimed.Count;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

    }

    // ------------------------------------------------------------------------------------------------

    // public void Instantiate(GameObject[] goSpawns, Vector3[] goSpawnPositions)
    // {
    //     // Random instantiate locations for all characters and items:
    //     // foreach (GameObject goPrefab in new List<GameObject>() {goPrefabPlayer, goPrefabEnemy, goPrefabTarget, goPrefabPowerUp, goPrefabSafeZone})
    //     // {
    //     //     Instantiate(goPrefab, new Vector3(Random.Range(-fXLimitSpawn, fXLimitSpawn), goPrefab.transform.position.y, Random.Range(-fZLimitSpawn, fZLimitSpawn)), goPrefab.transform.rotation);
    //     // }
    //
    //     Instantiate(goPrefabPlayer, new Vector3(-20f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
    //     Instantiate(goPrefabTarget, new Vector3(0f, goPrefabTarget.transform.position.y, -20f), goPrefabTarget.transform.rotation);
    //     Instantiate(goPrefabEnemy, new Vector3(20f, goPrefabEnemy.transform.position.y, -20f), goPrefabEnemy.transform.rotation);
    //     Instantiate(goPrefabSafeZone, new Vector3(20f, goPrefabSafeZone.transform.position.y, 20f), goPrefabSafeZone.transform.rotation);
    //     Instantiate(goPrefabPowerUp, new Vector3(-20f, goPrefabPowerUp.transform.position.y, 20f), goPrefabPowerUp.transform.rotation);
    //
    //     // for (int goSpawnsIdx=0; goSpawnsIdx<goSpawns.Length; goSpawnsIdx++)
    //     // {
    //     //     Instantiate(
    //     //         goSpawns[goSpawnsIdx],
    //     //         new Vector3(goSpawnPositions[goSpawnsIdx].x, goSpawns[goSpawnsIdx].transform.position.y, goSpawnPositions[goSpawnsIdx].z),
    //     //         goSpawns[goSpawnsIdx].transform.rotation
    //     //     );
    //     // }
    //
    // }

    // ------------------------------------------------------------------------------------------------

    public void Instantiate(int iLevel)
    {
        switch (iLevel)
        {
            case 0:
                // Level 0 0
                // Instantiate(goPrefabPlayer, new Vector3(0f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
                // Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, 20f), goPrefabSafeZonePlayer.transform.rotation);
                // Level 0
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(0f, goPrefabPlayer.transform.position.y, -19f), goPrefabPlayer.transform.rotation);
                break;
            case 1:
                // Level 1 0
                // Instantiate(goPrefabPlayer, new Vector3(-20f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
                // Instantiate(goPrefabEnemy, new Vector3(-5f, goPrefabEnemy.transform.position.y, 0f), goPrefabEnemy.transform.rotation);
                // Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, 0f), goPrefabSafeZonePlayer.transform.rotation);
                // Instantiate(goPrefabPowerUp, new Vector3(20f, goPrefabPowerUp.transform.position.y, 20f), goPrefabPowerUp.transform.rotation);
                // Level 1
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(-22f, goPrefabPlayer.transform.position.y, -22f), goPrefabPlayer.transform.rotation);
                goEnemy = Instantiate(goPrefabEnemy, new Vector3(7.5f, goPrefabEnemy.transform.position.y, 17f), goPrefabEnemy.transform.rotation);
                goEnemy.GetComponent<EnemyController>().goWallTimed = GameObject.Find("Wall Horizontal (6) (3)");
                goEnemy = Instantiate(goPrefabEnemy, new Vector3(-17f, goPrefabEnemy.transform.position.y, 7.5f), goPrefabEnemy.transform.rotation);
                goEnemy.GetComponent<EnemyController>().goWallTimed = GameObject.Find("Wall Horizontal (6) (4)");
                goEnemy = Instantiate(goPrefabEnemy, new Vector3(-7.5f, goPrefabEnemy.transform.position.y, -17f), goPrefabEnemy.transform.rotation);
                goEnemy.GetComponent<EnemyController>().goWallTimed = GameObject.Find("Wall Horizontal (6) (5)");
                goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(12f, goPrefabPowerUp.transform.position.y, -12.5f), goPrefabPowerUp.transform.rotation);
                break;
            case 2:
                // Level 2 0
                // goPlayer = Instantiate(goPrefabPlayer, new Vector3(-22f, goPrefabPlayer.transform.position.y, -23f), goPrefabPlayer.transform.rotation);
                // goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(-17f, goPrefabSafeZonePlayer.transform.position.y, -2f), goPrefabSafeZonePlayer.transform.rotation);
                // goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(0f, goPrefabPowerUp.transform.position.y, -23f), goPrefabPowerUp.transform.rotation);
                // Level 2
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(-7.35f, goPrefabPlayer.transform.position.y, -22.05f), goPrefabPlayer.transform.rotation);
                goTarget = Instantiate(goPrefabTarget, new Vector3(7.35f, goPrefabTarget.transform.position.y, -22.05f), goPrefabTarget.transform.rotation);
                break;
            case 3:
                // Level 3 0
                // goPlayer = Instantiate(goPrefabPlayer, new Vector3(-21f, goPrefabPlayer.transform.position.y, -21f), goPrefabPlayer.transform.rotation);
                // goTarget = Instantiate(goPrefabTarget, new Vector3(0f, goPrefabTarget.transform.position.y, -21f), goPrefabTarget.transform.rotation);
                // goEnemy = Instantiate(goPrefabEnemy, new Vector3(21f, goPrefabEnemy.transform.position.y, -21f), goPrefabEnemy.transform.rotation);
                // goSafeZoneTarget = Instantiate(goPrefabSafeZoneTarget, new Vector3(21f, goPrefabSafeZoneTarget.transform.position.y, 0f), goPrefabSafeZoneTarget.transform.rotation);
                // goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(-21f, goPrefabPowerUp.transform.position.y, 20f), goPrefabPowerUp.transform.rotation);
                // Level 3
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(-21f, goPrefabPlayer.transform.position.y, -14f), goPrefabPlayer.transform.rotation);
                goTarget = Instantiate(goPrefabTarget, new Vector3(21f, goPrefabTarget.transform.position.y, -14f), goPrefabTarget.transform.rotation);
                SetiListLevel3SpawnPositionXSelection();
                for (int i=0; i<iNumLevel3Enemy; i++)
                {
                    goEnemy = Instantiate(
                        goPrefabEnemy,
                        new Vector3(
                            iListLevel3SpawnPositionXSelection[i],
                            goPrefabEnemy.transform.position.y,
                            Random.Range(
                                fLevel3SpawnPositionZLower,
                                fLevel3SpawnPositionZUpper
                            )
                        ),
                        Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                    goEnemy.GetComponent<EnemyController>().goWallTimed = GameObject.Find(sListLevel3WallTimed[i]);
                }
                goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(0f, goPrefabPowerUp.transform.position.y, 0f), goPrefabPowerUp.transform.rotation);
                goPowerUp.GetComponent<PowerUpController>().iNumProjectile = 50;
                break;
            case 4:
                // Level 4 0
                // goPlayer = Instantiate(goPrefabPlayer, new Vector3(-22.5f, goPrefabPlayer.transform.position.y, 22.5f), goPrefabPlayer.transform.rotation);
                // goTarget = Instantiate(goPrefabTarget, new Vector3(22.5f, goPrefabTarget.transform.position.y, -22.5f), goPrefabTarget.transform.rotation);
                // goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, -8f), goPrefabSafeZonePlayer.transform.rotation);
                // goSafeZoneTarget = Instantiate(goPrefabSafeZoneTarget, new Vector3(-7.5f, goPrefabSafeZoneTarget.transform.position.y, 21.5f), goPrefabSafeZoneTarget.transform.rotation);
                // goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(-22.5f, goPrefabPowerUp.transform.position.y, 0f), goPrefabPowerUp.transform.rotation);
                // Level 4
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(15f, goPrefabPlayer.transform.position.y, -10f), goPrefabPlayer.transform.rotation);
                goTarget = Instantiate(goPrefabTarget, new Vector3(-15f, goPrefabTarget.transform.position.y, 10f), goPrefabTarget.transform.rotation);
                break;
            case 5:
                // Level 5 0
                // goPlayer = Instantiate(goPrefabPlayer, new Vector3(0f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
                // goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, 20f), goPrefabSafeZonePlayer.transform.rotation);
                // Level 5
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(0f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
                break;
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Destroy()
    {
        foreach (string sTag in new List<string>() {
            "Player",
            "Target",
            "Enemy",
            // "SafeZonePlayer",
            // "SafeZoneTarget",
            "PowerUp"
        })
        {
            GameObject[] goTag = GameObject.FindGameObjectsWithTag(sTag);
            foreach (GameObject go in goTag)
            {
                go.SetActive(false); // We must deactivate all game objects or they will not be found by the FindWithTag method on reload
                Destroy(go);
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    void SetiListLevel3SpawnPositionXSelection()
    {
        // for (int i=0; i<100; i++)
        // {
        //     int iIndex1 = Random.Range(0, 9);
        //     int iIndex2 = iIndex1;
        //     while (iIndex2 == iIndex1)
        //     {
        //         iIndex2 = Random.Range(0, 9);
        //     }
        //     int tmp = iListLevel3SpawnPositionX[iIndex1];
        //     iListLevel3SpawnPositionX[iIndex1] = iListLevel3SpawnPositionX[iIndex2];
        //     iListLevel3SpawnPositionX[iIndex2] = tmp;
        // }
        iListLevel3SpawnPositionXCopy = iListLevel3SpawnPositionX.ToList();
        iListLevel3SpawnPositionXSelection = new List<int>();
        for (int i=0; i<iNumLevel3Enemy; i++)
        {
            int iIdx = Random.Range(0, iListLevel3SpawnPositionXCopy.Count - 1);
            iListLevel3SpawnPositionXSelection.Add(iListLevel3SpawnPositionXCopy[iIdx]);
            iListLevel3SpawnPositionXCopy.RemoveAt(iIdx);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
