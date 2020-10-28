using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private GameManager gameManager;

    public enum switcher {stationary, translate, rotate};
    public switcher switcherType;

    public GameObject[] goArrDevice;
    public List<string> slistOkayTriggerCharacters = new List<string>() {"Player", "Target"};
    public string sTriggerCharacter = "";

    // private float fMetresToTranslate = XXXf;
    // private float fMetresTranslated = XXXf;
    // private float fMetresPerSec = XXXf;
    // private float fMetresPerFrame;

    private float fDegreesToRotate = 30f;
    private float fDegreesRotated = 0f;
    private float fDegreesPerSec = 360f;
    private float fDegreesPerFrame;

    private int iDirection = -1;
    private Vector3 v3RotationAxis = Vector3.forward;

    private bool bState1 = true;
    private bool bState2 = false;
    private bool bChangeState1to2 = false;
    private bool bChangeState2to1 = false;

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
        if (bChangeState1to2)
        {
            if (bState1)
            {
                bState1 = false;
                gameManager.SfxclpPlay("sfxclpSwitch");
            }
            switch (switcherType)
            {
                case switcher.stationary:
                    bChangeState1to2 = false;
                    break;
                // case switcher.translate:
                //     bChangeState1to2 = Translate(bChangeState1to2);
                //     break;
                case switcher.rotate:
                    bChangeState1to2 = Rotate(bChangeState1to2);
                    break;
            }
            if (!bChangeState1to2)
            {
                bState2 = true;
                iDirection = 1;
                if (goArrDevice.Length > 0)
                {
                    DeviceTrigger(goArrDevice);
                }
            }
        }
        else if (bChangeState2to1)
        {
            if (bState2)
            {
                bState2 = false;
                gameManager.SfxclpPlay("sfxclpSwitch");
                if (goArrDevice.Length > 0)
                {
                    DeviceTrigger(goArrDevice);
                }
            }
            switch (switcherType)
            {
                case switcher.stationary:
                    bChangeState2to1 = false;
                    break;
                // case switcher.translate:
                //     bChangeState2to1 = Translate(bChangeState2to1);
                //     break;
                case switcher.rotate:
                    bChangeState2to1 = Rotate(bChangeState2to1);
                    break;
            }
            if (!bChangeState2to1)
            {
                bState1 = true;
                iDirection = -1;
                sTriggerCharacter = "";
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(string sChangeState, string sTriggerCharacterGiven)
    {
        if (    (sChangeState == "state1to2")
            &&  bState1
            &&  slistOkayTriggerCharacters.Contains(sTriggerCharacterGiven) )
        {
            sTriggerCharacter = sTriggerCharacterGiven;
            bChangeState1to2 = true;
        }
        else if (   (sChangeState == "state2to1")
                &&  !bState1
                &&  (sTriggerCharacterGiven == sTriggerCharacter) )
        {
            bChangeState2to1 = true;
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void DeviceTrigger(GameObject[] goArrDevice)
    {
        foreach (GameObject goDevice in goArrDevice)
        {
            switch(goDevice.tag)
            {
                case "WallSlider":
                    goDevice.GetComponent<WallSliderController>().Trigger();
                    break;
                case "WallSpinner":
                    goDevice.GetComponent<WallSpinnerController>().Trigger("switcher");
                    break;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    // private bool Translate(bool bChangeState)
    // {
    //     <CODE HERE, SHOULD BE SIMILAR TO ROTATE CODE BELOW>
    //     return(bChangeState);
    // }

    // ------------------------------------------------------------------------------------------------

    private bool Rotate(bool bChangeState)
    {
        fDegreesPerFrame = fDegreesPerSec * Time.deltaTime;
        fDegreesRotated += fDegreesPerFrame;

        if (fDegreesRotated > fDegreesToRotate)
        {
            fDegreesRotated -= fDegreesPerFrame;
            fDegreesPerFrame = fDegreesToRotate - fDegreesRotated;
            bChangeState = false;
        }

        transform.Rotate(iDirection * fDegreesPerFrame * v3RotationAxis);

        if (!bChangeState)
        {
            transform.eulerAngles = new Vector3(
                Mathf.Round(transform.eulerAngles.x),
                Mathf.Round(transform.eulerAngles.y),
                Mathf.Round(transform.eulerAngles.z)
            );
            fDegreesRotated = 0f;
        }

        return(bChangeState);
    }

    // ------------------------------------------------------------------------------------------------

}
