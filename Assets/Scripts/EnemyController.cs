using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool bInPlay = true;
    public bool bActive = true;
    // private float fForce = 500f;
    private float fSpeed = 6f;
    private Rigidbody rbEnemy;
    private GameObject goTarget;
    private GameObject goPlayer;
    public string sObjective;

    // Start is called before the first frame update
    void Start()
    {
        rbEnemy = GetComponent<Rigidbody>();
        goTarget = GameObject.FindWithTag("Target");
        goPlayer = GameObject.FindWithTag("Player");

        if (goTarget)
        {
            sObjective = "Target";
        }
        else if (goPlayer)
        {
            sObjective = "Player";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bInPlay && bActive)
        {
            if ((sObjective == "Target") && goTarget && goTarget.GetComponent<TargetController>().bInPlay)
            {
                // Vector3 v3DirectionMove = (goTarget.transform.position - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3DirectionMove, Space.World);
                Move(goTarget.transform.position);
            }
            else if ((sObjective == "Player") && goPlayer && goPlayer.GetComponent<PlayerController>().bInPlay)
            {
                // Vector3 v3DirectionMove = (goPlayer.transform.position - transform.position).normalized;
                // transform.Translate(fSpeed * Time.deltaTime * v3DirectionMove, Space.World);
                Move(goPlayer.transform.position);
            }
            else
            {
                bActive = false;
            }
        }
    }

    private void Move(Vector3 v3PositionObjective)
    {
        Vector3 v3DirectionMove = (v3PositionObjective - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, v3PositionObjective, fSpeed * Time.deltaTime);

        Vector3 v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(v3DirectionLook);

        Debug.DrawRay(transform.position, v3DirectionMove * 10f, Color.red);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bActive && collision.gameObject.CompareTag("Target"))
        {
            Vector2 v2DirectionRand = Random.insideUnitCircle.normalized * 100f;
            Vector3 v3DirectionRand = new Vector3(v2DirectionRand.x, goTarget.transform.position.y, v2DirectionRand.y);
            goTarget.GetComponent<TargetController>().v3DirectionRand = v3DirectionRand;
            goTarget.GetComponent<TargetController>().sObjective = "Random";
            // goTarget.GetComponent<TargetController>().fForce = 300f;
            goTarget.GetComponent<TargetController>().fSpeed = 2f;
            sObjective = "Player";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bInPlay = false;
        }
    }
}
