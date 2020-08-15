using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public bool bOnGround = true;
    private float fZLimit = -100f;

    void Update()
    {
        if (!(transform.position.x >= -25f && transform.position.x <= 25f
        &&    transform.position.y >= 0.9f && transform.position.y <= 1.1f
        &&    transform.position.z >= -25f && transform.position.z <= 25f))
        {
            bOnGround = false;
        }
        if (transform.position.y < fZLimit)
        {
            Destroy(gameObject);
        }
    }
}
