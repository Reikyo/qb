using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public bool bOnGround = true;
    // public float fForce = 500f;
    public float fSpeed = 5f;
    private Rigidbody rbTarget;
    private GameObject goPlayer;
    private GameObject goSafeZone;
    public string sObjective = "None";
    public Vector3 v3DirectionRand;

    // Start is called before the first frame update
    void Start()
    {
        rbTarget = GetComponent<Rigidbody>();
        goPlayer = GameObject.Find("Player");
        goSafeZone = GameObject.Find("SafeZone");
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
        if ((sObjective == "Player") && goPlayer && goPlayer.GetComponent<PlayerController>().bOnGround)
        {
            Vector3 v3Objective = goPlayer.transform.position;
            Vector3 v3Direction = (v3Objective - transform.position).normalized;
            // rbTarget.AddForce(fForce * Time.deltaTime * v3Direction);
            transform.Translate(fSpeed * Time.deltaTime * v3Direction);
        }
        else if (sObjective == "Random")
        {
            Vector3 v3Objective = v3DirectionRand;
            Vector3 v3Direction = (v3Objective - transform.position).normalized;
            // rbTarget.AddForce(fForce * Time.deltaTime * v3Direction);
            transform.Translate(fSpeed * Time.deltaTime * v3Direction);
        }
        else if (sObjective == "SafeZone")
        {
            Vector3 v3Objective = new Vector3(goSafeZone.transform.position.x, 1f, goSafeZone.transform.position.z);
            Vector3 v3Direction = (v3Objective - transform.position).normalized;
            // rbTarget.AddForce(fForce * Time.deltaTime * v3Direction);
            transform.Translate(fSpeed * Time.deltaTime * v3Direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OffGroundTrigger"))
        {
            bOnGround = false;
            Debug.Log("Game over");
        }
        else if (other.gameObject.CompareTag("SafeZone") && (sObjective == "Player"))
        {
            Destroy(other);
            sObjective = "SafeZone";
            Debug.Log("Level cleared");
        }
    }
}
