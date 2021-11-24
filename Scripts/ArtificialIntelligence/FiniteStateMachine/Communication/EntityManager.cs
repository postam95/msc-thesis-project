using System.Collections.Generic;

// Represents a manager class that tracks FSM agents to provide
// them communication abilities.
public class EntityManager
{

    // Provides only one static instance of this class, thread-safely.
    private static EntityManager instance = null;
    private static readonly object padlock = new object();
    // It stores agents by its id so the EntityManager can access
    // them directly.
    private Dictionary<int, BaseCharacter> FsmCharacters;

    // Initializes the object to provide an empty database.
    public EntityManager()
    {
        FsmCharacters = new Dictionary<int, BaseCharacter>();
    }

    // Handles the singleton object.
    public static EntityManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new EntityManager();
                }
                return instance;
            }
        }
    }

    // Registers FSM characters.
    public void AddFsmCharacter(BaseCharacter baseCharacter)
    {
        FsmCharacters[baseCharacter.CharacterId] = baseCharacter;
    }

    // Returns a character by its id.
    public BaseCharacter GetCharacterById(int id)
    {
        return FsmCharacters[id];
    }

    // Returns characters except one that identified by its id.
    public List<BaseCharacter> GetAllCharactersExceptOneById(int id)
    {
        List<BaseCharacter> characters = new List<BaseCharacter>();
        foreach (KeyValuePair<int, BaseCharacter> entry in FsmCharacters)
        {
            if (entry.Key != id)
            {
                characters.Add(entry.Value);
            }
        }
        return characters;
    }

    // Removes a character from the database of this object.
    public bool RemoveCharacter(BaseCharacter baseCharacter)
    {
        if (FsmCharacters.ContainsKey(baseCharacter.CharacterId))
        {
            FsmCharacters.Remove(baseCharacter.CharacterId);
            return true;
        }
        return false;
    }

}
