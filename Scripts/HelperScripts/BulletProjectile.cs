using UnityEngine;

// Represents the bullet of the Player.
public class BulletProjectile : MonoBehaviour
{

    // Prepares the destroying animation.
    public Transform hitCollider;
    // Responsible for the physics simulation
    // of the object.
    private Rigidbody bulletRigidBody;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update.
    private void Start()
    {
        float speed = 100.0f;
        bulletRigidBody.velocity = transform.forward * speed;
    }

    // Called when the Collider enters the trigger.
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(hitCollider, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
