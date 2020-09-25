using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public bool bInPlay = true;
    public bool bActive = true;
    public bool bSafe = false;
    // private float fForce = 1000f;
    private float fSpeed = 10f;
    private Rigidbody rbPlayer;
    // private Animator anPlayer;
    public Animator[] anPlayerChildren;
    private GameObject goGameManager;
    private GameObject goEnemy;
    private GameObject goTarget;
    public GameObject goProjectile;
    private int iNumProjectile = 0;
    private TextMeshProUGUI guiNumProjectile;
    private List<string> slistChangeTargetObjective = new List<string>() {"None", "Random"};

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        // anPlayer = GetComponent<Animator>();
        // anPlayerChildren = GetComponentsInChildren<Animator>(); // n.b. This only gets the component of the first child in the tree
        goGameManager = GameObject.Find("Game Manager");
        goEnemy = GameObject.FindWithTag("Enemy");
        goTarget = GameObject.FindWithTag("Target");
        guiNumProjectile = GameObject.Find("Value : Num Projectile").GetComponent<TextMeshProUGUI>();
        guiNumProjectile.text = iNumProjectile.ToString();
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    // Using FixedUpdate() rather than Update() is good for motion, as it restricts such things as
    // different objects clipping into each other, and generally makes motion smoother.
    void FixedUpdate()
    {
        if (bInPlay
        &&  bActive
        &&  goGameManager.GetComponent<GameManager>().bActive)
        {
            float inputHorz = Input.GetAxis("Horizontal");
            float inputVert = Input.GetAxis("Vertical");

            if (Math.Abs(inputHorz) + Math.Abs(inputVert) > 0f)
            {
                foreach (Animator anPlayerChild in anPlayerChildren)
                {
                    anPlayerChild.SetFloat("fSpeed", 1f);
                }
                // anPlayer.SetFloat("Speed_f", 1f);
                Move(transform.position + ((inputHorz * Vector3.right) + (inputVert * Vector3.forward)).normalized);
            }
            else
            {
                foreach (Animator anPlayerChild in anPlayerChildren)
                {
                    anPlayerChild.SetFloat("fSpeed", 0f);
                }
                // anPlayer.SetFloat("Speed_f", 0f);
            }
        }
        else
        {
            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetFloat("fSpeed", 0f);
            }
        }
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
            // desired. No idea why, but it seems valid to have both Update() and FixedUpdate() and methods
            // present, hence the current code block.
            if ((iNumProjectile > 0) && Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(goProjectile, transform.position, transform.rotation);
                iNumProjectile -=1;
                guiNumProjectile.text = iNumProjectile.ToString();
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
        transform.position = Vector3.MoveTowards(transform.position, transform.position + v3DirectionMove, fSpeed * Time.deltaTime);

        Vector3 v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(v3DirectionLook);

        Debug.DrawRay(transform.position, v3DirectionMove * 10f, Color.blue);
    }

    // ------------------------------------------------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (slistChangeTargetObjective.Contains(goTarget.GetComponent<TargetController>().sObjective))
            {
                goGameManager.GetComponent<GameManager>().SfxclpPlay("sfxclpTargetObjectivePlayer");
                goTarget.GetComponent<TargetController>().sObjective = "Player";
                // goTarget.GetComponent<TargetController>().fForce = 500f;
                goTarget.GetComponent<TargetController>().fSpeed = 5f;
                if (goEnemy)
                {
                    goEnemy.GetComponent<EnemyController>().sObjective = "Target";
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger") && goGameManager.GetComponent<GameManager>().bActive)
        {
            bInPlay = false;
            goGameManager.GetComponent<GameManager>().LevelFailed();
        }
        else if (other.gameObject.CompareTag("SafeZonePlayer"))
        {
            if (!goTarget || goTarget.GetComponent<TargetController>().bSafe)
            {
                Destroy(other);
                bSafe = true;
                goGameManager.GetComponent<GameManager>().LevelCleared();
            }
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            iNumProjectile += 20;
            guiNumProjectile.text = iNumProjectile.ToString();
        }
    }

    // ------------------------------------------------------------------------------------------------

}
