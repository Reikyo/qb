using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public bool bInPlay = true;
    public bool bActive = true;
    // public float fForce = 500f;
    public float fSpeed = 5f;
    private Rigidbody rbTarget;
    private GameObject goPlayer;
    private GameObject goEnemy;
    private GameObject goSafeZone;
    public string sObjective;
    public Vector3 v3DirectionRand;

    // Start is called before the first frame update
    void Start()
    {
        rbTarget = GetComponent<Rigidbody>();
        goPlayer = GameObject.FindWithTag("Player");
        goEnemy = GameObject.FindWithTag("Enemy");
        goSafeZone = GameObject.FindWithTag("SafeZone");
        sObjective = "None";
    }

    // Update is called once per frame
    void Update()
    {
        if (bInPlay && bActive)
        {
            if ((sObjective == "Player") && goPlayer && goPlayer.GetComponent<PlayerController>().bInPlay)
            {
                // Vector3 v3Objective = goPlayer.transform.position;
                // Vector3 v3Direction = (v3Objective - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3Direction, Space.World);
                Move(goPlayer.transform.position);
            }
            else if (sObjective == "Random")
            {
                // Vector3 v3Objective = v3DirectionRand;
                // Vector3 v3Direction = (v3Objective - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3Direction, Space.World);
                Move(transform.position + v3DirectionRand);
            }
            else if (sObjective == "SafeZone")
            {
                // Vector3 v3Objective = new Vector3(goSafeZone.transform.position.x, 1f, goSafeZone.transform.position.z);
                // Vector3 v3Direction = (v3Objective - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3Direction, Space.World);
                Move(new Vector3(goSafeZone.transform.position.x, transform.position.y, goSafeZone.transform.position.z));
            }
        }
    }

    private void Move(Vector3 v3PositionObjective)
    {
        Vector3 v3DirectionMove = (v3PositionObjective - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, v3PositionObjective, fSpeed * Time.deltaTime);

        Vector3 v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(v3DirectionLook);

        Debug.DrawRay(transform.position, v3DirectionMove * 10f, Color.yellow);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bInPlay = false;
            Debug.Log("Game over");
        }
        else if (other.gameObject.CompareTag("SafeZone") && (sObjective == "Player"))
        {
            Destroy(other);
            goEnemy.GetComponent<EnemyController>().bActive = false;
            sObjective = "SafeZone";
            Debug.Log("Level cleared");
        }
    }
}
