using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameManager gameManager;

    public bool bInPlay = true;
    public bool bActive = true;
    // private float fForce = 500f;
    // public float fSpeed = 6f;
    private float fForcePush = 5f;
    private float fWaitTime = 10f;
    private Rigidbody rbEnemy;
    // private Animator anEnemy;
    private NavMeshAgent navEnemy;
    private GameObject goTarget;
    private List<string> sListLeaveTargetObjective = new List<string>() {"Random", "SafeZoneTarget"};
    private GameObject goPlayer;
    public GameObject goWallTimed;
    public string sObjective;
    // public ParticleSystem psInactive;
    private GameObject goInactive;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        rbEnemy = GetComponent<Rigidbody>();
        // anEnemy = GetComponent<Animator>();
        navEnemy = GetComponent<NavMeshAgent>();
        goTarget = GameObject.FindWithTag("Target");
        goPlayer = GameObject.FindWithTag("Player");
        goInactive = transform.Find("FX_Dust_Prefab_01 1").gameObject;

        if (goTarget)
        {
            sObjective = "Target";
        }
        else if (goPlayer)
        {
            sObjective = "Player";
        }
        else
        {
            sObjective = "None";
        }
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (    bInPlay
            &&  bActive
            &&  gameManager.bActive )
        {
            if (    (sObjective == "Target")
                &&  goTarget
                &&  goTarget.GetComponent<TargetController>().bInPlay )
            {
                // Vector3 v3DirectionMove = (goTarget.transform.position - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3DirectionMove, Space.World);
                // Move(goTarget.transform.position);
                navEnemy.destination = goTarget.transform.position;
            }
            else if (   (sObjective == "Player")
                    &&  goPlayer
                    &&  goPlayer.GetComponent<PlayerController>().bInPlay )
            {
                // Vector3 v3DirectionMove = (goPlayer.transform.position - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3DirectionMove, Space.World);
                // Move(goPlayer.transform.position);
                navEnemy.destination = goPlayer.transform.position;
            }
            else
            {
                bActive = false;
                navEnemy.enabled = false;
                // anEnemy.SetBool("bWalkForward", false);
                // anEnemy.ResetTrigger("tAttack1");
                // anEnemy.ResetTrigger("tAttack2");
            }
        }
        // else if (anEnemy.GetBool("bWalkForward") && (!bInPlay || !gameManager.bActive))
        else if (!gameManager.bActive)
        {
            bActive = false;
            navEnemy.enabled = false;
            // anEnemy.SetBool("bWalkForward", false);
            // anEnemy.ResetTrigger("tAttack1");
            // anEnemy.ResetTrigger("tAttack2");
        }
    }

    // ------------------------------------------------------------------------------------------------

    public void StartWait()
    {
        if (bActive)
        {
            StartCoroutine(Wait());
        }
    }

    // ------------------------------------------------------------------------------------------------

    IEnumerator Wait()
    {
        bActive = false;
        navEnemy.enabled = false;
        // anEnemy.SetBool("bSleep", true);
        // anEnemy.ResetTrigger("tAttack1");
        // anEnemy.ResetTrigger("tAttack2");
        gameManager.SfxclpPlay("sfxclpEnemySleep");
        // psInactive.Play();
        goInactive.SetActive(true);
        if (goWallTimed)
        {
            goWallTimed.SetActive(false);
            gameManager.VfxclpPlay("vfxclpWallTimed", goWallTimed.transform.position);
            gameManager.SfxclpPlay("sfxclpWallTimed");
        }
        yield return new WaitForSeconds(fWaitTime);
        // psInactive.Stop();
        // yield return new WaitForSeconds(7f);
        // anEnemy.SetBool("bSleep", false);
        goInactive.SetActive(false);
        bActive = true;
        navEnemy.enabled = true;
        if (goWallTimed)
        {
            goWallTimed.SetActive(true);
            gameManager.VfxclpPlay("vfxclpWallTimed", goWallTimed.transform.position);
            gameManager.SfxclpPlay("sfxclpWallTimed");
        }
    }

    // ------------------------------------------------------------------------------------------------

    // private void Move(Vector3 v3PositionObjective)
    // {
    //     Vector3 v3DirectionMove = (v3PositionObjective - transform.position).normalized;
    //     Vector3 v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fSpeed * Time.deltaTime, 0f);
    //
    //     transform.position = Vector3.MoveTowards(transform.position, v3PositionObjective, fSpeed * Time.deltaTime);
    //     transform.rotation = Quaternion.LookRotation(v3DirectionLook);
    //
    //     Debug.DrawRay(transform.position, v3DirectionMove * 10f, Color.red);
    // }

    // ------------------------------------------------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (    bActive
            &&  collision.gameObject.CompareTag("Target")
            &&  !sListLeaveTargetObjective.Contains(goTarget.GetComponent<TargetController>().sObjective) )
        {
            // anEnemy.SetTrigger("tAttack1");
            gameManager.SfxclpPlay("sfxclpEnemyAttack1");
            gameManager.SfxclpPlay("sfxclpTargetObjectiveRandom");
            goTarget.GetComponent<TargetController>().StartObjectiveRandom();
            sObjective = "Player";
        }
        else if (   bActive
                &&  collision.gameObject.CompareTag("Player") )
        {
            // anEnemy.SetTrigger("tAttack2");
            gameManager.SfxclpPlay("sfxclpEnemyAttack2");
            collision.gameObject.GetComponent<Rigidbody>().AddForce(fForcePush * (collision.gameObject.transform.position - transform.position).normalized, ForceMode.Impulse);
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bInPlay = false;
            navEnemy.enabled = false;
        }
    }

    // ------------------------------------------------------------------------------------------------

}
