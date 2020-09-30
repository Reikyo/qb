using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject goScreenTitle;
    public GameObject goScreenLevelCleared;
    public GameObject goScreenLevelFailed;
    public GameObject goScreenHUD;
    public TextMeshProUGUI guiNumProjectile;
    public TextMeshProUGUI guiLevelFailedHelp;
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

    public bool bNumProjectileFlash = false;
    private float fNumProjectileFlashTimeStart;
    private float fNumProjectileFlashFreq = 1f;
    private float fNumProjectileFlashAngFreq;
    private Color colNumProjectileFlashVertexColorNow = new Color(255f, 255f, 255f, 255f) / 255f;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        sfxsrcGameManager = GetComponent<AudioSource>();

        goSpawnManager = GameObject.Find("Spawn Manager");
        goCube = GameObject.Find("Cube");
        goCube.SetActive(true);
        goScreenTitle.SetActive(true);

        fNumProjectileFlashAngFreq = 2f * Mathf.PI * fNumProjectileFlashFreq;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

        // ------------------------------------------------------------------------------------------------

        // Temporarily allow level to be cleared for testing purposes:
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelCleared();
        }

        // ------------------------------------------------------------------------------------------------

        if (bNumProjectileFlash)
        {
            colNumProjectileFlashVertexColorNow.a = 0.6f + 0.4f * Mathf.Cos(fNumProjectileFlashAngFreq * (Time.time - fNumProjectileFlashTimeStart));
            guiNumProjectile.color = colNumProjectileFlashVertexColorNow;
        }

        // ------------------------------------------------------------------------------------------------

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
        if (bNumProjectileFlash)
        {
            EndNumProjectileFlash();
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelFailed(string sGuiLevelFailedHelpText)
    {
        if (bActive)
        {
            bActive = false;
            guiLevelFailedHelp.text = sGuiLevelFailedHelpText;
            goScreenLevelFailed.SetActive(true);
            sfxsrcGameManager.PlayOneShot(sfxclpLevelFailed);
            if (bNumProjectileFlash)
            {
                EndNumProjectileFlash();
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    // public void LoadScene()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     GameStart();
    // }

    // ------------------------------------------------------------------------------------------------

    public void StartNumProjectileFlash()
    {
        bNumProjectileFlash = true;
        fNumProjectileFlashTimeStart = Time.time;
    }

    // ------------------------------------------------------------------------------------------------

    public void EndNumProjectileFlash()
    {
        bNumProjectileFlash = false;
        colNumProjectileFlashVertexColorNow.a = 1f;
        guiNumProjectile.color = colNumProjectileFlashVertexColorNow;
    }

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
