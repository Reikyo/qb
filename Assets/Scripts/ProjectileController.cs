using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private bool bTriggered = false;
    private float fMetresPerSec = 50f;

    private GameManager gameManager;
    private CubeController cubeController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        cubeController = GameObject.Find("Cube").GetComponent<CubeController>();

        gameManager.SfxclpPlay("sfxclpProjectile");
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
                gameManager.VfxclpPlay("vfxclpWallDestructible", transform.position);
                gameManager.SfxclpPlay("sfxclpWallDestructible");
            }
            else if (other.gameObject.CompareTag("WallSlider"))
            {
                cubeController.SwitchWallsSlider();
                gameManager.SfxclpPlay("sfxclpWallSlider");
            }
            else if (other.gameObject.CompareTag("WallSpinnerSwitch"))
            {
                float fDot = Vector3.Dot(gameObject.transform.forward, other.transform.forward);

                if (fDot > 0)
                {
                    other.transform.parent.GetComponent<WallSpinnerController>().StartRotate(1);
                    gameManager.SfxclpPlay("sfxclpWallSpinner");
                }
                else if (fDot < 0)
                {
                    other.transform.parent.GetComponent<WallSpinnerController>().StartRotate(-1);
                    gameManager.SfxclpPlay("sfxclpWallSpinner");
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
