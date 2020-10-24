using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject goPrefabPlayer;
    public GameObject goPrefabTarget;
    public GameObject goPrefabEnemy;
    public GameObject goPrefabSafeZonePlayer;
    public GameObject goPrefabSafeZoneTarget;
    public GameObject goPrefabPowerUp;

    private GameObject goPlayer;
    private GameObject goTarget;
    private GameObject goEnemy;
    private GameObject goEnemy1;
    private GameObject goEnemy2;
    private GameObject goEnemy3;
    private GameObject goSafeZonePlayer;
    private GameObject goSafeZoneTarget;
    private GameObject goPowerUp;

    // private float fXLimitSpawn = 20f;
    // private float fZLimitSpawn = 20f;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {

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
                // Level 1 0
                // Instantiate(goPrefabPlayer, new Vector3(0f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
                // Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, 20f), goPrefabSafeZonePlayer.transform.rotation);
                // Level 1
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(0f, goPrefabPlayer.transform.position.y, -19f), goPrefabPlayer.transform.rotation);
                goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, 0f), goPrefabSafeZonePlayer.transform.rotation);
                break;
            case 1:
                // Level 2 0
                // Instantiate(goPrefabPlayer, new Vector3(-20f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
                // Instantiate(goPrefabEnemy, new Vector3(-5f, goPrefabEnemy.transform.position.y, 0f), goPrefabEnemy.transform.rotation);
                // Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, 0f), goPrefabSafeZonePlayer.transform.rotation);
                // Instantiate(goPrefabPowerUp, new Vector3(20f, goPrefabPowerUp.transform.position.y, 20f), goPrefabPowerUp.transform.rotation);
                // Level 2
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(-22f, goPrefabPlayer.transform.position.y, -22f), goPrefabPlayer.transform.rotation);
                goEnemy1 = Instantiate(goPrefabEnemy, new Vector3(7.5f, goPrefabEnemy.transform.position.y, 17f), goPrefabEnemy.transform.rotation);
                goEnemy2 = Instantiate(goPrefabEnemy, new Vector3(-17f, goPrefabEnemy.transform.position.y, 7.5f), goPrefabEnemy.transform.rotation);
                // goEnemy1 = Instantiate(goPrefabEnemy, new Vector3(-7.5f, goPrefabEnemy.transform.position.y, -13f), goPrefabEnemy.transform.rotation);
                // goEnemy2 = Instantiate(goPrefabEnemy, new Vector3(-7.5f, goPrefabEnemy.transform.position.y, -15f), goPrefabEnemy.transform.rotation);
                goEnemy3 = Instantiate(goPrefabEnemy, new Vector3(-7.5f, goPrefabEnemy.transform.position.y, -17f), goPrefabEnemy.transform.rotation);
                goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, 0f), goPrefabSafeZonePlayer.transform.rotation);
                goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(12f, goPrefabPowerUp.transform.position.y, -12.5f), goPrefabPowerUp.transform.rotation);
                // goEnemy1.name = "Enemy (1)"; // Associated with "Wall Horizontal (6) (3)"
                // goEnemy2.name = "Enemy (2)"; // Associated with "Wall Horizontal (6) (4)"
                // goEnemy3.name = "Enemy (3)"; // Associated with "Wall Horizontal (6) (5)"
                goEnemy1.GetComponent<EnemyController>().goWallTimed = GameObject.Find("Wall Horizontal (6) (3)");
                goEnemy2.GetComponent<EnemyController>().goWallTimed = GameObject.Find("Wall Horizontal (6) (4)");
                goEnemy3.GetComponent<EnemyController>().goWallTimed = GameObject.Find("Wall Horizontal (6) (5)");
                break;
            case 2:
                // Level 3 0
                // goPlayer = Instantiate(goPrefabPlayer, new Vector3(-22f, goPrefabPlayer.transform.position.y, -23f), goPrefabPlayer.transform.rotation);
                // goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(-17f, goPrefabSafeZonePlayer.transform.position.y, -2f), goPrefabSafeZonePlayer.transform.rotation);
                // goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(0f, goPrefabPowerUp.transform.position.y, -23f), goPrefabPowerUp.transform.rotation);
                // Level 3
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(-7.35f, goPrefabPlayer.transform.position.y, -22.05f), goPrefabPlayer.transform.rotation);
                goTarget = Instantiate(goPrefabTarget, new Vector3(7.35f, goPrefabTarget.transform.position.y, -22.05f), goPrefabTarget.transform.rotation);
                // goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(-12.25f, goPrefabSafeZonePlayer.transform.position.y, 1.75f), goPrefabSafeZonePlayer.transform.rotation);
                // goSafeZoneTarget = Instantiate(goPrefabSafeZoneTarget, new Vector3(12.25f, goPrefabSafeZoneTarget.transform.position.y, 1.75f), goPrefabSafeZoneTarget.transform.rotation);
                break;
            case 3:
                // Level 4
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(-21f, goPrefabPlayer.transform.position.y, -21f), goPrefabPlayer.transform.rotation);
                goTarget = Instantiate(goPrefabTarget, new Vector3(0f, goPrefabTarget.transform.position.y, -21f), goPrefabTarget.transform.rotation);
                goEnemy = Instantiate(goPrefabEnemy, new Vector3(21f, goPrefabEnemy.transform.position.y, -21f), goPrefabEnemy.transform.rotation);
                goSafeZoneTarget = Instantiate(goPrefabSafeZoneTarget, new Vector3(21f, goPrefabSafeZoneTarget.transform.position.y, 0f), goPrefabSafeZoneTarget.transform.rotation);
                goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(-21f, goPrefabPowerUp.transform.position.y, 20f), goPrefabPowerUp.transform.rotation);
                break;
            case 4:
                // Level 5
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(-22.5f, goPrefabPlayer.transform.position.y, 22.5f), goPrefabPlayer.transform.rotation);
                goTarget = Instantiate(goPrefabTarget, new Vector3(22.5f, goPrefabTarget.transform.position.y, -22.5f), goPrefabTarget.transform.rotation);
                goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, -8f), goPrefabSafeZonePlayer.transform.rotation);
                goSafeZoneTarget = Instantiate(goPrefabSafeZoneTarget, new Vector3(-7.5f, goPrefabSafeZoneTarget.transform.position.y, 21.5f), goPrefabSafeZoneTarget.transform.rotation);
                goPowerUp = Instantiate(goPrefabPowerUp, new Vector3(-22.5f, goPrefabPowerUp.transform.position.y, 0f), goPrefabPowerUp.transform.rotation);
                break;
            case 5:
                // Level 6
                goPlayer = Instantiate(goPrefabPlayer, new Vector3(0f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
                goSafeZonePlayer = Instantiate(goPrefabSafeZonePlayer, new Vector3(0f, goPrefabSafeZonePlayer.transform.position.y, 20f), goPrefabSafeZonePlayer.transform.rotation);
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
            "SafeZonePlayer",
            "SafeZoneTarget",
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

}
