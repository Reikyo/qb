using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunctions;
using TMPro;

public class CubeController : MonoBehaviour
{
    private int iLevel = 0;
    public TextMeshProUGUI guiLevel;
    public TextMeshProUGUI guiNumProjectile;

    public GameObject[] goArrLevels;
    private LevelController levelController;

    private Vector3 v3PositionInstantiate = new Vector3(0f, -230f, 19f);
    private Vector3 v3PositionFirstLevel = new Vector3(0f, -25f, 0f);

    private Vector3 v3EulerAngles;
    private Vector3 v3EulerAnglesInstantiate = new Vector3(253f, 0f, 0f);
    private Vector3 v3EulerAnglesFirstLevel = new Vector3(0f, 0f, 0f);
    private Vector3 v3EulerAnglesNextLevel = new Vector3(0f, 0f, 90f);

    private float fTransitionTimeFirstLevel = 1f;
    private float fTransitionTimeNextLevel = 0.5f;

    private float fMetresPerSecYFirstLevel;
    private float fMetresPerFrameYFirstLevel;
    private float fMetresPerSecZFirstLevel;
    private float fMetresPerFrameZFirstLevel;

    private float fDegreesPerSecXFirstLevel;
    private float fDegreesPerFrameXFirstLevel;
    private float fDegreesPerSecNextLevel;
    private float fDegreesPerFrameNextLevel;

    private bool bChangeStateStartFirstLevel = false;
    private bool bChangeStateStartNextLevel = false;

    private bool bChangeStatePositionY = false;
    private bool bChangeStatePositionZ = false;
    private bool bChangeStateEulerAngleX = false;
    private bool bChangeStateEulerAngleZ = false;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        transform.position = v3PositionInstantiate;
        transform.eulerAngles = v3EulerAnglesInstantiate;
        v3EulerAngles = v3EulerAnglesInstantiate;

        fMetresPerSecYFirstLevel = (v3PositionFirstLevel.y - v3PositionInstantiate.y) / fTransitionTimeFirstLevel;
        fMetresPerSecZFirstLevel = (v3PositionFirstLevel.z - v3PositionInstantiate.z) / fTransitionTimeFirstLevel;

        fDegreesPerSecXFirstLevel = (v3EulerAnglesFirstLevel.x - v3EulerAnglesInstantiate.x) / fTransitionTimeFirstLevel;
        fDegreesPerSecNextLevel = (v3EulerAnglesNextLevel.z - v3EulerAnglesFirstLevel.z) / fTransitionTimeNextLevel;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

        // ------------------------------------------------------------------------------------------------

        if (bChangeStateStartFirstLevel)
        {
            if (bChangeStatePositionY)
            {
                fMetresPerFrameYFirstLevel = fMetresPerSecYFirstLevel * Time.deltaTime;
                bChangeStatePositionY = MyFunctions.Move.Translate(
                    gameObject,
                    "y",
                    fMetresPerFrameYFirstLevel,
                    transform.position.y,
                    v3PositionFirstLevel.y
                );
            }
            if (bChangeStatePositionZ)
            {
                fMetresPerFrameZFirstLevel = fMetresPerSecZFirstLevel * Time.deltaTime;
                bChangeStatePositionZ = MyFunctions.Move.Translate(
                    gameObject,
                    "z",
                    fMetresPerFrameZFirstLevel,
                    transform.position.z,
                    v3PositionFirstLevel.z
                );
            }
            if (bChangeStateEulerAngleX)
            {
                fDegreesPerFrameXFirstLevel = fDegreesPerSecXFirstLevel * Time.deltaTime;
                var tuple = MyFunctions.Move.Rotate(
                    gameObject,
                    "x",
                    fDegreesPerFrameXFirstLevel,
                    v3EulerAngles.x,
                    v3EulerAnglesFirstLevel.x
                );
                bChangeStateEulerAngleX = tuple.Item1;
                v3EulerAngles.x = tuple.Item2;
            }
            if (!bChangeStatePositionY
            &&  !bChangeStatePositionZ
            &&  !bChangeStateEulerAngleX)
            {
                bChangeStateStartFirstLevel = false;
                Activate();
            }
        }

        // ------------------------------------------------------------------------------------------------

        else if (bChangeStateStartNextLevel)
        {
            fDegreesPerFrameNextLevel = fDegreesPerSecNextLevel * Time.deltaTime;

            if (bChangeStateEulerAngleZ)
            {
                var tuple = MyFunctions.Move.Rotate(
                    gameObject,
                    "z",
                    fDegreesPerFrameNextLevel,
                    v3EulerAngles.z,
                    v3EulerAnglesNextLevel.z
                );
                bChangeStateEulerAngleZ = tuple.Item1;
                v3EulerAngles.z = tuple.Item2;
                if (!bChangeStateEulerAngleZ)
                {
                    bChangeStateStartNextLevel = false;
                    Activate();
                }
            }
            else if (bChangeStateEulerAngleX)
            {
                var tuple = MyFunctions.Move.Rotate(
                    gameObject,
                    "x",
                    fDegreesPerFrameNextLevel,
                    v3EulerAngles.x,
                    v3EulerAnglesNextLevel.x
                );
                bChangeStateEulerAngleX = tuple.Item1;
                v3EulerAngles.x = tuple.Item2;
                if (!bChangeStateEulerAngleX)
                {
                    bChangeStateStartNextLevel = false;
                    Activate();
                }
            }
        }

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

    public void StartFirstLevel()
    {
        levelController = goArrLevels[iLevel].GetComponent<LevelController>();
        bChangeStateStartFirstLevel = true;
        bChangeStatePositionY = true;
        bChangeStatePositionZ = true;
        bChangeStateEulerAngleX = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void StartNextLevel()
    {
        levelController.FinishLevel();
    }

    // ------------------------------------------------------------------------------------------------

    public void StartNextLevelContinue()
    {
        bChangeStateStartNextLevel = true;

        if (iLevel % 2 == 0)
        {
            bChangeStateEulerAngleZ = true;
            if (iLevel > 0)
            {
                v3EulerAnglesNextLevel.z += 90f;
                v3EulerAnglesNextLevel.z = Mathf.Round(v3EulerAnglesNextLevel.z);
            }
        }
        else
        {
            bChangeStateEulerAngleX = true;
            v3EulerAnglesNextLevel.x += 90f;
            v3EulerAnglesNextLevel.x = Mathf.Round(v3EulerAnglesNextLevel.x);
        }

        if (iLevel < 5)
        {
            iLevel += 1;
        }
        else
        {
            iLevel = 0;
            v3EulerAngles = new Vector3(0f, 0f, 0f);
            v3EulerAnglesNextLevel = new Vector3(0f, 0f, 90f);
        }

        levelController = goArrLevels[iLevel].GetComponent<LevelController>();
    }

    // ------------------------------------------------------------------------------------------------

    public void RestartThisLevel()
    {
        levelController.Reset();
    }

    // ------------------------------------------------------------------------------------------------

    private void Activate()
    {
        guiLevel.text = (iLevel + 1).ToString();
        guiNumProjectile.text = "0";
        levelController.StartLevel();
    }

    // ------------------------------------------------------------------------------------------------

    public int GetiLevel()
    {
        return(iLevel);
    }

    // ------------------------------------------------------------------------------------------------

    public bool GetbProjectilePathDependentLevel()
    {
        return(levelController.bProjectilePathDependentLevel);
    }

    // ------------------------------------------------------------------------------------------------

    public void TriggerWallsSlider()
    {
        levelController.TriggerWallsSlider();
    }

    // ------------------------------------------------------------------------------------------------

}
