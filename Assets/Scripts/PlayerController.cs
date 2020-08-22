using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool bInPlay = true;
    public bool bActive = true;
    // private float fForce = 1000f;
    private float fSpeed = 10f;
    private Rigidbody rbPlayer;
    private GameObject goEnemy;
    private GameObject goTarget;
    private List<string> slistChangeTargetObjective = new List<string>() {"None", "Random"};
    private int iNumPowerUp = 0;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        goEnemy = GameObject.FindWithTag("Enemy");
        goTarget = GameObject.FindWithTag("Target");
    }

    // Update is called once per frame
    void Update()
    {
        if (bInPlay && bActive)
        {
            float inputHorz = Input.GetAxis("Horizontal");
            float inputVert = Input.GetAxis("Vertical");

            Move(transform.position + ((inputHorz * Vector3.right) + (inputVert * Vector3.forward)).normalized);
        }
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (slistChangeTargetObjective.Contains(goTarget.GetComponent<TargetController>().sObjective))
            {
                goTarget.GetComponent<TargetController>().sObjective = "Player";
                // goTarget.GetComponent<TargetController>().fForce = 500f;
                goTarget.GetComponent<TargetController>().fSpeed = 5f;
                goEnemy.GetComponent<EnemyController>().sObjective = "Target";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bInPlay = false;
            Debug.Log("Game over");
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            iNumPowerUp += 1;
        }
    }
}
