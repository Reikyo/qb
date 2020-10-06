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

    // This requires a Collider component on both objects, and "Is Trigger" enabled on one of them.
    // Also, a RigidBody component must be on at least one of them, it doesn't matter which one.

    // I chose to add a RigidBody component on the projectile only, just to save having unnecessary
    // components on other objects like walls, as we're not currently doing any physics interactions
    // with walls anyway.

    // Note that "OnTriggerEnter" is preferred over "OnCollisionEnter", as the latter only works if
    // "Is Trigger" is disabled on the colliding objects, and in the case of the projectile being
    // emitted from the player, this then results in an interaction of Collider components that results
    // in a physical kick-back to the player. This happens even without a RigidBody component on the
    // projectile, so it's not a true physics interaction (there must still be a RigidBody component
    // on the player though for this effect to occur).
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
            else if (other.gameObject.CompareTag("WallSpinnerSwitch"))
            {
                float fDot = Vector3.Dot(gameObject.transform.forward, other.transform.forward);

                if (fDot > 0)
                {
                    other.transform.parent.GetComponent<WallSpinnerController>().Rotate(1);
                }
                else if (fDot < 0)
                {
                    other.transform.parent.GetComponent<WallSpinnerController>().Rotate(-1);
                }
            }

            if (!other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }

    // ------------------------------------------------------------------------------------------------

}
