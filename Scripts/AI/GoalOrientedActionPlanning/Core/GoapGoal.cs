using System.Collections.Generic;

// Represents a GOAP Goal. The goal has a set of
// conditions that are the desires of the goal.
// If the world state reaches all these desires
// then the goal is achieved.
public class GoapGoal
{

    // Stores the desires of the goal.
    public Dictionary<Conditions, bool> desires;

    // Initializes the goal with conditions.
    public GoapGoal(Conditions condition, bool state)
    {
        desires = new Dictionary<Conditions, bool>();
        desires.Add(condition, state);
    }

}