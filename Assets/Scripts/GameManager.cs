using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject goScreenTitle;
    public GameObject goScreenLevelCleared;
    public GameObject goScreenLevelFailed;
    public GameObject goScreenHUD;
    private GameObject goSpawnManager;
    private GameObject goCube;
    // public Button butStart;
    // public Button butNextLevel;
    // public Button butTryAgain;
    public bool bActive = false;

    private int iLevel;
    public TextMeshProUGUI guiLevel;

    private AudioSource sfxsrcGameManager;
    public AudioClip sfxclpButton;
    public AudioClip sfxclpLevelCleared;
    public AudioClip sfxclpLevelFailed;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goSpawnManager = GameObject.Find("Spawn Manager");
        goCube = GameObject.Find("Cube");

        iLevel = 0;
        guiLevel.text = (iLevel + 1).ToString();
        sfxsrcGameManager = GetComponent<AudioSource>();
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        // Temporarily allow level to be cleared for testing purposes:
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelCleared();
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void GameStart(string screen)
    {
        sfxsrcGameManager.PlayOneShot(sfxclpButton);
        if (screen == "Screen: Title")
        {
            goScreenTitle.SetActive(false);
            goScreenHUD.SetActive(true);
            goCube.GetComponent<CubeController>().FirstLevelStart();
        }
        else if (screen == "Screen: Level Cleared")
        {
            iLevel += 1;
            guiLevel.text = (iLevel + 1).ToString();
            goScreenLevelCleared.SetActive(false);
            goCube.GetComponent<CubeController>().NextLevelStart();
            // Destroy characters and items
            // Remove objects
            // Rotate cube
            // Insert objects
            // Instantiate characters and items
        }
        else if (screen == "Screen: Level Failed")
        {
            goScreenLevelFailed.SetActive(false);
            goCube.GetComponent<CubeController>().goLevels[iLevel].GetComponent<LevelController>().Deactivate();
            goCube.GetComponent<CubeController>().goLevels[iLevel].GetComponent<LevelController>().Activate();
            // goSpawnManager.GetComponent<SpawnManager>().Destroy();
            // goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        }
        bActive = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelCleared()
    {
        goScreenLevelCleared.SetActive(true);
        bActive = false;
        sfxsrcGameManager.PlayOneShot(sfxclpLevelCleared);
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelFailed()
    {
        goScreenLevelFailed.SetActive(true);
        bActive = false;
        sfxsrcGameManager.PlayOneShot(sfxclpLevelFailed);
    }

    // ------------------------------------------------------------------------------------------------

    // public void LoadScene()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     GameStart();
    // }

    // ------------------------------------------------------------------------------------------------

}
