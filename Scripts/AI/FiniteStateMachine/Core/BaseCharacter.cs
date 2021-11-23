using UnityEngine;

// This class is the base class of every FSM agent.
public abstract class BaseCharacter : MonoBehaviour
{

    // It provides a uniqe id for the character.
    private int _characterId;
    public int CharacterId { get { return _characterId; } }
    // It stores the next id for the agents.
    private static int nextcharacterId = 0;

    // Initiates the character object.
    protected BaseCharacter()
    {
        this._characterId = nextcharacterId++;
    }

    // Through this method character can change its state.
    public abstract void ChangeState(State newState);

    // Through this method character can change its state to
    // the previous state.
    public abstract void ReturnToPreviousState();

    // This method will receive and process the messages that
    // the agent gets. 
    public virtual void HandleMessage(Message message)
    {
        Debug.Log("This character has no message handling!");
    }

    // This method will send the messages that
    // the agent wants to send.
    public virtual void SendMessage()
    {
        Debug.Log("This character has no message handling");
    }

}
