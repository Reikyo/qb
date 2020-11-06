using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

    public bool bInPlay = true;
    public bool bActive = true;
    public bool bSafe;
    private bool bMoveAuto = false;
    private bool bWait = false;
    private bool bInMotionLastFrame = false;
    private bool bInMotionThisFrame = false;
    private bool bBoost = false;
    private bool bLaunch = false;
    private bool bWarp = false;
    private bool bWarpDown = false;
    private bool bWarpUp = false;
    private bool bWarpStageStart = false;
    private float fWarpBoundary = 1.1f;
    private Vector3 v3PositionWarpFrom;
    private Vector3 v3PositionWarpTo;
    private bool bPlayerBuddySwitch = false;
    private bool bPlayerBuddySwitched = false;
    private string sNameExchangerEngagedByTarget;
    private string sNameTranslatorEngagedByPlayer = "";
    // private float fForce = 1000f;
    private float fSpeed = 12f;
    private float fSpeedAnPlayerChild;
    private float fSpeedWarp = 5f;
    private float fForceBoost = 40f;
    private float fForceLaunch = 5f;
    private float fTimeDeltaBoost = 0.1f;
    private float fTimeDeltaWait = 0.5f;
    private Rigidbody rbPlayer;
    // private Animator anPlayer;
    public Animator[] anPlayerChildren;
    private GameObject goPlayerTrail;
    private NavMeshAgent navPlayer;
    private NavMeshPath navPlayerPath;
    private GameObject goTarget;
    private List<string> sListTargetObjectiveLeave = new List<string>() {"Player", "SafeZoneTarget"};
    private GameObject[] goArrEnemy;
    private GameObject goSafeZonePlayer;
    private GameObject goSafeZoneTarget;
    public GameObject goProjectile;
    private int iNumProjectile = 0;
    private int iNumProjectileWarning = 5;
    private TextMeshProUGUI guiNumProjectile;
    private Color colPlayer;
    private Color colTarget;
    private Color colSafeZonePlayer;
    private Color colSafeZoneTarget;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        rbPlayer = GetComponent<Rigidbody>();
        // anPlayer = GetComponent<Animator>();
        // anPlayerChildren = GetComponentsInChildren<Animator>(); // n.b. This only gets the component of the first child in the tree
        goPlayerTrail = transform.Find("Trail").gameObject;
        navPlayer = GetComponent<NavMeshAgent>();
        navPlayer.enabled = false; // Only set to true when necessary, otherwise the player will not be able to move off the navmesh i.e. they can't fall off the cube
        navPlayerPath = new NavMeshPath();

        goTarget = GameObject.FindWithTag("Target");
        goArrEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        goSafeZonePlayer = GameObject.FindWithTag("SafeZonePlayer");
        goSafeZoneTarget = GameObject.FindWithTag("SafeZoneTarget");

        bSafe = !goSafeZonePlayer;
        guiNumProjectile = GameObject.Find("Value : Num Projectile").GetComponent<TextMeshProUGUI>();
        guiNumProjectile.text = iNumProjectile.ToString();

        colPlayer = GetComponent<Renderer>().material.GetColor("_Color");
        if (goTarget)
        {
            colTarget = goTarget.GetComponent<Renderer>().material.GetColor("_Color");
        }
        if (goSafeZonePlayer)
        {
            colSafeZonePlayer = goSafeZonePlayer.GetComponent<SpriteRenderer>().color;
        }
        if (goSafeZoneTarget)
        {
            colSafeZoneTarget = goSafeZoneTarget.GetComponent<SpriteRenderer>().color;
        }
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    // Using FixedUpdate() rather than Update() is good for motion, as it restricts such things as
    // different objects clipping into each other, and generally makes motion smoother.
    void FixedUpdate()
    {

        // ------------------------------------------------------------------------------------------------

        if (    bInPlay
            &&  bActive
            &&  gameManager.bActive )
        {
            float inputHorz = Input.GetAxis("Horizontal");
            float inputVert = Input.GetAxis("Vertical");

            if (Math.Abs(inputHorz) + Math.Abs(inputVert) > 0f)
            {
                bInMotionThisFrame = true;
                Move(transform.position + ((inputHorz * Vector3.right) + (inputVert * Vector3.forward)).normalized);
            }
            else
            {
                bInMotionThisFrame = false;
            }
        }
        else if (bMoveAuto)
        {
            MoveAuto();
        }
        else if (navPlayer.enabled)
        {
            MoveNavMesh();
        }
        else if (   !bWait
                &&  bWarp )
        {
            Warp();
        }
        else if (   !bWait
                &&  bPlayerBuddySwitch )
        {
            PlayerBuddySwitch();
        }
        else
        {
            bInMotionThisFrame = false;
        }

        // ------------------------------------------------------------------------------------------------

        if (bInMotionLastFrame != bInMotionThisFrame)
        {
            if (bInMotionThisFrame)
            {
                fSpeedAnPlayerChild = 1f;
            }
            else
            {
                fSpeedAnPlayerChild = 0f;
            }
            // Only needed if the character model has multiple animated parts
            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetFloat("fSpeed", fSpeedAnPlayerChild);
            }
            // anPlayer.SetFloat("Speed_f", 0f);
        }

        // ------------------------------------------------------------------------------------------------

        bInMotionLastFrame = bInMotionThisFrame;

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (    bInPlay
            &&  bActive
            &&  gameManager.bActive )
        {
            // For some reason, if projectiles are instantiated via FixedUpdate(), then typically more than one
            // appear at any time, usually two or three. If instantiated via Update() then we get only one, as
            // desired. No idea why, but it seems valid to have both Update() and FixedUpdate() methods
            // present, hence the current code block.
            if (    (iNumProjectile > 0)
                &&  Input.GetKeyDown(KeyCode.Space) )
            {
                Instantiate(goProjectile, transform.position, transform.rotation);

                iNumProjectile -= 1;
                guiNumProjectile.text = iNumProjectile.ToString();

                if (gameManager.bProjectilePathDependentLevel)
                {
                    if (    (iNumProjectile <= iNumProjectileWarning)
                        &&  !gameManager.bNumProjectileFlash )
                    {
                        gameManager.StartNumProjectileFlash();
                    }
                    else if (iNumProjectile == 0)
                    {
                        navPlayer.enabled = true;
                        navPlayer.CalculatePath(goSafeZonePlayer.transform.position, navPlayerPath);
                        navPlayer.enabled = false;
                        if (navPlayerPath.status == NavMeshPathStatus.PathPartial)
                        {
                            gameManager.LevelFailed("Watch that ammo!");
                        }
                    }
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void Move(Vector3 v3PositionObjective)
    {
        // Version 1 (slippy)
        // rbPlayer.AddForce(inputHorz * fForce * Time.deltaTime * Vector3.right);
        // rbPlayer.AddForce(inputVert * fForce * Time.deltaTime * Vector3.forward);

        // Version 2 (okay, but then wanted to standardise methodology when I added the rotation)
        // transform.Translate(fSpeed * Time.deltaTime * v3DirectionMove, Space.World);

        // Version 3 (good, but moved some stuff to Update() function to harmonise with enemy and target scripts)
        // Vector3 v3DirectionMove = ((inputHorz * Vector3.right) + (inputVert * Vector3.forward)).normalized;

        Vector3 v3DirectionMove = (v3PositionObjective - transform.position).normalized;
        Vector3 v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fSpeed * Time.deltaTime, 0f);

        transform.position = Vector3.MoveTowards(transform.position, transform.position + v3DirectionMove, fSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(v3DirectionLook);

        if (bActive)
        {
            if (    Input.GetKeyDown(KeyCode.LeftShift)
                &&  !bLaunch )
            {
                StartCoroutine(StartBoost());
            }
            else if (   Input.GetKeyUp(KeyCode.LeftShift)
                    &&  bBoost )
            {
                FinishBoost();
            }
        }

        Debug.DrawRay(transform.position, v3DirectionMove * 10f, Color.blue);
    }

    // ------------------------------------------------------------------------------------------------

    IEnumerator StartBoost()
    {
        bBoost = true;
        goPlayerTrail.SetActive(true);
        transform.Translate(1.0f * -Vector3.forward);
        rbPlayer.AddForce(fForceBoost * transform.forward, ForceMode.Impulse);
        gameManager.SfxclpPlay("sfxclpBoost");
        yield return new WaitForSeconds(fTimeDeltaBoost);
        FinishBoost();
    }

    // ------------------------------------------------------------------------------------------------

    private void FinishBoost()
    {
        bBoost = false;
        goPlayerTrail.SetActive(false);
        rbPlayer.velocity = new Vector3(0f, 0f, 0f);
    }

    // ------------------------------------------------------------------------------------------------

    private void MoveAuto()
    {
        if (Math.Pow(Math.Pow(transform.position.x - v3PositionWarpFrom.x, 2f) + Math.Pow(transform.position.z - v3PositionWarpFrom.z, 2f), 0.5f) > (fSpeed * Time.deltaTime))
        {
            Move(new Vector3(v3PositionWarpFrom.x, transform.position.y, v3PositionWarpFrom.z));
        }
        else
        {
            bInMotionThisFrame = false;
            bMoveAuto = false;
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void MoveNavMesh()
    {
        // This commented line should work, and has been seen to work, but for some reason it doesn't
        // always work, hence the manual calculation ...
        // if (navPlayer.remainingDistance <= navPlayer.stoppingDistance)
        if (Math.Pow(Math.Pow(transform.position.x - navPlayer.destination.x, 2f) + Math.Pow(transform.position.z - navPlayer.destination.z, 2f), 0.5f) <= navPlayer.stoppingDistance)
        {
            // Debug.Log(transform.position);
            // Debug.Log(navPlayer.destination);
            // Debug.Log(navPlayer.remainingDistance);
            // Debug.Log(navPlayer.stoppingDistance);
            bInMotionThisFrame = false;
            if (bWarp)
            {
                navPlayer.enabled = false;
                rbPlayer.isKinematic = true;
            }
            else if (bPlayerBuddySwitch)
            {
                navPlayer.enabled = false;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    IEnumerator Wait()
    {
        bActive = false;
        bWait = true;
        yield return new WaitForSeconds(fTimeDeltaWait);
        bActive = true;
        bWait = false;
    }

    // ------------------------------------------------------------------------------------------------

    private void Warp()
    {
        if (bWarpDown)
        {
            if (bWarpStageStart)
            {
                bWarpStageStart = false;
                gameManager.VfxclpPlay("vfxclpWarp", new Vector3(v3PositionWarpFrom.x, v3PositionWarpFrom.y + 0.5f, v3PositionWarpFrom.z));
                rbPlayer.isKinematic = true; // Suspends regular physics and allows the character to move into the floor
            }
            if (transform.position.y > v3PositionWarpFrom.y - fWarpBoundary)
            {
                transform.Translate(0f, -fSpeedWarp * Time.deltaTime, 0f, Space.World);
            }
            else
            {
                transform.position = new Vector3(v3PositionWarpTo.x, v3PositionWarpTo.y - fWarpBoundary, v3PositionWarpTo.z);
                gameManager.SfxclpPlay("sfxclpWarp");
                bWarpDown = false;
                bWarpUp = true;
                bWarpStageStart = true;
            }
        }
        else if (bWarpUp)
        {
            if (bWarpStageStart)
            {
                bWarpStageStart = false;
                gameManager.VfxclpPlay("vfxclpWarp", new Vector3(v3PositionWarpTo.x, v3PositionWarpTo.y + 0.5f, v3PositionWarpTo.z));
            }
            if (transform.position.y < v3PositionWarpTo.y + fWarpBoundary)
            {
                transform.Translate(0f, fSpeedWarp * Time.deltaTime, 0f, Space.World);
            }
            else
            {
                rbPlayer.isKinematic = false;
                bWarpUp = false;
                bActive = true;
                StartCoroutine(Wait());
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void PlayerBuddySwitch()
    {

        // From first player-buddy switch attempt:

        // This first attempt at the player-buddy switch required both a PlayerController and a
        // TargetController on both the player and the target (buddy). But as the player retained the
        // "Player" tag and the target retained the "Target" tag, various other features of the game still
        // didn't work quite right. I was about to start solving these issues when I realised the
        // alternative approach of physically switching the player and target locations as well as their
        // colours, simply to make it appear like the player and target swapped control abilities but
        // retained their locations. This currently seems to work well, no noticeable delay. Only possible
        // as the same player and target models are in use.

        // if (gameObject.tag == "Player")
        // {
        //     goTarget.GetComponent<TargetController>().enabled = false;
        //     goTarget.GetComponent<TargetController>().bActive = false;
        //     goTarget.GetComponent<TargetController>().sObjective = "None";
        //     goTarget.GetComponent<PlayerController>().enabled = true;
        //     goTarget.GetComponent<PlayerController>().bActive = true;
        //     goTarget.GetComponent<PlayerController>().iNumProjectile = iNumProjectile;
        //     goTarget.GetComponent<NavMeshAgent>().enabled = false;
        //
        //     goPlayer.GetComponent<TargetController>().enabled = true;
        //     goPlayer.GetComponent<PlayerController>().enabled = false;
        // }
        // else
        // {
        //     goPlayer.GetComponent<TargetController>().enabled = false;
        //     goPlayer.GetComponent<TargetController>().bActive = false;
        //     goPlayer.GetComponent<TargetController>().sObjective = "None";
        //     goPlayer.GetComponent<PlayerController>().enabled = true;
        //     goPlayer.GetComponent<PlayerController>().bActive = true;
        //     goPlayer.GetComponent<PlayerController>().iNumProjectile = iNumProjectile;
        //     goPlayer.GetComponent<NavMeshAgent>().enabled = false;
        //
        //     goTarget.GetComponent<TargetController>().enabled = true;
        //     goTarget.GetComponent<PlayerController>().enabled = false;
        // }

        sNameExchangerEngagedByTarget = goTarget.GetComponent<TargetController>().sNameExchangerEngagedByTarget;

        gameManager.VfxclpPlay("vfxclpWarp", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
        gameManager.VfxclpPlay("vfxclpWarp", new Vector3(goTarget.transform.position.x, goTarget.transform.position.y + 0.5f, goTarget.transform.position.z));
        gameManager.SfxclpPlay("sfxclpExchange");

        Vector3 v3PositionPlayerOrig = transform.position;
        Vector3 v3RotationPlayerOrig = transform.eulerAngles;
        transform.position = goTarget.transform.position;
        transform.eulerAngles = goTarget.transform.eulerAngles;
        goTarget.transform.position = v3PositionPlayerOrig;
        goTarget.transform.eulerAngles = v3RotationPlayerOrig;
        rbPlayer.velocity = new Vector3(0f, 0f, 0f);

        if (!bPlayerBuddySwitched)
        {
            GetComponent<Renderer>().material.SetColor("_Color", colTarget);
            goTarget.GetComponent<Renderer>().material.SetColor("_Color", colPlayer);
        }
        else
        {
            GetComponent<Renderer>().material.SetColor("_Color", colPlayer);
            goTarget.GetComponent<Renderer>().material.SetColor("_Color", colTarget);
        }

        if (    goSafeZonePlayer
            &&  goSafeZoneTarget )
        {
            Vector3 v3PositionSafeZonePlayerOrig = goSafeZonePlayer.transform.position;
            goSafeZonePlayer.transform.position = goSafeZoneTarget.transform.position;
            goSafeZoneTarget.transform.position = v3PositionSafeZonePlayerOrig;

            if (!bPlayerBuddySwitched)
            {
                goSafeZonePlayer.GetComponent<SpriteRenderer>().color = colSafeZoneTarget;
                goSafeZoneTarget.GetComponent<SpriteRenderer>().color = colSafeZonePlayer;
            }
            else
            {
                goSafeZonePlayer.GetComponent<SpriteRenderer>().color = colSafeZonePlayer;
                goSafeZoneTarget.GetComponent<SpriteRenderer>().color = colSafeZoneTarget;
            }
        }

        bPlayerBuddySwitch = (sNameExchangerEngagedByTarget != "");
        bPlayerBuddySwitched = !bPlayerBuddySwitched;

        StartCoroutine(Wait());
    }

    // ------------------------------------------------------------------------------------------------

    // This requires a Collider component on both objects, and "Is Trigger" disabled on both of them.
    // Also, a RigidBody component must be on at least this object, the other doesn't matter.
    private void OnCollisionEnter(Collision collision)
    {
        if (bActive)
        {
            if (    collision.gameObject.CompareTag("Cube")
                &&  bLaunch )
            {
                bLaunch = false;
                goPlayerTrail.SetActive(false);
            }
            else if (   collision.gameObject.CompareTag("WallDestructible")
                    &&  bBoost )
            {
                collision.gameObject.SetActive(false);
                gameManager.VfxclpPlay("vfxclpWallDestructible", collision.gameObject.transform.position);
                gameManager.SfxclpPlay("sfxclpWallDestructible");
            }
            else if (   collision.gameObject.CompareTag("Translator")
                    &&  (transform.position.x >= (collision.gameObject.transform.position.x - 2.5f))
                    &&  (transform.position.x <= (collision.gameObject.transform.position.x + 2.5f))
                    &&  (transform.position.z >= (collision.gameObject.transform.position.z - 2.5f))
                    &&  (transform.position.z <= (collision.gameObject.transform.position.z + 2.5f)) )
            {
                transform.parent = collision.gameObject.transform;
                sNameTranslatorEngagedByPlayer = collision.gameObject.name; // We keep track of this as otherwise timing means we could exit from one translator and enter another, but have the exit trigger fulfilled second, so no parent then assigned
            }
            else if (   collision.gameObject.CompareTag("Target")
                    &&  !sListTargetObjectiveLeave.Contains(goTarget.GetComponent<TargetController>().sObjective) )
            {
                gameManager.SfxclpPlay("sfxclpTargetObjectivePlayer");
                goTarget.GetComponent<TargetController>().StartObjectivePlayer();
                foreach (GameObject goEnemy in goArrEnemy)
                {
                    if (goEnemy)
                    {
                        goEnemy.GetComponent<EnemyController>().sObjective = "Target";
                    }
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnCollisionExit(Collision collision)
    {
        if (    collision.gameObject.CompareTag("Translator")
            &&  collision.gameObject.name == sNameTranslatorEngagedByPlayer )
        {
            transform.parent = null;
            sNameTranslatorEngagedByPlayer = "";
        }
    }

    // ------------------------------------------------------------------------------------------------

    // This requires a Collider component on both objects, and "Is Trigger" enabled on one of them.
    // Also, a RigidBody component must be on at least one of them, it doesn't matter which one.
    private void OnTriggerEnter(Collider other)
    {
        if (    other.gameObject.CompareTag("OffGroundTrigger")
            &&  !bWarp )
        {
            bInPlay = false;
            gameManager.LevelFailed("That's a long way down ...");
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            gameManager.SfxclpPlay("sfxclpPowerUp");
            iNumProjectile += other.gameObject.GetComponent<PowerUpController>().iNumProjectile;
            guiNumProjectile.text = iNumProjectile.ToString();
            if (    gameManager.bProjectilePathDependentLevel
                &&  gameManager.bNumProjectileFlash
                &&  (iNumProjectile > iNumProjectileWarning) )
            {
                gameManager.EndNumProjectileFlash();
            }
        }
        else if (other.gameObject.CompareTag("LauncherTriggerOn"))
        {
            bLaunch = true;
            gameManager.SfxclpPlay("sfxclpLaunch");
            goPlayerTrail.SetActive(true);
            rbPlayer.AddForce(fForceLaunch * other.gameObject.transform.right, ForceMode.Impulse);
        }
        else if (   other.gameObject.CompareTag("Warper")
                &&  !bWarp )
        {
            if (other.gameObject.GetComponent<WarpController>().goWarpPartner)
            {
                v3PositionWarpFrom = other.gameObject.transform.position;
                v3PositionWarpTo = other.gameObject.GetComponent<WarpController>().goWarpPartner.transform.position;
                bWarp = true;
                bWarpDown = true;
                bWarpStageStart = true;
                bActive = false;
                bInMotionThisFrame = true;
                bMoveAuto = true;
                // navPlayer.enabled = true;
                // navPlayer.destination = new Vector3(
                //     v3PositionWarpFrom.x,
                //     transform.position.y,
                //     v3PositionWarpFrom.z
                // );
            }
        }
        // From first player-buddy switch attempt:
        // else if (   other.gameObject.CompareTag("Exchanger")
        //         && (!other.gameObject.GetComponent<PlayerBuddySwitchController>().sTriggeredBy == "")
        //         &&  goTarget )
        else if (   other.gameObject.CompareTag("Exchanger")
                &&  !other.gameObject.GetComponent<PlayerBuddySwitchController>().bEngagedByTarget
                &&  !bPlayerBuddySwitch
                &&  goTarget )
        {
            // From first player-buddy switch attempt:
            // other.gameObject.GetComponent<PlayerBuddySwitchController>().sTriggeredBy = gameObject.tag;
            goTarget.GetComponent<TargetController>().FinishObjective();
            v3PositionWarpFrom = other.gameObject.transform.position;
            bPlayerBuddySwitch = true;
            bActive = false;
            bInMotionThisFrame = true;
            bMoveAuto = true;
            // navPlayer.enabled = true;
            // navPlayer.destination = new Vector3(
            //     other.gameObject.transform.position.x,
            //     transform.position.y,
            //     other.gameObject.transform.position.z
            // );
        }
        else if (   other.gameObject.CompareTag("SafeZonePlayer")
                &&  (!goTarget || goTarget.GetComponent<TargetController>().bSafe)
                &&  !bSafe )
        {
            bSafe = true;
            bActive = false;
            bInMotionThisFrame = true;
            navPlayer.enabled = true;
            navPlayer.destination = new Vector3(
                goSafeZonePlayer.transform.position.x,
                transform.position.y,
                goSafeZonePlayer.transform.position.z
            );
            gameManager.LevelCleared();
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerExit(Collider other)
    {
        // From first player-buddy switch attempt:

        // if (    (other.gameObject.CompareTag("Exchanger"))
        //     &&  (other.gameObject.GetComponent<PlayerBuddySwitchController>().sTriggeredBy == gameObject.tag)
        //     &&  (   (GetComponent<PlayerController>().enabled && GetComponent<PlayerController>().bActive)
        //         ||  (GetComponent<TargetController>().enabled && GetComponent<TargetController>().bActive) ) )
        // {
        //     other.gameObject.GetComponent<PlayerBuddySwitchController>().sTriggeredBy = "";
        // }

        if (    other.gameObject.CompareTag("Warper")
            &&  bActive
            &&  bWarp )
        {
            bWarp = false;
        }
        else if (   other.gameObject.CompareTag("Exchanger")
                &&  (other.gameObject.name == sNameExchangerEngagedByTarget)
                &&  bActive
                &&  bPlayerBuddySwitch )
        {
            bPlayerBuddySwitch = false;
        }
    }

    // ------------------------------------------------------------------------------------------------

}
