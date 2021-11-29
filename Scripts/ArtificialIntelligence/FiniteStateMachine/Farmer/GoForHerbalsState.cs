using UnityEngine;

// In this state the Farmer FSM agent goes for herbals.
public class GoForHerbalsState : State
{

    // Provides only one instance of this class, thread-safely.
    private static GoForHerbalsState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    GoForHerbalsState()
    {
    }

    // Handles the singleton object.
    public static GoForHerbalsState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new GoForHerbalsState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Farmer farmer = (Farmer)character;
        farmer.animator.SetBool("isWalking", true);
        PrepareMovementForGoForHerbals(farmer);
        farmer.navMeshAgent.SetDestination(farmer.GetHerbalPosition());

        Debug.Log("FSM Id = " + farmer.CharacterId + " farmer is entering go for herbals state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Farmer farmer = (Farmer)character;

        if (farmer.navMeshAgent.remainingDistance < 2.0f)
        {
            farmer.ChangeState(CollectingState.Instance);
        }

        Debug.Log("FSM Id = " + farmer.CharacterId + " farmer is in go for herbals state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Farmer farmer = (Farmer)character;
        farmer.animator.SetBool("isWalking", false);

        Debug.Log("FSM Id = " + farmer.CharacterId + " farmer is leaving go for herbals state.");
    }

    // Preparing agent movements for chasing
    private void PrepareMovementForGoForHerbals(Farmer farmer)
    {
        farmer.navMeshAgent.speed = 6.0f;
    }

}
