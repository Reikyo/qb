using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public float fForce = 500f;
    private Rigidbody rbTarget;
    private GameObject goPlayer;
    public string sObjective = "Player";
    public Vector3 v3DirectionRand;

    // Start is called before the first frame update
    void Start()
    {
        rbTarget = GetComponent<Rigidbody>();
        goPlayer = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<DestroyOutOfBounds>().bOnGround)
        {
            Move();
        }
    }

    private void Move()
    {
        if ((sObjective == "Player") && goPlayer && goPlayer.GetComponent<DestroyOutOfBounds>().bOnGround)
        {
            Vector3 v3Direction = (goPlayer.transform.position - transform.position).normalized;
            rbTarget.AddForce(fForce * Time.deltaTime * v3Direction);
        }
        else if (sObjective == "Self destruct")
        {
            Vector3 v3Direction = (v3DirectionRand - transform.position).normalized;
            rbTarget.AddForce(fForce * Time.deltaTime * v3Direction);
        }
    }
}
