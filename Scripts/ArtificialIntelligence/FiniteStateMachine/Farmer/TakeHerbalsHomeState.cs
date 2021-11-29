using UnityEngine;

// In this state the Farmer FSM agent goes for herbals.
public class TakeHerbalsHomeState : State
{

    // Provides only one instance of this class, thread-safely.
    private static TakeHerbalsHomeState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    TakeHerbalsHomeState()
    {
    }

    // Handles the singleton object.
    public static TakeHerbalsHomeState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TakeHerbalsHomeState();
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
        PrepareMovementForTakeHerbalsHome(farmer);
        farmer.navMeshAgent.SetDestination(farmer.GetFarmPosition());

        Debug.Log("FSM Id = " + farmer.CharacterId + " farmer is entering take herbals home state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Farmer farmer = (Farmer)character;

        if (farmer.navMeshAgent.remainingDistance < 2.0f)
        {
            farmer.ChangeState(GoForHerbalsState.Instance);
        }

        Debug.Log("FSM Id = " + farmer.CharacterId + " farmer is in take herbals home state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Farmer farmer = (Farmer)character;
        farmer.animator.SetBool("isWalking", false);

        farmer.EmptyHerbalBucket();

        Debug.Log("FSM Id = " + farmer.CharacterId + " guard is leaving take herbals home state.");
    }

    // Prepars agent movements for taking herbals home.
    private void PrepareMovementForTakeHerbalsHome(Farmer farmer)
    {
        farmer.navMeshAgent.speed = 5.0f;
    }

}