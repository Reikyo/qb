﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject goPlayer;
    public GameObject goEnemy;
    public GameObject goTarget;
    public GameObject goPowerUp;
    public GameObject goSafeZone;

    private float fXLimitSpawn = 20f;
    private float fZLimitSpawn = 20f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in new List<GameObject>() {goPlayer, goEnemy, goTarget, goPowerUp, goSafeZone})
        {
            Instantiate(go, new Vector3(Random.Range(-fXLimitSpawn, fXLimitSpawn), go.transform.position.y, Random.Range(-fZLimitSpawn, fZLimitSpawn)), go.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
