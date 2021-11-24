using UnityEngine;

// In this state the Guard FSM agent chases the player.
public class ChaseState : State
{

    // Provides only one instance of this class, thread-safely.
    private static ChaseState instance = null;
    private static readonly object padlock = new object();
    // Defines how close the guard has to go to the waypoints.
    public float chasingAccuracyDistance;

    // Initiates the this state.
    ChaseState()
    {
    }

    // Handles the singleton object.
    public static ChaseState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new ChaseState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        guard.animator.SetBool("isChasing", true);
        PrepareMovementForChasing(guard);

        Debug.Log("FSM Id= " + guard.CharacterId + " guard is entering chasing state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Guard guard = (Guard)character;

        if (guard.IsHealthLow())
        {
            guard.ChangeState(FleeState.Instance);
        }

        if (guard.IsInChasingDistance())
        {
            guard.navMeshAgent.SetDestination(guard.player.transform.position);

            if (guard.IsInShootingDistance())
            {
                guard.ChangeState(AttackState.Instance);
            }
        } else
        {
            guard.ChangeState(PatrolState.Instance);
        }

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is in chasing state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        guard.animator.SetBool("isChasing", false);

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is leaving chasing state.");
    }

    // Preparing agent movements for chasing.
    private void PrepareMovementForChasing(Guard guard)
    {
        guard.navMeshAgent.speed = 24;
        guard.navMeshAgent.angularSpeed = 250;
        guard.navMeshAgent.acceleration = 8;
    }

}