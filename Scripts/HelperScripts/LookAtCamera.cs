using UnityEngine;

// This script is important to show the canvas
// object for the main camera. Thanks to this
// object, the Player always sees all the canvases
// from the right angle.
public class LookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }

}
