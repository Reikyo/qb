using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float zLimit = -100f;

    void Update()
    {
        if (transform.position.y < zLimit)
        {
            Destroy(gameObject);
        }
    }
}
