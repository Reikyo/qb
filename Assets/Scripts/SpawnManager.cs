using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject goPrefabPlayer;
    public GameObject goPrefabEnemy;
    public GameObject goPrefabTarget;
    public GameObject goPrefabPowerUp;
    public GameObject goPrefabSafeZone;

    private float fXLimitSpawn = 20f;
    private float fZLimitSpawn = 20f;

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

    public void Instantiate()
    {
        // Random instantiate locations for all characters and items:
        // foreach (GameObject goPrefab in new List<GameObject>() {goPrefabPlayer, goPrefabEnemy, goPrefabTarget, goPrefabPowerUp, goPrefabSafeZone})
        // {
        //     Instantiate(goPrefab, new Vector3(Random.Range(-fXLimitSpawn, fXLimitSpawn), goPrefab.transform.position.y, Random.Range(-fZLimitSpawn, fZLimitSpawn)), goPrefab.transform.rotation);
        // }

        Instantiate(goPrefabPlayer, new Vector3(-20f, goPrefabPlayer.transform.position.y, -20f), goPrefabPlayer.transform.rotation);
        Instantiate(goPrefabEnemy, new Vector3(20f, goPrefabEnemy.transform.position.y, -20f), goPrefabEnemy.transform.rotation);
        Instantiate(goPrefabTarget, new Vector3(0f, goPrefabTarget.transform.position.y, -20f), goPrefabTarget.transform.rotation);
        Instantiate(goPrefabPowerUp, new Vector3(-20f, goPrefabPowerUp.transform.position.y, 20f), goPrefabPowerUp.transform.rotation);
        Instantiate(goPrefabSafeZone, new Vector3(20f, goPrefabSafeZone.transform.position.y, 20f), goPrefabSafeZone.transform.rotation);
    }

    // ------------------------------------------------------------------------------------------------

    public void Destroy()
    {
        foreach (string sTag in new List<string>() {"Player", "Enemy", "Target", "PowerUp", "SafeZone"})
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
