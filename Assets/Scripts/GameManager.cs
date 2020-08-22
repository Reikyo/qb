using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// using TMPro;

public class GameManager : MonoBehaviour
{
    private GameObject goTitleScreen;
    private GameObject goSpawnManager;
    public Button butStart;
    public bool bActive = false;

    // Start is called before the first frame update
    void Start()
    {
        goTitleScreen = GameObject.Find("Title Screen");
        goSpawnManager = GameObject.Find("Spawn Manager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        goTitleScreen.SetActive(false);
        goSpawnManager.GetComponent<SpawnManager>().GameStart();
        bActive = true;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // goTitleScreen.SetActive(true);
        // goSpawnManager.GetComponent<SpawnManager>().GameOver();
        // bActive = false;
    }
}
