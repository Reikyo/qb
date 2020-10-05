using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private bool bTriggered = false;
    private float fMetresPerSec = 50f;

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
        transform.Translate(fMetresPerSec * Time.deltaTime * Vector3.forward);
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (!bTriggered)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                // We set this flag immediately as otherwise the projectile could be triggered by something else before it is destroyed
                bTriggered = true;
            }

            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyController>().StartWait();
            }
            // else if (other.gameObject.CompareTag("Target"))
            // {
            //
            // }
            // else if (other.gameObject.CompareTag("Wall"))
            // {
            //
            // }
            else if (other.gameObject.CompareTag("WallDestructible"))
            {
                other.gameObject.SetActive(false);
                goGameManager.GetComponent<GameManager>().VfxclpPlay("vfxclpWallDestructible", transform.position);
                goGameManager.GetComponent<GameManager>().SfxclpPlay("sfxclpWallDestructible");
            }
            else if (other.gameObject.CompareTag("WallMoveable"))
            {
                goCube.GetComponent<CubeController>().SwitchWallsMoveable();
                goGameManager.GetComponent<GameManager>().SfxclpPlay("sfxclpWallMoveable");
            }
            else if (other.gameObject.CompareTag("WallSpinnerTriggerClockwise"))
            {
                // Debug.Log("Clockwise");
                other.transform.parent.GetComponent<WallSpinnerController>().Rotate(1);
            }
            else if (other.gameObject.CompareTag("WallSpinnerTriggerAnticlockwise"))
            {
                // Debug.Log("Antilockwise");
                other.transform.parent.GetComponent<WallSpinnerController>().Rotate(-1);
            }

            if (!other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
