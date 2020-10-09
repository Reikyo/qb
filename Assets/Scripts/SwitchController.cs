using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject goDevice;
    public List<string> slistOkayTriggerCharacters = new List<string>() {"Player", "Target"};
    public string sTriggerCharacter = "";
    private bool bState1 = true;
    private bool bState2 = false;
    private bool bChangeState1to2 = false;
    private bool bChangeState2to1 = false;
    private int iDirection = -1;
    private float fDegreesToRotate = 30f;
    private float fDegreesRotated = 0f;
    private float fDegreesPerSec = 360f;
    private float fDegreesPerFrame;
    private Vector3 v3RotationAxis = Vector3.forward;

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
            bChangeState1to2 = Rotate(bChangeState1to2);
            if (bState1)
            {
                bState1 = false;
                gameManager.SfxclpPlay("sfxclpSwitch");
            }
            if (!bChangeState1to2)
            {
                bState2 = true;
                iDirection = 1;
                if (goDevice)
                {
                    DeviceTrigger(goDevice, 1);
                }
            }
        }
        else if (bChangeState2to1)
        {
            bChangeState2to1 = Rotate(bChangeState2to1);
            if (bState2)
            {
                bState2 = false;
                gameManager.SfxclpPlay("sfxclpSwitch");
                if (goDevice)
                {
                    DeviceTrigger(goDevice, -1);
                }
            }
            if (!bChangeState2to1)
            {
                sTriggerCharacter = "";
                bState1 = true;
                iDirection = -1;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(string sChangeState, string sTriggerCharacterGiven)
    {
        if ((sChangeState == "state1to2")
        &&  bState1
        &&  slistOkayTriggerCharacters.Contains(sTriggerCharacterGiven))
        {
            sTriggerCharacter = sTriggerCharacterGiven;
            bChangeState1to2 = true;
        }
        else if ((sChangeState == "state2to1")
        &&  !bState1
        &&  (sTriggerCharacterGiven == sTriggerCharacter))
        {
            bChangeState2to1 = true;
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void DeviceTrigger(GameObject goDevice, int iChangeState)
    {
        switch(goDevice.tag)
        {
            case "WallSlider":
                goDevice.GetComponent<WallSliderController>().Trigger();
                break;
            case "WallSpinner":
                goDevice.GetComponent<WallSpinnerController>().Trigger(iChangeState);
                break;
        }
    }

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
