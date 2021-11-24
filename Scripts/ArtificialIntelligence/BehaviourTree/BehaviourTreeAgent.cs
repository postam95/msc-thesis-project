using UnityEngine;

// Represents a Behavior Tree Agent.
public class BehaviourTreeAgent : MonoBehaviour
{

    // It provides a uniqe id for the character.
    private int characterId;
    // It stores the next id for the agents.
    private static int nextcharacterId = 1;

    // Initiates the character object.
    protected BehaviourTreeAgent()
    {
        this.characterId = nextcharacterId++;
    }

}
