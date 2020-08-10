using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float force = 1000.0f;
    private Rigidbody rbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorz = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");

        rbPlayer.AddForce(inputHorz * force * Time.deltaTime * Vector3.right);
        rbPlayer.AddForce(inputVert * force * Time.deltaTime * Vector3.forward);
    }
}
