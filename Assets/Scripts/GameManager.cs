using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

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

    private AudioSource sfxsrcGameManager;
    private AudioClip sfxclpName;
    public AudioClip sfxclpButton;
    public AudioClip sfxclpLevelCleared;
    public AudioClip sfxclpLevelFailed;
    public AudioClip sfxclpProjectile;
    public AudioClip sfxclpWallDestructible;
    public AudioClip sfxclpWallMoveable;
    public AudioClip sfxclpTargetObjectivePlayer;
    public AudioClip sfxclpTargetObjectiveRandom;
    public AudioClip sfxclpEnemyAttack1;
    public AudioClip sfxclpEnemyAttack2;
    public AudioClip sfxclpEnemySleep;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goSpawnManager = GameObject.Find("Spawn Manager");
        goCube = GameObject.Find("Cube");

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
            goCube.GetComponent<CubeController>().ThisLevelRestart();
            // goSpawnManager.GetComponent<SpawnManager>().Destroy();
            // goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        }
        bActive = true;
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelCleared()
    {
        bActive = false;
        goScreenLevelCleared.SetActive(true);
        sfxsrcGameManager.PlayOneShot(sfxclpLevelCleared);
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelFailed()
    {
        bActive = false;
        goScreenLevelFailed.SetActive(true);
        sfxsrcGameManager.PlayOneShot(sfxclpLevelFailed);
    }

    // ------------------------------------------------------------------------------------------------

    // public void LoadScene()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     GameStart();
    // }

    // ------------------------------------------------------------------------------------------------

    public void SfxclpPlay(string strSfxclpName)
    {
        switch(strSfxclpName)
        {
            case "Projectile":
                sfxclpName = sfxclpProjectile;
                break;
            case "WallDestructible":
                sfxclpName = sfxclpWallDestructible;
                break;
            case "WallMoveable":
                sfxclpName = sfxclpWallMoveable;
                break;
            case "sfxclpTargetObjectivePlayer":
                sfxclpName = sfxclpTargetObjectivePlayer;
                break;
            case "sfxclpTargetObjectiveRandom":
                sfxclpName = sfxclpTargetObjectiveRandom;
                break;
            case "sfxclpEnemyAttack1":
                sfxclpName = sfxclpEnemyAttack1;
                break;
            case "sfxclpEnemyAttack2":
                sfxclpName = sfxclpEnemyAttack2;
                break;
            case "sfxclpEnemySleep":
                sfxclpName = sfxclpEnemySleep;
                break;
        }
        sfxsrcGameManager.PlayOneShot(sfxclpName);
    }

    // ------------------------------------------------------------------------------------------------

}
