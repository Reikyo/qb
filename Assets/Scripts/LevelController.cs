using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Vector3 v3InstantiatePosition = new Vector3(0f, -5f, 1f);
    private Vector3 v3LevelPosition = new Vector3(0f, 0f, 0f);

    private float fLevelStartTime = 0.5f;

    private float fLevelStartMetresPerSecY;
    private float fLevelStartMetresPerFrameY;
    private float fLevelStartMetresPerSecZ;
    private float fLevelStartMetresPerFrameZ;

    public bool bLevelStart = false;
    public bool bLevelFinish = false;

    private bool bLevelPositionY = false;
    private bool bLevelPositionZ = false;

    public GameObject goSpawnManager;
    public GameObject goCube;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        fLevelStartMetresPerSecY = (v3LevelPosition.y - v3InstantiatePosition.y) / fLevelStartTime;
        fLevelStartMetresPerFrameY = fLevelStartMetresPerSecY * Time.deltaTime;
        fLevelStartMetresPerSecZ = (v3LevelPosition.z - v3InstantiatePosition.z) / fLevelStartTime;
        fLevelStartMetresPerFrameZ = fLevelStartMetresPerSecZ * Time.deltaTime;

        transform.position = v3InstantiatePosition;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

        // ------------------------------------------------------------------------------------------------

        if (bLevelStart)
        {
            if (!bLevelPositionY)
            {
                bLevelPositionY = Translate("y", fLevelStartMetresPerFrameY, transform.position.y, v3LevelPosition.y, bLevelPositionY);
            }
            if (!bLevelPositionZ)
            {
                bLevelPositionZ = Translate("z", fLevelStartMetresPerFrameZ, transform.position.z, v3LevelPosition.z, bLevelPositionZ);
            }
            if (bLevelPositionY && bLevelPositionZ)
            {
                bLevelPositionY = false;
                bLevelPositionZ = false;
                bLevelStart = false;
                Activate();
            }
        }

        // ------------------------------------------------------------------------------------------------

        if (bLevelFinish)
        {
            if (!bLevelPositionY)
            {
                bLevelPositionY = Translate("y", -fLevelStartMetresPerFrameY, transform.position.y, v3InstantiatePosition.y, bLevelPositionY);
            }
            if (!bLevelPositionZ)
            {
                bLevelPositionZ = Translate("z", -fLevelStartMetresPerFrameZ, transform.position.z, v3InstantiatePosition.z, bLevelPositionZ);
            }
            if (bLevelPositionY && bLevelPositionZ)
            {
                bLevelPositionY = false;
                bLevelPositionZ = false;
                bLevelFinish = false;
                gameObject.SetActive(false);
                goCube.GetComponent<CubeController>().bNextLevelStart = true;
            }
        }

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

    private bool Translate(string sAxis, float fMetresPerFrame, float fCurrentPosition, float fTargetPosition, bool bTargetPosition)
    {
        if (((fMetresPerFrame > 0f) && (fTargetPosition > (fCurrentPosition + fMetresPerFrame)))
        ||  ((fMetresPerFrame < 0f) && (fTargetPosition < (fCurrentPosition + fMetresPerFrame))))
        {
            switch(sAxis)
            {
                case "x":
                    transform.Translate(fMetresPerFrame, 0f, 0f, Space.World);
                    break;
                case "y":
                    transform.Translate(0f, fMetresPerFrame, 0f, Space.World);
                    break;
                case "z":
                    transform.Translate(0f, 0f, fMetresPerFrame, Space.World);
                    break;
            }
        }
        else
        {
            switch(sAxis)
            {
                case "x":
                    transform.Translate(fTargetPosition - fCurrentPosition, 0f, 0f, Space.World);
                    break;
                case "y":
                    transform.Translate(0f, fTargetPosition - fCurrentPosition, 0f, Space.World);
                    break;
                case "z":
                    transform.Translate(0f, 0f, fTargetPosition - fCurrentPosition, Space.World);
                    break;
            }
            bTargetPosition = true;
        }
        return(bTargetPosition);
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelStart()
    {
        gameObject.SetActive(true);
        bLevelStart = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelFinish()
    {
        goSpawnManager.GetComponent<SpawnManager>().Destroy();
        bLevelFinish = true;
    }

    // ------------------------------------------------------------------------------------------------

    private void Activate()
    {
        goSpawnManager.GetComponent<SpawnManager>().Instantiate();
    }

    // ------------------------------------------------------------------------------------------------

}
