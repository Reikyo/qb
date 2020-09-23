using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float fSpeed = 50f;

    private GameObject goCube;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goCube = GameObject.Find("Cube");
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(fSpeed * Time.deltaTime * Vector3.forward);
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().WaitStart();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("WallDestructible"))
        {
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("WallMoveable"))
        {
            goCube.GetComponent<CubeController>().SwitchWallsMoveable();
            // other.gameObject.GetComponent<WallController>().bTargetPositionY = false;
            Destroy(gameObject);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
