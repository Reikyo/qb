using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpIfTrappedController : MonoBehaviour
{
    private GameObject goPlayer;

    [System.Serializable]
    public class TrapZone
    {
        public GameObject goWallVerticalLower; // Level-1: (3)(6) & (4)(6)
        public GameObject goWallVerticalUpper; // Level-1:  (3)(7) & (4)(7)
        public GameObject goWallHorizontalLower; // Level-1: (6)(3) & (6)(4)
        public GameObject goWallHorizontalUpper; // Level-1: (6)(4) & (6)(5)
        public GameObject goWarpFrom;
    }

    public TrapZone[] customArrTrapZone;

    // public GameObject[] goArrWallTimed;
    // public GameObject[] goArrWarpFrom;
    // private int iNumWallTimedActive = 0;
    // private bool bWarp = false;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {

    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        // if (!goPlayer)
        // {
        //     goPlayer = GameObject.FindWithTag("Player");
        // }
        // else if (   (goPlayer.transform.position.x >= 0f)
        //         &&  (goPlayer.transform.position.x <= 4.9f)
        //         &&  (goPlayer.transform.position.z >= -14.7)
        //         &&  (goPlayer.transform.position.z <= -4.9) )
        // {
        //     if (!bWarp)
        //     {
        //         iNumWallTimedActive = 0;
        //         foreach (GameObject goWallTimed in goArrWallTimed)
        //         {
        //             if (goWallTimed.activeSelf)
        //             {
        //                 iNumWallTimedActive += 1;
        //             }
        //         }
        //         if (iNumWallTimedActive == goArrWallTimed.Length)
        //         {
        //             bWarp = true;
        //             foreach (GameObject goWarpFrom in goArrWarpFrom)
        //             {
        //                 goWarpFrom.SetActive(true);
        //             }
        //         }
        //     }
        // }
        // else if (bWarp)
        // {
        //     iNumWallTimedActive = 0;
        //     bWarp = false;
        //     foreach (GameObject goWarpFrom in goArrWarpFrom)
        //     {
        //         goWarpFrom.SetActive(false);
        //     }
        // }

        if (!goPlayer)
        {
            goPlayer = GameObject.FindWithTag("Player");
        }
        else
        {
            foreach (TrapZone customTrapZone in customArrTrapZone)
            {
                if (    customTrapZone.goWallVerticalLower.activeSelf
                    &&  customTrapZone.goWallVerticalUpper.activeSelf
                    &&  customTrapZone.goWallHorizontalLower.activeSelf
                    &&  customTrapZone.goWallHorizontalUpper.activeSelf
                    &&  (goPlayer.transform.position.x >= customTrapZone.goWallVerticalLower.transform.position.x)
                    &&  (goPlayer.transform.position.x <= customTrapZone.goWallVerticalUpper.transform.position.x)
                    &&  (goPlayer.transform.position.z >= customTrapZone.goWallHorizontalLower.transform.position.z)
                    &&  (goPlayer.transform.position.z <= customTrapZone.goWallHorizontalUpper.transform.position.z) )
                {
                    customTrapZone.goWarpFrom.SetActive(true);
                }
                else if (customTrapZone.goWarpFrom.activeSelf)
                {
                    customTrapZone.goWarpFrom.SetActive(false);
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
