﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool bInPlay = false;
    public bool bActiveScreenButton = false;
    public bool bProjectilePathDependentLevel = false;
    public bool bCompletedGameAtLeastOnce = false;

    public GameObject goScreenTitle;
    public GameObject goScreenLevelCleared;
    public GameObject goScreenLevelFailed;
    public GameObject goScreenCredits;
    public GameObject goScreenRestart;
    public GameObject goScreenHUD;
    public TextMeshProUGUI[] guiArrNumProjectileFlash;
    public TextMeshProUGUI guiLevelFailedHelp;
    // public Button butStart;
    // public Button butNextLevel;
    // public Button butTryAgain;

    private GameObject goSpawnManager;
    private CubeController cubeController;

    private GameObject vfxclpName;
    // private Dictionary<string, GameObject> vfxclpNames = new Dictionary<string, GameObject>();
    public GameObject vfxclpWarp; // CFX3_MagicAura_B_Runic 1
    public GameObject vfxclpWallDestructible; // FX_Fireworks_Yellow_Large 1
    public GameObject vfxclpWallTimed; // FX_Fireworks_Green_Large 1

    private AudioSource sfxsrcGameManager;
    private AudioClip sfxclpName;
    // private Dictionary<string, AudioClip> sfxclpNames = new Dictionary<string, AudioClip>();
    public AudioClip sfxclpButton; // DM-CGS-01
    public AudioClip sfxclpLevelClearedPartial; // DM-CGS-26
    public AudioClip sfxclpLevelCleared; // DM-CGS-45
    public AudioClip sfxclpLevelFailed; // DM-CGS-23
    public AudioClip sfxclpBoost; // DM-CGS-46
    public AudioClip sfxclpPowerUp; // DM-CGS-28
    public AudioClip sfxclpProjectile; // DM-CGS-20
    public AudioClip sfxclpLaunch; // DM-CGS-34
    public AudioClip sfxclpWarp; // DM-CGS-42
    public AudioClip sfxclpExchange; // DM-CGS-16
    public AudioClip sfxclpWallDestructible; // DM-CGS-32
    public AudioClip sfxclpWallTimed; // DM-CGS-32
    public AudioClip sfxclpTranslator; // DM-CGS-38
    public AudioClip sfxclpRotator; // DM-CGS-37
    public AudioClip sfxclpSwitcher; // DM-CGS-19
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

    void Start()
    {
        sfxsrcGameManager = GetComponent<AudioSource>();

        // vfxclpNames.Add("vfxclpWallDestructible", vfxclpWallDestructible);

        // sfxclpNames.Add("sfxclpButton", sfxclpButton);
        // sfxclpNames.Add("sfxclpLevelClearedPartial", sfxclpLevelClearedPartial);
        // sfxclpNames.Add("sfxclpLevelCleared", sfxclpLevelCleared);
        // sfxclpNames.Add("sfxclpLevelFailed", sfxclpLevelFailed);
        // sfxclpNames.Add("sfxclpBoost", sfxclpBoost);
        // sfxclpNames.Add("sfxclpPowerUp", sfxclpPowerUp);
        // sfxclpNames.Add("sfxclpProjectile", sfxclpProjectile);
        // sfxclpNames.Add("sfxclpLaunch", sfxclpLaunch);
        // sfxclpNames.Add("sfxclpWarp", sfxclpWarp);
        // sfxclpNames.Add("sfxclpWallDestructible", sfxclpWallDestructible);
        // sfxclpNames.Add("sfxclpTranslator", sfxclpTranslator);
        // sfxclpNames.Add("sfxclpRotator", sfxclpRotator);
        // sfxclpNames.Add("sfxclpSwitcher", sfxclpSwitcher);
        // sfxclpNames.Add("sfxclpTargetObjectivePlayer", sfxclpTargetObjectivePlayer);
        // sfxclpNames.Add("sfxclpTargetObjectiveRandom", sfxclpTargetObjectiveRandom);
        // sfxclpNames.Add("sfxclpEnemyAttack1", sfxclpEnemyAttack1);
        // sfxclpNames.Add("sfxclpEnemyAttack2", sfxclpEnemyAttack2);
        // sfxclpNames.Add("sfxclpEnemySleep", sfxclpEnemySleep);

        goSpawnManager = GameObject.Find("Spawn Manager");
        cubeController = GameObject.Find("Cube").GetComponent<CubeController>();

        goScreenTitle.SetActive(true);
        bActiveScreenButton = true;

        fNumProjectileFlashAngFreq = 2f * Mathf.PI * fNumProjectileFlashFreq;
    }

    // ------------------------------------------------------------------------------------------------

    void Update()
    {

        // ------------------------------------------------------------------------------------------------

        // The GameStart function is currently the only function to be triggered by clicking on an active
        // overlay screen button. In addition to mouse clicks on the button (default), we also accept
        // "return" or "space" to activate the button. Note bActiveScreenButton is only affected by an
        // overlay screen button, not the HUD reset button, otherwise "return" and "space" could always
        // trigger the GameStart function during play, which is not desired:
        if (bActiveScreenButton)
        {
            if (    Input.GetKeyUp(KeyCode.Return)
                ||  Input.GetKeyUp(KeyCode.Space) )
            {
                GameStart();
            }
        }
        else
        {
            // Temporarily allow the level to be cleared for testing purposes:
            // if (Input.GetKeyDown(KeyCode.C))
            // {
            //     LevelCleared();
            // }

            // This is no longer needed now that we have a reset button in the HUD:
            // Temporarily allow the level to be failed for testing purposes:
            // if (Input.GetKeyDown(KeyCode.F))
            // {
            //     LevelFailed();
            // }
        }

        // ------------------------------------------------------------------------------------------------

        if (bNumProjectileFlash)
        {
            colNumProjectileFlashVertexColorNow.a = 0.6f + 0.4f * Mathf.Cos(fNumProjectileFlashAngFreq * (Time.time - fNumProjectileFlashTimeStart));
            foreach (TextMeshProUGUI gui in guiArrNumProjectileFlash)
            {
                gui.color = colNumProjectileFlashVertexColorNow;
            }
        }

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

    // This function starts the game from an overlay screen button click, either at the very beginning
    // or end of the game, or between levels or lives, or on manual reset:
    public void GameStart()
    {
        sfxsrcGameManager.PlayOneShot(sfxclpButton);
        if (goScreenTitle.activeSelf)
        {
            goScreenTitle.SetActive(false);
            cubeController.StartFirstLevel();
        }
        else if (goScreenLevelCleared.activeSelf)
        {
            goScreenLevelCleared.SetActive(false);
            cubeController.StartNextLevel();
            // Destroy characters and items
            // Remove objects
            // Rotate cube
            // Insert objects
            // Instantiate characters and items
        }
        else if (goScreenLevelFailed.activeSelf)
        {
            goScreenLevelFailed.SetActive(false);
            cubeController.RestartThisLevel();
            // goSpawnManager.GetComponent<SpawnManager>().Destroy();
            // goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        }
        else if (goScreenHUD.activeSelf)
        {
            if (bNumProjectileFlash)
            {
                EndNumProjectileFlash();
            }
            cubeController.RestartThisLevel();
        }
        else if (goScreenRestart.activeSelf)
        {
            sfxsrcGameManager.Stop();
            goScreenRestart.SetActive(false);
            cubeController.StartNextLevel();
        }
        bInPlay = true;
        bActiveScreenButton = false;
        goScreenHUD.SetActive(true);
        bProjectilePathDependentLevel = cubeController.GetbProjectilePathDependentLevel();
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelCleared()
    {
        if (bInPlay)
        {
            bInPlay = false;
            goScreenHUD.SetActive(false);
            if (cubeController.GetiLevel() < 5)
            {
                goScreenLevelCleared.SetActive(true);
                bActiveScreenButton = true;
            }
            else
            {
                sfxsrcGameManager.Play();
                goScreenRestart.SetActive(true);
                // Only play the credits if the game is being completed for the first time, otherwise it could be a
                // bit tedious seeing them again (not that anyone would actually be playing over and over again,
                // but still):
                if (!bCompletedGameAtLeastOnce)
                {
                    goScreenCredits.SetActive(true);
                }
                else
                {
                    bActiveScreenButton = true;
                }
            }
            // sfxsrcGameManager.PlayOneShot(sfxclpNames["sfxclpLevelCleared"]);
            sfxsrcGameManager.PlayOneShot(sfxclpLevelCleared);
            if (bNumProjectileFlash)
            {
                EndNumProjectileFlash();
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelFailed(string sGuiLevelFailedHelpText="")
    {
        if (bInPlay)
        {
            bInPlay = false;
            guiLevelFailedHelp.text = sGuiLevelFailedHelpText;
            goScreenHUD.SetActive(false);
            goScreenLevelFailed.SetActive(true);
            bActiveScreenButton = true;
            // sfxsrcGameManager.PlayOneShot(sfxclpNames["sfxclpLevelFailed"]);
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
        foreach (TextMeshProUGUI gui in guiArrNumProjectileFlash)
        {
            gui.color = colNumProjectileFlashVertexColorNow;
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void VfxclpPlay(string sVfxclpName, Vector3 v3Position)
    {
        switch(sVfxclpName)
        {
            case "vfxclpWallDestructible": vfxclpName = vfxclpWallDestructible; break;
            case "vfxclpWallTimed": vfxclpName = vfxclpWallTimed; break;
            case "vfxclpWarp": vfxclpName = vfxclpWarp; break;
        }
        // GameObject go = Instantiate(vfxclpNames[sVfxclpName], v3Position, vfxclpNames[sVfxclpName].transform.rotation);
        GameObject go = Instantiate(vfxclpName, v3Position, vfxclpName.transform.rotation);
        StartCoroutine(WaitUntilDestroy(go, 10f));
    }

    // ------------------------------------------------------------------------------------------------

    public void SfxclpPlay(string sSfxclpName)
    {
        switch(sSfxclpName)
        {
            case "sfxclpButton": sfxclpName = sfxclpButton; break;
            case "sfxclpLevelClearedPartial": sfxclpName = sfxclpLevelClearedPartial; break;
            case "sfxclpLevelCleared": sfxclpName = sfxclpLevelCleared; break;
            case "sfxclpLevelFailed": sfxclpName = sfxclpLevelFailed; break;
            case "sfxclpBoost": sfxclpName = sfxclpBoost; break;
            case "sfxclpPowerUp": sfxclpName = sfxclpPowerUp; break;
            case "sfxclpProjectile": sfxclpName = sfxclpProjectile; break;
            case "sfxclpLaunch": sfxclpName = sfxclpLaunch; break;
            case "sfxclpWarp": sfxclpName = sfxclpWarp; break;
            case "sfxclpExchange": sfxclpName = sfxclpExchange; break;
            case "sfxclpWallDestructible": sfxclpName = sfxclpWallDestructible; break;
            case "sfxclpWallTimed": sfxclpName = sfxclpWallTimed; break;
            case "sfxclpTranslator": sfxclpName = sfxclpTranslator; break;
            case "sfxclpRotator": sfxclpName = sfxclpRotator; break;
            case "sfxclpSwitcher": sfxclpName = sfxclpSwitcher; break;
            case "sfxclpTargetObjectivePlayer": sfxclpName = sfxclpTargetObjectivePlayer; break;
            case "sfxclpTargetObjectiveRandom": sfxclpName = sfxclpTargetObjectiveRandom; break;
            case "sfxclpEnemyAttack1": sfxclpName = sfxclpEnemyAttack1; break;
            case "sfxclpEnemyAttack2": sfxclpName = sfxclpEnemyAttack2; break;
            case "sfxclpEnemySleep": sfxclpName = sfxclpEnemySleep; break;
        }
        sfxsrcGameManager.PlayOneShot(sfxclpName);
        // sfxsrcGameManager.PlayOneShot(sfxclpNames[sSfxclpName]);
    }

    // ------------------------------------------------------------------------------------------------

    IEnumerator WaitUntilDestroy(GameObject go, float fTimeWait)
    {
        yield return new WaitForSeconds(fTimeWait);
        Destroy(go);
    }

    // ------------------------------------------------------------------------------------------------

}
