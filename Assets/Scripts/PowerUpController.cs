using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private float fDegreesPerSecond = 90f;
    private float fDegreesPerFrame;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        fDegreesPerFrame = fDegreesPerSecond * Time.deltaTime;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, fDegreesPerFrame, 0f, Space.World);
    }

    // ------------------------------------------------------------------------------------------------

}
