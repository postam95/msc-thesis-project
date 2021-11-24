using UnityEngine;

// In this state the Guard FSM agent is patrolling.
public class PatrolState : State
{
    // Provides only one instance of this class, thread-safely.
    private static PatrolState instance = null;
    private static readonly object padlock = new object();
    // Current index of the array of the waypoints.
    private int currentWaypointIndex;

    // Initiates the this state.
    PatrolState()
    {
    }

    // Handles the singleton object.
    public static PatrolState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new PatrolState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Guard guard = (Guard) character;
        guard.animator.SetBool("isPatrolling", true);
        PrepareMovementForPatrolling(guard);
        currentWaypointIndex = 0;

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is entering patrolling state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        GameObject[] gaurdsWaypoints = guard.waypoints;

        if (guard.IsHealthLow())
        {
            guard.ChangeState(FleeState.Instance);
        }

        if (gaurdsWaypoints.Length == 0)
        {
            return;
        }

        if (!guard.navMeshAgent.hasPath)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex == gaurdsWaypoints.Length)
                currentWaypointIndex = 0;
        }

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is in patrolling to number " + currentWaypointIndex + " waypoint.");
        guard.navMeshAgent.SetDestination(gaurdsWaypoints[currentWaypointIndex].transform.position);

        if (guard.IsInChasingDistance())
        {
            guard.ChangeState(ChaseState.Instance);
        }
            

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is in patrolling state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        guard.animator.SetBool("isPatrolling", false);

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is leaving patrolling state.");
    }

    // Setting the agent's moving parameters before it enters patrolling state.
    private void PrepareMovementForPatrolling(Guard guard)
    {
        guard.navMeshAgent.speed = 12;
        guard.navMeshAgent.angularSpeed = 200;
        guard.navMeshAgent.acceleration = 5;
    }

}
