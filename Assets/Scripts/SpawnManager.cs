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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Instantiate()
    {
        foreach (GameObject goPrefab in new List<GameObject>() {goPrefabPlayer, goPrefabEnemy, goPrefabTarget, goPrefabPowerUp, goPrefabSafeZone})
        {
            Instantiate(goPrefab, new Vector3(Random.Range(-fXLimitSpawn, fXLimitSpawn), goPrefab.transform.position.y, Random.Range(-fZLimitSpawn, fZLimitSpawn)), goPrefab.transform.rotation);
        }
    }

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
}
