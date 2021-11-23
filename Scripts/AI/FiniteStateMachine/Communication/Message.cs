using UnityEngine;

// Represents the content of the message.
public enum MessageContent
{
    ALARM,
};

// Represents a message object for the FSM communication.
public class Message
{

    // Stores the senders's id.
    public int source;
    // Stores the content of the message.
    public MessageContent messageContent;
    // It is an extra content that provides a 3D vector.
    public Vector3 extraContent;

    // Initializes the message object.
    public Message(int source, MessageContent messageContent, Vector3 extraContent)
    {
        this.source = source;
        this.messageContent = messageContent;
        this.extraContent = extraContent;
    }

}