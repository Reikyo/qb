using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float fSpeed = 50f;

    private GameObject goGameManager;
    private GameObject goCube;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goGameManager = GameObject.Find("Game Manager");
        goCube = GameObject.Find("Cube");

        goGameManager.GetComponent<GameManager>().SfxclpPlay("sfxclpProjectile");
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        transform.Translate(fSpeed * Time.deltaTime * Vector3.forward);
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().StartWait();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("WallDestructible"))
        {
            other.gameObject.SetActive(false);
            goGameManager.GetComponent<GameManager>().VfxclpPlay("vfxclpWallDestructible", transform.position);
            goGameManager.GetComponent<GameManager>().SfxclpPlay("sfxclpWallDestructible");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("WallMoveable"))
        {
            goCube.GetComponent<CubeController>().SwitchWallsMoveable();
            goGameManager.GetComponent<GameManager>().SfxclpPlay("sfxclpWallMoveable");
            Destroy(gameObject);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
