using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject goScreenTitle;
    public GameObject goScreenLevelCleared;
    public GameObject goScreenGameOver;
    public GameObject goSpawnManager;
    // public Button butStart;
    // public Button butNextLevel;
    // public Button butTryAgain;
    public bool bActive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart(string screen)
    {
        if (screen == "Screen: Title")
        {
            goScreenTitle.SetActive(false);
        }
        else if (screen == "Screen: Level Cleared")
        {
            goScreenLevelCleared.SetActive(false);
            goSpawnManager.GetComponent<SpawnManager>().Destroy();
        }
        else if (screen == "Screen: Game Over")
        {
            goScreenGameOver.SetActive(false);
            goSpawnManager.GetComponent<SpawnManager>().Destroy();
        }
        goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        bActive = true;
    }

    public void LevelCleared()
    {
        goScreenLevelCleared.SetActive(true);
        bActive = false;
    }

    public void GameOver()
    {
        goScreenGameOver.SetActive(true);
        bActive = false;
    }

    // public void LoadScene()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     GameStart();
    // }
}
