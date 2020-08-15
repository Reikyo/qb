using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private float force = 500.0f;
    private Rigidbody rbTarget;
    private GameObject goPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbTarget = GetComponent<Rigidbody>();
        goPlayer = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (goPlayer)
        {
            Vector3 direction = (goPlayer.transform.position - transform.position).normalized;
            rbTarget.AddForce(force * Time.deltaTime * direction);
        }
    }
}
