using UnityEngine;

// This script reponsible for destroying
// objects like bullets.
public class DestroyMe : MonoBehaviour
{

	// When the object collides, right after
	// destroys too. It is a good practice
	// for bullets.
	void OnCollisionEnter(Collision col)
	{
		Destroy(this.gameObject);
	}

}
