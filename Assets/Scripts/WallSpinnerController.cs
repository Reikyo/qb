using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpinnerController : MonoBehaviour
{
    private GameManager gameManager;

    private bool bChangeState = false;
    private int iDirection;
    private float fDegreesToRotate = 90f;
    private float fDegreesRotated = 0f;
    private float fDegreesPerSec = 180f;
    private float fDegreesPerFrame;
    private Vector3 v3RotationAxis = Vector3.up;

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
        if (bChangeState)
        {
            bChangeState = Rotate(bChangeState);
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void Trigger(int iDirectionGiven)
    {
        // Only trigger rotation if not already rotating, else the rotation direction could be messed up
        if (!bChangeState)
        {
            gameManager.SfxclpPlay("sfxclpWallSpinner");
            bChangeState = true;
            iDirection = iDirectionGiven;
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
