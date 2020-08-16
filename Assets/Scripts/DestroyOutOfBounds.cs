using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float fZLimit = -100f;

    void Update()
    {
        if (transform.position.y < fZLimit)
        {
            Destroy(gameObject);
        }
    }
}
