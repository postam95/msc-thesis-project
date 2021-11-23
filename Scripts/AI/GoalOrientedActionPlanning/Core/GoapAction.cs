using System.Collections.Generic;
using UnityEngine;

// Represents a GOAP Action. The sequence of
// GOAP Actions will define a plan that
public abstract class GoapAction
{

    // Stores the name of the GOAP Action.
    public string nameOfTheAction = "TheActionHasNoNameGiven";
    // The cost of the execution.
    public float cost = 1.0f;
    // Every GOAP Action has a position
    // in the virtual world where can be
    // executed.
    public GameObject position;
    // This GOAP Agent owns this GOAP
    // Action object.
    public GoapAgent owner;
    // The conditions that needs for this
    // Action to be executed.
    public Dictionary<Conditions, bool> preConditions;
    // Store the effect of this GOAP Action
    // execution.
    public Dictionary<Conditions, bool> afterEffects;
    // Whether the GOAP Action is running.
    public bool running = false;

    // Initializes the GOAP Action.
    public GoapAction()
    {
        preConditions = new Dictionary<Conditions, bool>();
        afterEffects = new Dictionary<Conditions, bool>();
    }

    // Whether this action achiavable from the
    // given state.
    public bool IsThisActionAchievable(Dictionary<Conditions, bool> conditions)
    {
        if (preConditions.Count == 0)
        {
            return true;
        }
        foreach (KeyValuePair<Conditions, bool> p in preConditions)
        {
            if (!conditions.ContainsKey(p.Key))
                return false;
        }
        return true;
    }

    // Runs before the action ONCE.
    public abstract void BeforeAction();
    // Runs in the action ONCE.
    public abstract void ExecuteAction();
    // Runs after the action ONCE.
    public abstract void AfterAction();
    // Prepares movement settings for the GOAP Agent.
    protected abstract void PrepareMovementForThisAction();

}
