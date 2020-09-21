using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetController : MonoBehaviour
{
    public bool bInPlay = true;
    public bool bActive = true;
    // public float fForce = 500f;
    public float fSpeed = 5f;
    private float fDistPlayerStop = 5f;
    private Rigidbody rbTarget;
    private NavMeshAgent navTarget;
    private GameObject goGameManager;
    private GameObject goPlayer;
    private GameObject goSafeZoneTarget;
    public string sObjective;
    public Vector3 v3DirectionRand;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        rbTarget = GetComponent<Rigidbody>();
        navTarget = GetComponent<NavMeshAgent>();
        goGameManager = GameObject.Find("Game Manager");
        goPlayer = GameObject.FindWithTag("Player");
        goSafeZoneTarget = GameObject.FindWithTag("SafeZoneTarget");
        sObjective = "None";
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bInPlay && bActive)
        {
            if ((sObjective == "Player")
            &&  goPlayer
            &&  goPlayer.GetComponent<PlayerController>().bInPlay
            &&  (Math.Abs((goPlayer.transform.position - transform.position).magnitude) > fDistPlayerStop))
            {
                // Vector3 v3Objective = goPlayer.transform.position;
                // Vector3 v3Direction = (v3Objective - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3Direction, Space.World);
                // Move(goPlayer.transform.position);
                navTarget.destination = goPlayer.transform.position;
            }
            else if (sObjective == "Random")
            {
                // Vector3 v3Objective = v3DirectionRand;
                // Vector3 v3Direction = (v3Objective - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3Direction, Space.World);
                // Move(transform.position + v3DirectionRand);
                navTarget.destination = new Vector3(-50f, transform.position.y, -50f);
            }
            else if (sObjective == "SafeZoneTarget")
            {
                // Vector3 v3Objective = new Vector3(goSafeZoneTarget.transform.position.x, 1f, goSafeZoneTarget.transform.position.z);
                // Vector3 v3Direction = (v3Objective - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3Direction, Space.World);
                // Move(new Vector3(goSafeZoneTarget.transform.position.x, transform.position.y, goSafeZoneTarget.transform.position.z));
                navTarget.destination = new Vector3(goSafeZoneTarget.transform.position.x, transform.position.y, goSafeZoneTarget.transform.position.z);
            }
            else
            {
                navTarget.destination = transform.position;
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void Move(Vector3 v3PositionObjective)
    {
        Vector3 v3DirectionMove = (v3PositionObjective - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, v3PositionObjective, fSpeed * Time.deltaTime);

        Vector3 v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(v3DirectionLook);

        Debug.DrawRay(transform.position, v3DirectionMove * 10f, Color.yellow);
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger") && goGameManager.GetComponent<GameManager>().bActive)
        {
            bInPlay = false;
            goGameManager.GetComponent<GameManager>().LevelFailed();
        }
        else if (other.gameObject.CompareTag("SafeZoneTarget") && (sObjective == "Player"))
        {
            Destroy(other);
            goGameManager.GetComponent<GameManager>().LevelCleared();
            sObjective = "SafeZoneTarget";
        }
    }

    // ------------------------------------------------------------------------------------------------

}
