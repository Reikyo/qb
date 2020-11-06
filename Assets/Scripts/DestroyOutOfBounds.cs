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
            if (gameObject.tag == "Enemy")
            {
                EnemyController enemyController = gameObject.GetComponent<EnemyController>();
                if (enemyController.goWallTimed)
                {
                    enemyController.goWallTimed.SetActive(false);
                    gameManager.VfxclpPlay("vfxclpWallTimed", enemyController.goWallTimed.transform.position);
                    gameManager.SfxclpPlay("sfxclpWallTimed");
                }
            }
            Destroy(gameObject);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
