using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpIfTrappedController : MonoBehaviour
{
    private GameObject goPlayer;
    public GameObject[] golistWallSlider;
    public GameObject[] golistWarp;
    private int iNumWallSliderDown = 0;
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
                iNumWallSliderDown = 0;
                foreach (GameObject goWallSlider in golistWallSlider)
                {
                    if (goWallSlider.GetComponent<WallSliderController>().bPositionYCurrentDown)
                    {
                        iNumWallSliderDown += 1;
                    }
                }
                if (iNumWallSliderDown == golistWallSlider.Length)
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
            iNumWallSliderDown = 0;
            bWarp = false;
            foreach (GameObject goWarp in golistWarp)
            {
                goWarp.SetActive(false);
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
