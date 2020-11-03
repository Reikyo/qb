﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpIfTrappedController : MonoBehaviour
{
    private GameObject goPlayer;
    public GameObject[] golistWallTimed;
    public GameObject[] golistWarp;
    private int iNumWallTimedActive = 0;
    private bool bWarp = false;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goPlayer = GameObject.FindWithTag("Player");
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (!goPlayer)
        {
            goPlayer = GameObject.FindWithTag("Player");
        }
        if (    (goPlayer.transform.position.x >= 0f)
            &&  (goPlayer.transform.position.x <= 4.9f)
            &&  (goPlayer.transform.position.z >= -14.7)
            &&  (goPlayer.transform.position.z <= -4.9) )
        {
            if (!bWarp)
            {
                iNumWallTimedActive = 0;
                foreach (GameObject goWallTimed in golistWallTimed)
                {
                    if (goWallTimed.activeSelf)
                    {
                        iNumWallTimedActive += 1;
                    }
                }
                if (iNumWallTimedActive == golistWallTimed.Length)
                {
                    bWarp = true;
                    foreach (GameObject goWarp in golistWarp)
                    {
                        goWarp.SetActive(true);
                    }
                }
            }
        }
        else if (bWarp)
        {
            iNumWallTimedActive = 0;
            bWarp = false;
            foreach (GameObject goWarp in golistWarp)
            {
                goWarp.SetActive(false);
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}