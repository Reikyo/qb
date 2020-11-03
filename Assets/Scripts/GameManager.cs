using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool bActive = false;
    public bool bProjectilePathDependentLevel = false;

    public GameObject goScreenTitle;
    public GameObject goScreenLevelCleared;
    public GameObject goScreenLevelFailed;
    public GameObject goScreenCredits;
    public GameObject goScreenHUD;
    public TextMeshProUGUI guiNumProjectile;
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
    public GameObject vfxclpWallTimed; // FX_Fireworks_Yellow_Large 1

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
    public AudioClip sfxclpWallDestructible; // DM-CGS-32
    public AudioClip sfxclpWallTimed; // DM-CGS-32
    public AudioClip sfxclpTranslator; // DM-CGS-38
    public AudioClip sfxclpRotator; // DM-CGS-37
    public AudioClip sfxclpSwitch; // DM-CGS-19
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
        // sfxclpNames.Add("sfxclpSwitch", sfxclpSwitch);
        // sfxclpNames.Add("sfxclpTargetObjectivePlayer", sfxclpTargetObjectivePlayer);
        // sfxclpNames.Add("sfxclpTargetObjectiveRandom", sfxclpTargetObjectiveRandom);
        // sfxclpNames.Add("sfxclpEnemyAttack1", sfxclpEnemyAttack1);
        // sfxclpNames.Add("sfxclpEnemyAttack2", sfxclpEnemyAttack2);
        // sfxclpNames.Add("sfxclpEnemySleep", sfxclpEnemySleep);

        goSpawnManager = GameObject.Find("Spawn Manager");
        cubeController = GameObject.Find("Cube").GetComponent<CubeController>();
        goScreenTitle.SetActive(true);

        fNumProjectileFlashAngFreq = 2f * Mathf.PI * fNumProjectileFlashFreq;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

        // ------------------------------------------------------------------------------------------------

        // Temporarily allow level to be cleared for testing purposes:
        if (Input.GetKeyDown(KeyCode.C))
        {
            LevelCleared();
        }

        // ------------------------------------------------------------------------------------------------

        // Temporarily allow level to be failed for testing purposes:
        if (Input.GetKeyDown(KeyCode.F))
        {
            LevelFailed();
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
        if (screen == "Screen : Title")
        {
            goScreenTitle.SetActive(false);
            cubeController.StartFirstLevel();
        }
        else if (screen == "Screen : Level Cleared")
        {
            goScreenLevelCleared.SetActive(false);
            cubeController.StartNextLevel();
            // Destroy characters and items
            // Remove objects
            // Rotate cube
            // Insert objects
            // Instantiate characters and items
        }
        else if (screen == "Screen : Level Failed")
        {
            goScreenLevelFailed.SetActive(false);
            cubeController.RestartThisLevel();
            // goSpawnManager.GetComponent<SpawnManager>().Destroy();
            // goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        }
        else if (screen == "Screen : Credits")
        {
            goScreenCredits.SetActive(false);
            cubeController.StartNextLevel();
        }
        bActive = true;
        goScreenHUD.SetActive(true);
        bProjectilePathDependentLevel = cubeController.GetbProjectilePathDependentLevel();
    }

    // ------------------------------------------------------------------------------------------------

    public void LevelCleared()
    {
        if (bActive)
        {
            bActive = false;
            goScreenHUD.SetActive(false);
            if (cubeController.GetiLevel() < 5)
            {
                goScreenLevelCleared.SetActive(true);
            }
            else
            {
                goScreenCredits.SetActive(true);
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
        if (bActive)
        {
            bActive = false;
            guiLevelFailedHelp.text = sGuiLevelFailedHelpText;
            goScreenHUD.SetActive(false);
            goScreenLevelFailed.SetActive(true);
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
        guiNumProjectile.color = colNumProjectileFlashVertexColorNow;
    }

    // ------------------------------------------------------------------------------------------------

    public void VfxclpPlay(string strVfxclpName, Vector3 v3Position)
    {
        switch(strVfxclpName)
        {
            case "vfxclpWallDestructible": vfxclpName = vfxclpWallDestructible; break;
            case "vfxclpWallTimed": vfxclpName = vfxclpWallTimed; break;
            case "vfxclpWarp": vfxclpName = vfxclpWarp; break;
        }
        // GameObject go = Instantiate(vfxclpNames[strVfxclpName], v3Position, vfxclpNames[strVfxclpName].transform.rotation);
        GameObject go = Instantiate(vfxclpName, v3Position, vfxclpName.transform.rotation);
        StartCoroutine(WaitUntilDestroy(go, 10f));
    }

    // ------------------------------------------------------------------------------------------------

    public void SfxclpPlay(string strSfxclpName)
    {
        switch(strSfxclpName)
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
            case "sfxclpWallDestructible": sfxclpName = sfxclpWallDestructible; break;
            case "sfxclpWallTimed": sfxclpName = sfxclpWallTimed; break;
            case "sfxclpTranslator": sfxclpName = sfxclpTranslator; break;
            case "sfxclpRotator": sfxclpName = sfxclpRotator; break;
            case "sfxclpSwitch": sfxclpName = sfxclpSwitch; break;
            case "sfxclpTargetObjectivePlayer": sfxclpName = sfxclpTargetObjectivePlayer; break;
            case "sfxclpTargetObjectiveRandom": sfxclpName = sfxclpTargetObjectiveRandom; break;
            case "sfxclpEnemyAttack1": sfxclpName = sfxclpEnemyAttack1; break;
            case "sfxclpEnemyAttack2": sfxclpName = sfxclpEnemyAttack2; break;
            case "sfxclpEnemySleep": sfxclpName = sfxclpEnemySleep; break;
        }
        sfxsrcGameManager.PlayOneShot(sfxclpName);
        // sfxsrcGameManager.PlayOneShot(sfxclpNames[strSfxclpName]);
    }

    // ------------------------------------------------------------------------------------------------

    IEnumerator WaitUntilDestroy(GameObject go, float fTimeWait)
    {
        yield return new WaitForSeconds(fTimeWait);
        Destroy(go);
    }

    // ------------------------------------------------------------------------------------------------

}
