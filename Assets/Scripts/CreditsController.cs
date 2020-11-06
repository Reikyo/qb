using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    private GameManager gameManager;

    private float fSpeed = 2.5f;
    private float fPositionYStop = 55f; // World-space value

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
            gameManager.bActiveScreenButton = true;
        }
    }

    // ------------------------------------------------------------------------------------------------

}
