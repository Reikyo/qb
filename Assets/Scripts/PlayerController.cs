using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public bool bInPlay = true;
    public bool bActive = true;
    public bool bSafe;
    private bool bInMotionLastFrame = false;
    private bool bInMotionThisFrame = false;
    // private float fForce = 1000f;
    private float fSpeed = 10f;
    private float anPlayerChildfSpeed;
    private Rigidbody rbPlayer;
    // private Animator anPlayer;
    public Animator[] anPlayerChildren;
    private NavMeshAgent navPlayer;
    private NavMeshPath navPlayerPath;
    private GameObject goGameManager;
    private GameObject goTarget;
    private List<string> slistLeaveTargetObjective = new List<string>() {"Player", "SafeZoneTarget"};
    private GameObject goEnemy;
    private GameObject goSafeZonePlayer;
    public GameObject goProjectile;
    private int iNumProjectile = 0;
    private int iNumProjectileWarning = 5;
    private TextMeshProUGUI guiNumProjectile;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        // anPlayer = GetComponent<Animator>();
        // anPlayerChildren = GetComponentsInChildren<Animator>(); // n.b. This only gets the component of the first child in the tree
        navPlayer = GetComponent<NavMeshAgent>();
        navPlayer.enabled = false; // Only set to true when necessary, otherwise the player will not be able to move off the navmesh i.e. they can't fall off the cube
        navPlayerPath = new NavMeshPath();
        goGameManager = GameObject.Find("Game Manager");
        goTarget = GameObject.FindWithTag("Target");
        goEnemy = GameObject.FindWithTag("Enemy");
        goSafeZonePlayer = GameObject.FindWithTag("SafeZonePlayer");
        bSafe = !goSafeZonePlayer;
        guiNumProjectile = GameObject.Find("Value : Num Projectile").GetComponent<TextMeshProUGUI>();
        guiNumProjectile.text = iNumProjectile.ToString();
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    // Using FixedUpdate() rather than Update() is good for motion, as it restricts such things as
    // different objects clipping into each other, and generally makes motion smoother.
    void FixedUpdate()
    {

        // ------------------------------------------------------------------------------------------------

        if (bInPlay
        &&  bActive
        &&  goGameManager.GetComponent<GameManager>().bActive)
        {
            float inputHorz = Input.GetAxis("Horizontal");
            float inputVert = Input.GetAxis("Vertical");

            if (Math.Abs(inputHorz) + Math.Abs(inputVert) > 0f)
            {
                Move(transform.position + ((inputHorz * Vector3.right) + (inputVert * Vector3.forward)).normalized);
                bInMotionThisFrame = true;
            }
            else
            {
                bInMotionThisFrame = false;
            }
        }
        else if (goSafeZonePlayer
        &&  bSafe
        &&  bInMotionThisFrame)
        {
            if (navPlayer.remainingDistance <= navPlayer.stoppingDistance)
            {
                bInMotionThisFrame = false;
            }
        }
        else
        {
            bInMotionThisFrame = false;
        }

        // ------------------------------------------------------------------------------------------------

        if (bInMotionLastFrame != bInMotionThisFrame)
        {
            if (!bInMotionLastFrame
            &&  bInMotionThisFrame)
            {
                anPlayerChildfSpeed = 1f;
            }
            else if (bInMotionLastFrame
            &&  !bInMotionThisFrame)
            {
                anPlayerChildfSpeed = 0f;
            }
            // Only needed if the character model has multiple animated parts
            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetFloat("fSpeed", anPlayerChildfSpeed);
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
        if (bInPlay
        &&  bActive
        &&  goGameManager.GetComponent<GameManager>().bActive)
        {
            // For some reason, if projectiles are instantiated via FixedUpdate(), then typically more than one
            // appear at any time, usually two or three. If instantiated via Update() then we get only one, as
            // desired. No idea why, but it seems valid to have both Update() and FixedUpdate() methods
            // present, hence the current code block.
            if ((iNumProjectile > 0) && Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(goProjectile, transform.position, transform.rotation);

                iNumProjectile -= 1;
                guiNumProjectile.text = iNumProjectile.ToString();

                if (goGameManager.GetComponent<GameManager>().bProjectilePathDependentLevel)
                {
                    if ((iNumProjectile <= iNumProjectileWarning)
                    &&  (!goGameManager.GetComponent<GameManager>().bNumProjectileFlash))
                    {
                        goGameManager.GetComponent<GameManager>().StartNumProjectileFlash();
                    }
                    else if (iNumProjectile == 0)
                    {
                        navPlayer.enabled = true;
                        navPlayer.CalculatePath(goSafeZonePlayer.transform.position, navPlayerPath);
                        navPlayer.enabled = false;
                        if (navPlayerPath.status == NavMeshPathStatus.PathPartial)
                        {
                            goGameManager.GetComponent<GameManager>().LevelFailed("Watch that ammo!");
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

        Debug.DrawRay(transform.position, v3DirectionMove * 10f, Color.blue);
    }

    // ------------------------------------------------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (bActive
        &&  collision.gameObject.CompareTag("Target")
        &&  !slistLeaveTargetObjective.Contains(goTarget.GetComponent<TargetController>().sObjective))
        {
            goGameManager.GetComponent<GameManager>().SfxclpPlay("sfxclpTargetObjectivePlayer");
            goTarget.GetComponent<TargetController>().StartObjectivePlayer();
            if (goEnemy)
            {
                goEnemy.GetComponent<EnemyController>().sObjective = "Target";
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bInPlay = false;
            goGameManager.GetComponent<GameManager>().LevelFailed("That's a long way down ...");
        }
        else if (other.gameObject.CompareTag("SafeZonePlayer"))
        {
            if (!goTarget
            ||  goTarget.GetComponent<TargetController>().bSafe)
            {
                Destroy(other);
                bSafe = true;
                bInMotionThisFrame = true;
                navPlayer.enabled = true;
                navPlayer.destination = new Vector3(
                    goSafeZonePlayer.transform.position.x,
                    transform.position.y,
                    goSafeZonePlayer.transform.position.z
                );
                goGameManager.GetComponent<GameManager>().LevelCleared();
            }
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            goGameManager.GetComponent<GameManager>().SfxclpPlay("sfxclpPowerUp");
            iNumProjectile += other.gameObject.GetComponent<PowerUpController>().iValue;
            guiNumProjectile.text = iNumProjectile.ToString();
            if ((goGameManager.GetComponent<GameManager>().bProjectilePathDependentLevel)
            &&  (goGameManager.GetComponent<GameManager>().bNumProjectileFlash)
            &&  (iNumProjectile > iNumProjectileWarning))
            {
                goGameManager.GetComponent<GameManager>().EndNumProjectileFlash();
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
