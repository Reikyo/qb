using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetController : MonoBehaviour
{
    private GameManager gameManager;

    public bool bInPlay = true;
    public bool bActive = false;
    public bool bSafe;
    public string sNameExchangerEngagedByTarget = "";
    private string sNameTranslatorEngagedByTarget = "";
    // public float fForce = 500f;
    private float fSpeed;
    private float fSpeedObjectivePlayer = 10f;
    private float fSpeedObjectiveRandom = 5f;
    private float fDistPlayerStop = 3f;
    private Rigidbody rbTarget;
    private NavMeshAgent navTarget;
    private Material matTarget;
    private GameObject goPlayer;
    private GameObject[] goArrEnemy;
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

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        rbTarget = GetComponent<Rigidbody>();
        navTarget = GetComponent<NavMeshAgent>();
        matTarget = GetComponent<Renderer>().material;
        goPlayer = GameObject.FindWithTag("Player");
        goArrEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        goSafeZoneTarget = GameObject.FindWithTag("SafeZoneTarget");
        bSafe = !goSafeZoneTarget;
        fObjectiveRandomEmissionAngFreq = 2f * Mathf.PI * fObjectiveRandomEmissionFreq;
        navTarget.speed = fSpeedObjectivePlayer;
        fSpeed = fSpeedObjectiveRandom;
        SetDirectionRandom();
    }

    // ------------------------------------------------------------------------------------------------

    void FixedUpdate()
    {
        // Game Manager bInPlay check intentionally left out here, so that target can still move further
        // into the safe zone even after that action triggers the level clearing
        if (    bInPlay
            &&  bActive )
        {
            if (    (sObjective == "Player")
                &&  goPlayer
                &&  goPlayer.GetComponent<PlayerController>().bInPlay )
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
        // fSpeed = fSpeedObjectivePlayer;
        navTarget.enabled = true;
        matTarget.DisableKeyword("_EMISSION");
        transform.Find("Trail").gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------------------------------

    public void StartObjectiveRandom()
    {
        bActive = true;
        sObjective = "Random";
        // fSpeed = fSpeedObjectiveRandom;
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
        else if (   collision.gameObject.CompareTag("Translator")
                &&  (transform.position.x >= (collision.gameObject.transform.position.x - 2.5f))
                &&  (transform.position.x <= (collision.gameObject.transform.position.x + 2.5f))
                &&  (transform.position.z >= (collision.gameObject.transform.position.z - 2.5f))
                &&  (transform.position.z <= (collision.gameObject.transform.position.z + 2.5f)) )
        {
            transform.parent = collision.gameObject.transform;
            sNameTranslatorEngagedByTarget = collision.gameObject.name; // We keep track of this as otherwise timing means we could exit from one translator and enter another, but have the exit trigger fulfilled second, so no parent then assigned
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnCollisionExit(Collision collision)
    {
        if (    collision.gameObject.CompareTag("Translator")
            &&  (collision.gameObject.name == sNameTranslatorEngagedByTarget) )
        {
            transform.parent = null;
            sNameTranslatorEngagedByTarget = "";
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bInPlay = false;
            navTarget.enabled = false;
            gameManager.LevelFailed("look after your buddy!");
        }
        else if (other.gameObject.CompareTag("Exchanger"))
        {
            other.gameObject.GetComponent<ExchangerController>().bEngagedByTarget = true;
            sNameExchangerEngagedByTarget = other.gameObject.name;
        }
        else if (   other.gameObject.CompareTag("SafeZoneTarget")
                &&  (sObjective == "Player")
                &&  !bSafe )
        {
            bSafe = true;
            sObjective = "SafeZoneTarget";
            if (goPlayer.GetComponent<PlayerController>().bSafe)
            {
                gameManager.LevelCleared();
            }
            else
            {
                gameManager.SfxclpPlay("sfxclpLevelClearedPartial");
                foreach (GameObject goEnemy in goArrEnemy)
                {
                    if (goEnemy)
                    {
                        goEnemy.GetComponent<EnemyController>().sObjective = "Player";
                    }
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Exchanger"))
        {
            other.gameObject.GetComponent<ExchangerController>().bEngagedByTarget = false;
            sNameExchangerEngagedByTarget = "";
        }
    }

    // ------------------------------------------------------------------------------------------------

}
