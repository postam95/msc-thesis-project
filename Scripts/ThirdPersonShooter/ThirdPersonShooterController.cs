using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ThirdPersonShooterController : MonoBehaviour
{
    public CinemachineVirtualCamera aimVirtualCamera;
    public float normalSensitivity;
    public float aimSensitivity;
    public LayerMask aimColliderLayerMask = new LayerMask();
    public Transform debugTransform;
    public Transform pfBulletProjectile;
    public Transform spawnBulletPosition;
    public Animator animator;
    public Healthbar healthbar;
    public float health;
    public GameObject tiger;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        health = 100.0f;
    }

    void OnCollisionEnter(Collision colliderObject)
    {
        if (colliderObject.gameObject.tag == "bullet")
        {
            health -= 10.0f;
            healthbar.UpdateHealth(health / 100.0f);
            if (health < 1.0f)
            {
                Application.Quit();
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if ((this.transform.position - tiger.transform.position).magnitude < 10.0f)
        {
            health -= 0.1f;
            healthbar.UpdateHealth(health / 100.0f);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }


        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20.0f);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1.0f, Time.deltaTime * 10.0f));
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0.0f, Time.deltaTime * 10.0f));
        }

        if (starterAssetsInputs.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.shoot = false;
        }

    }
}
