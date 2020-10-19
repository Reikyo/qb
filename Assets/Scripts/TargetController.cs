using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetController : MonoBehaviour
{
    public bool bInPlay = true;
    public bool bActive = false;
    public bool bSafe;
    public string sPlayerBuddySwitchEngagedByTarget = "";
    // public float fForce = 500f;
    public float fSpeed = 5f;
    private float fDistPlayerStop = 3f;
    private Rigidbody rbTarget;
    private NavMeshAgent navTarget;
    private Material matTarget;
    private GameManager gameManager;
    private GameObject goPlayer;
    private GameObject goSafeZoneTarget;
    public string sObjective = "None";
    private Vector2 v2DirectionRandom;
    private Vector3 v3DirectionRandom;
    private float fObjectiveRandomTimeStart;
    private float fObjectiveRandomTimeStartThisDirection;
    private float fObjectiveRandomTimeSpendThisDirection;
    private float fObjectiveRandomEmissionFreq = 1f;
    private float fObjectiveRandomEmissionAngFreq;
    private Color colObjectiveRandomEmissionColor = new Color(255f, 70f, 0f, 255f) / 255f;
    private Color colObjectiveRandomEmissionColorNow;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        rbTarget = GetComponent<Rigidbody>();
        navTarget = GetComponent<NavMeshAgent>();
        matTarget = GetComponent<Renderer>().material;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        goPlayer = GameObject.FindWithTag("Player");
        goSafeZoneTarget = GameObject.FindWithTag("SafeZoneTarget");
        bSafe = !goSafeZoneTarget;
        fObjectiveRandomEmissionAngFreq = 2f * Mathf.PI * fObjectiveRandomEmissionFreq;
        SetDirectionRandom();
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        // Game Manager bActive check intentionally left out here, so that target can still move further
        // into the safe zone even after that action triggers the level clearing
        if (bInPlay
        &&  bActive)
        {
            if ((sObjective == "Player")
            &&  goPlayer
            &&  goPlayer.GetComponent<PlayerController>().bInPlay)
            {
                // Vector3 v3Objective = goPlayer.transform.position;
                // Vector3 v3Direction = (v3Objective - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3Direction, Space.World);
                // Move(goPlayer.transform.position);

                // This condition should remain here rather than be put into the parent condition, as the final
                // "else" of this code block will otherwise immediately set bActive to false, as the player is too
                // close when it collides with the target
                if (Math.Abs((goPlayer.transform.position - transform.position).magnitude) > fDistPlayerStop)
                {
                    navTarget.destination = goPlayer.transform.position;
                }
                else
                {
                    navTarget.destination = transform.position;
                }
            }
            else if (sObjective == "Random")
            {
                Move(transform.position + v3DirectionRandom);
                colObjectiveRandomEmissionColorNow = (0.4f - 0.2f * Mathf.Cos(fObjectiveRandomEmissionAngFreq * (Time.time - fObjectiveRandomTimeStart))) * colObjectiveRandomEmissionColor;
                colObjectiveRandomEmissionColorNow.a = 1f;
                matTarget.SetColor("_EmissionColor", colObjectiveRandomEmissionColorNow);
                if ((Time.time - fObjectiveRandomTimeStartThisDirection) >= fObjectiveRandomTimeSpendThisDirection)
                {
                    SetDirectionRandom();
                }
            }
            else if (sObjective == "SafeZoneTarget")
            {
                // Vector3 v3Objective = new Vector3(goSafeZoneTarget.transform.position.x, 1f, goSafeZoneTarget.transform.position.z);
                // Vector3 v3Direction = (v3Objective - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3Direction, Space.World);
                // Move(new Vector3(goSafeZoneTarget.transform.position.x, transform.position.y, goSafeZoneTarget.transform.position.z));
                navTarget.destination = new Vector3(
                    goSafeZoneTarget.transform.position.x,
                    transform.position.y,
                    goSafeZoneTarget.transform.position.z
                );
            }
            else
            {
                bActive = false;
                navTarget.enabled = false;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void Move(Vector3 v3PositionObjective)
    {
        Vector3 v3DirectionMove = (v3PositionObjective - transform.position).normalized;
        Vector3 v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fSpeed * Time.deltaTime, 0f);

        transform.position = Vector3.MoveTowards(transform.position, v3PositionObjective, fSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(v3DirectionLook);

        Debug.DrawRay(transform.position, v3DirectionMove * 10f, Color.yellow);
    }

    // ------------------------------------------------------------------------------------------------

    private void SetDirectionRandom()
    {
        v2DirectionRandom = UnityEngine.Random.insideUnitCircle.normalized * 100f;
        v3DirectionRandom = new Vector3(v2DirectionRandom.x, transform.position.y, v2DirectionRandom.y);
        fObjectiveRandomTimeStartThisDirection = Time.time;
        fObjectiveRandomTimeSpendThisDirection = UnityEngine.Random.Range(0.5f, 2f);
    }

    // ------------------------------------------------------------------------------------------------

    public void StartObjectivePlayer()
    {
        bActive = true;
        sObjective = "Player";
        navTarget.enabled = true;
        matTarget.DisableKeyword("_EMISSION");
        transform.Find("Trail").gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------------------------------

    public void StartObjectiveRandom()
    {
        bActive = true;
        sObjective = "Random";
        navTarget.enabled = false;
        matTarget.EnableKeyword("_EMISSION");
        transform.Find("Trail").gameObject.SetActive(true);
        fObjectiveRandomTimeStart = Time.time;
        fObjectiveRandomTimeStartThisDirection = fObjectiveRandomTimeStart;
    }

    // ------------------------------------------------------------------------------------------------

    public void FinishObjective()
    {
        bActive = false;
        sObjective = "None";
        navTarget.enabled = false;
        matTarget.DisableKeyword("_EMISSION");
        transform.Find("Trail").gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (sObjective == "Random")
        {
            SetDirectionRandom();
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bInPlay = false;
            navTarget.enabled = false;
            gameManager.LevelFailed("Look after your buddy!");
        }
        else if (other.gameObject.CompareTag("PlayerBuddySwitch"))
        {
            other.gameObject.GetComponent<PlayerBuddySwitchController>().bEngagedByTarget = true;
            sPlayerBuddySwitchEngagedByTarget = other.gameObject.name;
        }
        else if (other.gameObject.CompareTag("SafeZoneTarget")
        &&  (sObjective == "Player"))
        {
            Destroy(other);
            bSafe = true;
            sObjective = "SafeZoneTarget";
            if (goPlayer.GetComponent<PlayerController>().bSafe)
            {
                gameManager.LevelCleared();
            }
            else
            {
                gameManager.SfxclpPlay("sfxclpLevelClearedPartial");
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBuddySwitch"))
        {
            other.gameObject.GetComponent<PlayerBuddySwitchController>().bEngagedByTarget = false;
            sPlayerBuddySwitchEngagedByTarget = "";
        }
    }

    // ------------------------------------------------------------------------------------------------

}
