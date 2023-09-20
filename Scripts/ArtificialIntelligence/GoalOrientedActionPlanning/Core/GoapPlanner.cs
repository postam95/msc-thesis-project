using System.Collections.Generic;
using UnityEngine;

// Represents a GOAP Planner. The GOAP Planner is
// the most important part of my GOAP system. It has
// to provide a sequence of actions to achieve the
// goal of the GOAP Agent.
public class GoapPlanner
{

    // Stores the list of the GOAP Actions which
    // can be used by the GOAP Planner.
    public List<GoapAction> actions { get; }
    // Stores a possible plan.
    private GoapAction[] possibleSolution;
    // Stores the best plan,
    private Queue<GoapAction> bestSolution;
    // It stores the cost of the best plan.
    private float bestPlanCost;
    // Whether the planner algorithm find a
    // solution for the.
    private bool hasSolution;
    // Stores the current world states relevant
    // to the caller agent.
    private Dictionary<Conditions, bool> baseState;

    // Initializes the most basic properties of the planner.
    public GoapPlanner(List<GoapAction> actions)
    {
        this.actions = new List<GoapAction>();
        foreach (GoapAction goapAction in actions)
        {
            this.actions.Add(goapAction);
        }
        this.possibleSolution = new GoapAction[actions.Count];
    }

    // Final preparation for the planning.
    public Queue<GoapAction> Plan(GoapGoal goal, Dictionary<Conditions, bool> newState)
    {
        List<GoapAction> usableActions = new List<GoapAction>();
        Dictionary<Conditions, bool> currentState = new Dictionary<Conditions, bool>();
        this.baseState = new Dictionary<Conditions, bool>();
        foreach (KeyValuePair<Conditions, bool> subState in newState)
        {
            this.baseState.Add(subState.Key, subState.Value);
            currentState.Add(subState.Key, subState.Value);
        }

        foreach (GoapAction goapAction in actions)
        {
            usableActions.Add(goapAction);
        }
        bestPlanCost = CalculatePossibleHighestPlanCost(usableActions);
        hasSolution = false;

        Backtrack(0, usableActions, currentState, goal, 0.0f);
        if (!hasSolution)
            return null;
        return bestSolution;
    }

    // We need the best plan which has the lowest cost, so
    // first of all, we calculate the cost of the worst
    // scenario + 1. We need the plus one because if we use
    // all of the actions, the sum of the costs will be
    // equal to the cost of the worst scenario.
    private float CalculatePossibleHighestPlanCost(List<GoapAction> usableActions)
    {
        float sumOfCosts = 0.0f;
        foreach (GoapAction usableAction in usableActions)
        {
            sumOfCosts += usableAction.cost;
        }
        return sumOfCosts + 1;
    }

    // The optimized backtrack algorithm the find possible
    // solutions.
    private void Backtrack(int index, List<GoapAction> usableActions, Dictionary<Conditions, bool> currentState, GoapGoal goal, float cost)
    {
        // If we find a better solution, we store it.
        if (IsGoalAchieved(goal, currentState) && cost < bestPlanCost)
        {
            hasSolution = true;
            bestSolution = new Queue<GoapAction>();
            for (int i = 0; i < index; i++)
            {
                bestSolution.Enqueue(possibleSolution[i]);
            }
            return;
        }

        // Tries to find the next action that suitable
        // for the current state.
        foreach(GoapAction goapAction in usableActions)
        {
            if (goapAction.IsThisActionAchievable(currentState))
            {
                if ((cost + goapAction.cost) < bestPlanCost)
                {
                    possibleSolution[index] = goapAction;

                    List<GoapAction> newUsableActions = RemoveActionFromUsableActions(usableActions, goapAction);
                    Dictionary<Conditions, bool> newCurrentState = RegenerateCurrentState(index);

                    Backtrack(index + 1, newUsableActions, newCurrentState, goal, cost + goapAction.cost);
                }
            }
        }
    }

    // It regenerates the world state according to the
    // starting state and the alreary executed actions.
    private Dictionary<Conditions, bool> RegenerateCurrentState(int index)
    {
        Dictionary<Conditions, bool> newState = new Dictionary<Conditions, bool>();
        foreach(KeyValuePair<Conditions, bool> subState in this.baseState)
        {
            newState.Add(subState.Key, subState.Value);
        }
        for (int i = 0; i <= index; i++)
        {
            foreach (KeyValuePair<Conditions, bool> effect in possibleSolution[i].afterEffects)
            {
                if (!newState.ContainsKey(effect.Key))
                {
                    newState.Add(effect.Key, effect.Value);
                }     
            }
        }
        return newState;
    }

    // Removes an action from the usable actions.
    private List<GoapAction> RemoveActionFromUsableActions(List<GoapAction> usableActions, GoapAction removableAction)
    {
        List<GoapAction> newUsableActions = new List<GoapAction>();

        foreach (GoapAction action in usableActions)
        {
            if (!action.Equals(removableAction))
            {
                newUsableActions.Add(action);
            }
        }
        return newUsableActions;
    }

    // It checks whether the current world state is
    // fulfill the requirements to reach the goal.
    private bool IsGoalAchieved(GoapGoal goal, Dictionary<Conditions, bool> currentState)
    {
        foreach (KeyValuePair<Conditions, bool> goalCondition in goal.desires)
        {
            if (!currentState.ContainsKey(goalCondition.Key))
            {
                return false;
            }
        }
        return true;
    }
}
