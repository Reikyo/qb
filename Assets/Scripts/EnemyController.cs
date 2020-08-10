using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float force = 500.0f;
    private Rigidbody rbEnemy;
    private GameObject goTarget;

    // Start is called before the first frame update
    void Start()
    {
        rbEnemy = GetComponent<Rigidbody>();
        goTarget = GameObject.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 direction = (goTarget.transform.position - transform.position).normalized;
        rbEnemy.AddForce(force * Time.deltaTime * direction);
    }
}
