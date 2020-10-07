using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public List<string> slistOkayTriggerCharacters = new List<string>() {"Player", "Target"};
    public string sTriggerCharacter = "";
    public bool bPositionFullyOff = true;
    public bool bPositionFullyOn = false;
    private bool bRotateOn = false;
    private bool bRotateOff = false;
    private Vector3 v3RotationAxis = Vector3.forward;
    private int iRotationDirection = -1;
    private float fDegreesPerSec = 360f;
    private float fDegreesPerFrame;
    private float fDegreesToRotate = 30f;
    private float fDegreesRotated = 0f;

    private GameManager gameManager;

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
        if (bRotateOn)
        {
            bRotateOn = Rotate(bRotateOn);
            if (bPositionFullyOff)
            {
                bPositionFullyOff = false;
                gameManager.SfxclpPlay("sfxclpSwitch");
            }
            if (!bRotateOn)
            {
                bPositionFullyOn = true;
                iRotationDirection = 1;
            }
        }
        else if (bRotateOff)
        {
            bRotateOff = Rotate(bRotateOff);
            if (bPositionFullyOn)
            {
                bPositionFullyOn = false;
                gameManager.SfxclpPlay("sfxclpSwitch");
            }
            if (!bRotateOff)
            {
                sTriggerCharacter = "";
                bPositionFullyOff = true;
                iRotationDirection = -1;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void StartRotate(int iRotationDirectionGiven)
    {
        if (iRotationDirectionGiven == -1)
        {
            bRotateOn = true;
        }
        else if (iRotationDirectionGiven == 1)
        {
            bRotateOff = true;
        }
    }

    // ------------------------------------------------------------------------------------------------

    private bool Rotate(bool bRotate)
    {
        fDegreesPerFrame = fDegreesPerSec * Time.deltaTime;
        fDegreesRotated += fDegreesPerFrame;

        if (fDegreesRotated > fDegreesToRotate)
        {
            fDegreesRotated -= fDegreesPerFrame;
            fDegreesPerFrame = fDegreesToRotate - fDegreesRotated;
            bRotate = false;
        }

        transform.Rotate(iRotationDirection * fDegreesPerFrame * v3RotationAxis);

        if (!bRotate)
        {
            transform.eulerAngles = new Vector3(
                Mathf.Round(transform.eulerAngles.x),
                Mathf.Round(transform.eulerAngles.y),
                Mathf.Round(transform.eulerAngles.z)
            );
            fDegreesRotated = 0f;
        }

        return(bRotate);
    }

    // ------------------------------------------------------------------------------------------------

}
