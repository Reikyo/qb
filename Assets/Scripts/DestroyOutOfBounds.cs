using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float fXZLimitExist = 50f;
    private float fYLimitExist = -100f;

    void Update()
    {
        if (Math.Abs(transform.position.x) >= fXZLimitExist || Math.Abs(transform.position.z) >= fXZLimitExist || transform.position.y <= fYLimitExist)
        {
            Destroy(gameObject);
        }
    }
}
