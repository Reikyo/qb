using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float fSpeed = 50f;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {

    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
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
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
