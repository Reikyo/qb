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
    public bool bProjectilePathDependentLevel = false;

    private AudioSource sfxsrcGameManager;
    private AudioClip sfxclpName;
    public AudioClip sfxclpButton; // DM-CGS-01
    public AudioClip sfxclpLevelClearedPartial; // DM-CGS-26
    public AudioClip sfxclpLevelCleared; // DM-CGS-45
    public AudioClip sfxclpLevelFailed; // DM-CGS-23
    public AudioClip sfxclpPowerUp; // DM-CGS-28
    public AudioClip sfxclpProjectile; // DM-CGS-20
    public AudioClip sfxclpWallDestructible; // DM-CGS-32
    public AudioClip sfxclpWallMoveable; // DM-CGS-38
    public AudioClip sfxclpTargetObjectivePlayer; // DM-CGS-24
    public AudioClip sfxclpTargetObjectiveRandom; // DM-CGS-25
    public AudioClip sfxclpEnemyAttack1; // DM-CGS-47
    public AudioClip sfxclpEnemyAttack2; // DM-CGS-30
    public AudioClip sfxclpEnemySleep; // DM-CGS-02

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goSpawnManager = GameObject.Find("Spawn Manager");
        goCube = GameObject.Find("Cube");
        goCube.SetActive(true);
        goScreenTitle.SetActive(true);

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
        bProjectilePathDependentLevel = goCube.GetComponent<CubeController>().GetbProjectilePathDependentLevel();
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
        if (bActive)
        {
            bActive = false;
            goScreenLevelFailed.SetActive(true);
            sfxsrcGameManager.PlayOneShot(sfxclpLevelFailed);
        }
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
            case "sfxclpLevelClearedPartial":
                sfxclpName = sfxclpLevelClearedPartial;
                break;
            case "sfxclpPowerUp":
                sfxclpName = sfxclpPowerUp;
                break;
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
