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
    public GameObject goScreenGameOver;
    public GameObject goScreenHUD;
    public GameObject goSpawnManager;
    public GameObject goCube;
    // public Button butStart;
    // public Button butNextLevel;
    // public Button butTryAgain;
    public bool bActive = false;

    private int iLevel;
    public TextMeshProUGUI guiLevel;

    private AudioSource sfxsrcGameManager;
    public AudioClip sfxclpButton;
    public AudioClip sfxclpLevelCleared;
    public AudioClip sfxclpGameOver;

    // Start is called before the first frame update
    void Start()
    {
        iLevel = 0;
        guiLevel.text = iLevel.ToString();
        sfxsrcGameManager = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart(string screen)
    {
        sfxsrcGameManager.PlayOneShot(sfxclpButton);
        if (screen == "Screen: Title")
        {
            goScreenTitle.SetActive(false);
            goScreenHUD.SetActive(true);
            goCube.GetComponent<CubeController>().bGameStart = true;
        }
        else if (screen == "Screen: Level Cleared")
        {
            iLevel += 1;
            guiLevel.text = iLevel.ToString();
            goScreenLevelCleared.SetActive(false);
            goSpawnManager.GetComponent<SpawnManager>().Destroy();
            goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        }
        else if (screen == "Screen: Game Over")
        {
            goScreenGameOver.SetActive(false);
            goSpawnManager.GetComponent<SpawnManager>().Destroy();
            goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        }
        bActive = true;
    }

    public void LevelCleared()
    {
        goScreenLevelCleared.SetActive(true);
        bActive = false;
        sfxsrcGameManager.PlayOneShot(sfxclpLevelCleared);
    }

    public void GameOver()
    {
        goScreenGameOver.SetActive(true);
        bActive = false;
        sfxsrcGameManager.PlayOneShot(sfxclpGameOver);
    }

    // public void LoadScene()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     GameStart();
    // }
}
