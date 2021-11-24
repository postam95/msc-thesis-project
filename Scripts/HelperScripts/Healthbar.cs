using UnityEngine;
using UnityEngine.UI;

// The healthbar is an important component
// of the gameplay. It shows the current
// state of the character right above of it.
public class Healthbar : MonoBehaviour
{
    // The image that represents the
    // healthbar like a progress bar.
    public Image healthbar;

    // Updates the healthbar according to
    // the level.
    public void UpdateHealth(float fraction)
    {
        healthbar.fillAmount = fraction;
    }
}
