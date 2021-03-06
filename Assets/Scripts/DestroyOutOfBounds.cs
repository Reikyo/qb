using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private GameManager gameManager;

    private float fXZLimitExist = 50f;
    private float fYLimitExist = -100f;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (    (Math.Abs(transform.position.x) >= fXZLimitExist)
            ||  (Math.Abs(transform.position.z) >= fXZLimitExist)
            ||  (transform.position.y <= fYLimitExist) )
        {
            if (    (   gameObject.CompareTag("Player")
                    ||  gameObject.CompareTag("Target") )
                &&  gameManager.bActive )
            {
                gameManager.LevelFailed("that's a long way down ...");
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                EnemyController enemyController = gameObject.GetComponent<EnemyController>();
                if (enemyController.goListWallTimed.Count > 0)
                {
                    foreach (GameObject goWallTimed in enemyController.goListWallTimed)
                    {
                        goWallTimed.SetActive(false);
                        gameManager.VfxclpPlay("vfxclpWallTimed", goWallTimed.transform.position);
                    }
                    gameManager.SfxclpPlay("sfxclpWallTimed");
                }
            }
            Destroy(gameObject);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
