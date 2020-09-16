using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    // Start:
    //   pos: 0, -210, 72
    //   rot: -122.5, 0, 0
    // Target:
    //   pos: 0, -25, 0
    //   rot: 0, 0, 0

    private float fGameStartTime = 2f;
    private float fSpeedY;
    private float fSpeedZ;
    private float fAngularSpeed;
    public bool bGameStart = false;
    private int iGameStarted = 0;
    public float fNextLevelStart = 0f;
    private bool bNextLevelStarted = false;
    public GameObject goSpawnManager;
    public GameObject[] goObstacles;

    // Start is called before the first frame update
    void Start()
    {
        fSpeedY = (210f - 25f) / fGameStartTime;
        fSpeedZ = (72f - 0f) / fGameStartTime;
        fAngularSpeed = (237.5f - 0f) / fGameStartTime;
        transform.position = new Vector3(0f, -210f, 72f);
        transform.eulerAngles = new Vector3(237.5f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (bGameStart)
        {
            if (transform.position.y < -25f)
            {
                transform.Translate(0f, fSpeedY * Time.deltaTime, 0f, Space.World);
            }
            else
            {
                iGameStarted += 1;
            }
            if (transform.position.z > 0f)
            {
                transform.Translate(0f, 0f, -fSpeedZ * Time.deltaTime, Space.World);
            }
            else
            {
                iGameStarted += 1;
            }
            if (transform.rotation.x > 0f)
            {
                transform.Rotate(-fAngularSpeed * Time.deltaTime, 0f, 0f, Space.World);
            }
            else
            {
                iGameStarted += 1;
            }
        }
        if (iGameStarted == 3)
        {
            bGameStart = false;
            iGameStarted = 0;
            transform.position = new Vector3(0f, -25f, 0f);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            foreach (GameObject goObstacle in goObstacles)
            {
                goObstacle.SetActive(true);
            }
            goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        }
        if (fNextLevelStart > 0f)
        {
            foreach (GameObject goObstacle in goObstacles)
            {
                goObstacle.SetActive(false);
            }
            if (transform.eulerAngles.z < fNextLevelStart)
            {
                transform.Rotate(0f, 0f, fAngularSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                fNextLevelStart = 0f;
                bNextLevelStarted = true;
            }
        }
        if (bNextLevelStarted)
        {
            bNextLevelStarted = false;
            foreach (GameObject goObstacle in goObstacles)
            {
                goObstacle.SetActive(true);
            }
            goSpawnManager.GetComponent<SpawnManager>().Instantiate();
        }
    }
}
