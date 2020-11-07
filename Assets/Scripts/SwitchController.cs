using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private GameManager gameManager;

    public enum switcher {stationary, translate, rotate};
    public switcher switcherType;

    public GameObject[] goArrDevice;
    public List<string> sListOkayTriggerCharacters = new List<string>() {"Player", "Target"};
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
            if (switcherType == switcher.stationary)
            {
                bChangeState1to2 = false;
                sTriggerCharacter = "";
                gameManager.SfxclpPlay("sfxclpSwitch");
                if (goArrDevice.Length > 0)
                {
                    DeviceTrigger(goArrDevice, "state1to2");
                }
            }
            else
            {
                if (bState1)
                {
                    bState1 = false;
                    gameManager.SfxclpPlay("sfxclpSwitch");
                }

                if (switcherType == switcher.translate)
                {
                    // bChangeState1to2 = Translate(bChangeState1to2);
                }
                else if (switcherType == switcher.rotate)
                {
                    bChangeState1to2 = Rotate(bChangeState1to2);
                }

                if (!bChangeState1to2)
                {
                    bState2 = true;
                    iDirection = 1;
                    if (goArrDevice.Length > 0)
                    {
                        DeviceTrigger(goArrDevice, "state1to2");
                    }
                }
            }
        }
        else if (bChangeState2to1)
        {
            if (switcherType == switcher.stationary)
            {
                bChangeState2to1 = false;
                sTriggerCharacter = "";
                gameManager.SfxclpPlay("sfxclpSwitch");
                if (goArrDevice.Length > 0)
                {
                    DeviceTrigger(goArrDevice, "state2to1");
                }
            }
            else
            {
                if (bState2)
                {
                    bState2 = false;
                    gameManager.SfxclpPlay("sfxclpSwitch");
                    if (goArrDevice.Length > 0)
                    {
                        DeviceTrigger(goArrDevice, "state2to1");
                    }
                }

                if (switcherType == switcher.translate)
                {
                    // bChangeState2to1 = Translate(bChangeState2to1);
                }
                else if (switcherType == switcher.rotate)
                {
                    bChangeState2to1 = Rotate(bChangeState2to1);
                }

                if (!bChangeState2to1)
                {
                    bState1 = true;
                    iDirection = -1;
                    sTriggerCharacter = "";
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(string sSwitcherTrigger, string sTriggerCharacterGiven)
    {
        if (    (sSwitcherTrigger == "state1to2")
            &&  bState1
            &&  sListOkayTriggerCharacters.Contains(sTriggerCharacterGiven) )
        {
            sTriggerCharacter = sTriggerCharacterGiven;
            bChangeState1to2 = true;
        }
        else if (   (sSwitcherTrigger == "state2to1")
                &&  !bState1
                &&  (sTriggerCharacterGiven == sTriggerCharacter) )
        {
            bChangeState2to1 = true;
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void DeviceTrigger(GameObject[] goArrDevice, string sSwitcherTrigger)
    {
        foreach (GameObject goDevice in goArrDevice)
        {
            switch(goDevice.tag)
            {
                case "Translator":
                    goDevice.GetComponent<TranslatorController>().Trigger("switcher", sSwitcherTrigger);
                    break;
                case "Rotator":
                    goDevice.GetComponent<RotatorController>().Trigger("switcher", sSwitcherTrigger);
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
