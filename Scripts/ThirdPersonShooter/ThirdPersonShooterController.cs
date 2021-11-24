using UnityEngine;
using Cinemachine;
using StarterAssets;

// This class is for receiving the commands from the
// Player and controlling the main character.
public class ThirdPersonShooterController : MonoBehaviour
{

    // This controller based on the ThirdPersonController of
    // the Standard Assets.
    private ThirdPersonController thirdPersonController;
    // This controller uses the on the StarterAssetsInputs of
    // the Starter Assets.
    private StarterAssetsInputs starterAssetsInputs;
    // This camera objects reponsible for the view
    // when the Player is aiming.
    public CinemachineVirtualCamera aimVirtualCamera;
    // This Layermask object helps aiming.
    public LayerMask aimColliderLayerMask = new LayerMask();
    // Helps the Player to aim.
    public Transform debugTransform;
    // The Bullet object is fired by the Player.
    public Transform bullet;
    // The Turret object to place the bullet.
    public Transform turret;
    // Animator component of this character to manipulate
    // its movement.
    public Animator animator;
    // Shows health status in the game.
    public Healthbar healthbar;
    // Stores the tiger object. Important because if the
    // the tiger is too close, it affects the health level
    // of the Player.
    public GameObject tiger;
    // Health level of the Player.
    public float health;
    // The maximum health of the Player.
    private float maximumHealth;
    // Mouse sensitivty in normal mode.
    public float normalSensitivity;
    // Mouse sensitivty in aiming mode.
    public float aimSensitivity;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Initialization.
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        health = 100.0f;
        maximumHealth = 100.0f;
    }

    // Reduces health level when the Player is hit by
    // a bullet fired by NPCs.
    void OnCollisionEnter(Collision colliderObject)
    {
        if (colliderObject.gameObject.tag == "bullet")
        {
            health -= 10.0f;
            healthbar.UpdateHealth(health / maximumHealth);
            // When the health level is too low, the game
            // is over.
            if (health < 1.0f)
            {
                Application.Quit();
            }
        }
    }

    // Update is called once per frame. It processes
    // the Player's commands.
    private void Update()
    {
        // Initialization.
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        // Show aiming helper sphere.
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        // Handles the tiger.
        TigerHit();

        // Closes the game when the Player presses
        // escape button.
        TestGameOver();
        
        // Aiming when the Player presses the right
        // mouse button.
        if (starterAssetsInputs.aim)
        {
            Aim(mouseWorldPosition);
        }
        else
        {
            Standby();
        }

        // Shooting when the Player presses the left
        // mouse button.
        if (starterAssetsInputs.shoot)
        {
            Shoot(mouseWorldPosition);
        }
    }

    // Handles the attack of the tiger.
    private void TigerHit()
    {
        // Handles when the tiger attacks the Player.
        if ((this.transform.position - tiger.transform.position).magnitude < 10.0f)
        {
            health -= 0.1f;
            healthbar.UpdateHealth(health / 100.0f);
        }
    }

    // Aiming when the Player presses the right
    // mouse button.
    private void Aim(Vector3 mouseWorldPosition)
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

    // The base state of the Player.
    private void Standby()
    {
        aimVirtualCamera.gameObject.SetActive(false);
        thirdPersonController.SetSensitivity(normalSensitivity);
        thirdPersonController.SetRotateOnMove(true);
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0.0f, Time.deltaTime * 10.0f));
    }

    // Shooting when the Player presses the left
    // mouse button.
    private void Shoot(Vector3 mouseWorldPosition)
    {
        Vector3 aimDirection = (mouseWorldPosition - turret.position).normalized;
        Instantiate(bullet, turret.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        starterAssetsInputs.shoot = false;
    }

    private void TestGameOver()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

}
