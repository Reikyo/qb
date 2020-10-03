using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    private float fSpeed = 5f;
    private float fPositionYStop = 50f; // World-space value

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {

    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < fPositionYStop)
        {
            transform.Translate(fSpeed * Time.deltaTime * Vector3.up);
        }
        else
        {
            transform.parent.Find("Button : Restart").gameObject.SetActive(true);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
