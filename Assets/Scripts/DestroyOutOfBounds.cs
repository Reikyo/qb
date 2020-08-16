using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public bool bOnGround = true;
    private float fZLimit = -100f;
    private List<string> slistGameOver = new List<string>() {"Player", "Target"};

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
            if (slistGameOver.Contains(gameObject.tag))
            {
                Debug.Log("Game over");
            }
        }
    }
}
