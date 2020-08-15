using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float fForce = 1000f;
    private Rigidbody rbPlayer;
    private GameObject goEnemy;
    private GameObject goTarget;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        goEnemy = GameObject.Find("Enemy");
        goTarget = GameObject.Find("Target");
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
        float inputHorz = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");
        rbPlayer.AddForce(inputHorz * fForce * Time.deltaTime * Vector3.right);
        rbPlayer.AddForce(inputVert * fForce * Time.deltaTime * Vector3.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (goTarget.GetComponent<TargetController>().sObjective != "Player")
            {
                goTarget.GetComponent<TargetController>().sObjective = "Player";
                goTarget.GetComponent<TargetController>().fForce = 500f;
                goEnemy.GetComponent<EnemyController>().sObjective = "Target";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other);
        }
    }
}
