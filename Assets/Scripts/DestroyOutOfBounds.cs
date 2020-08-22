using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float fXZLimit = 50f;
    private float fYLimit = -100f;

    void Update()
    {
        if (Math.Abs(transform.position.x) >= fXZLimit || Math.Abs(transform.position.z) >= fXZLimit || transform.position.y <= fYLimit)
        {
            Destroy(gameObject);
        }
    }
}
