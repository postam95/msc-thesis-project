using System.Collections.Generic;
using UnityEngine;

// It provides a centre for the messages that FSM agents
// can send to each other. The FSM agent uses this class
// to reach other agents.
public class MessageDispatcher
{

    // Provides only one instance of this class, thread-safely.
    private static MessageDispatcher instance = null;
    private static readonly object padlock = new object();

    // An empty contructor to create an object from this class.
    public MessageDispatcher()
    {
    }

    // Handles the singleton object.
    public static MessageDispatcher Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MessageDispatcher();
                }
                return instance;
            }
        }
    }

    // Sends the message to every FSM agent, except one that
    // is the sender of the message.
    public void Broadcast (int source, MessageContent content, Vector3 extraContent)
    {
        List<BaseCharacter> destinations = EntityManager.Instance.GetAllCharactersExceptOneById(source);
        Message message = new Message(source, content, extraContent);
        foreach (BaseCharacter destination in destinations)
        {
            SendMessage(destination, message);
        }
    }

    // Sends a message to one FSM agent.
    private void SendMessage(BaseCharacter destination, Message message)
    {
        destination.HandleMessage(message);
    }

}
