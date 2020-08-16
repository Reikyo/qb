using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool bOnGround = true;
    // private float fForce = 500f;
    private float fSpeed = 6f;
    private Rigidbody rbEnemy;
    private GameObject goPlayer;
    private GameObject goTarget;
    public string sObjective = "Target";

    // Start is called before the first frame update
    void Start()
    {
        rbEnemy = GetComponent<Rigidbody>();
        goPlayer = GameObject.Find("Player");
        goTarget = GameObject.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        if (bOnGround)
        {
            Move();
        }
    }

    private void Move()
    {
        if ((sObjective == "Target") && goTarget && goTarget.GetComponent<TargetController>().bOnGround)
        {
            Vector3 v3Direction = (goTarget.transform.position - transform.position).normalized;
            // rbEnemy.AddForce(fForce * Time.deltaTime * v3Direction);
            transform.Translate(fSpeed * Time.deltaTime * v3Direction);
        }
        else if ((sObjective == "Player") && goPlayer && goPlayer.GetComponent<PlayerController>().bOnGround)
        {
            Vector3 v3Direction = (goPlayer.transform.position - transform.position).normalized;
            // rbEnemy.AddForce(fForce * Time.deltaTime * v3Direction);
            transform.Translate(fSpeed * Time.deltaTime * v3Direction);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Vector2 v2DirectionRand = Random.insideUnitCircle.normalized * 100f;
            Vector3 v3DirectionRand = new Vector3(v2DirectionRand.x, 1f, v2DirectionRand.y);
            goTarget.GetComponent<TargetController>().v3DirectionRand = v3DirectionRand;
            goTarget.GetComponent<TargetController>().sObjective = "Random";
            // goTarget.GetComponent<TargetController>().fForce = 300f;
            goTarget.GetComponent<TargetController>().fSpeed = 3f;
            sObjective = "Player";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bOnGround = false;
        }
    }
}
