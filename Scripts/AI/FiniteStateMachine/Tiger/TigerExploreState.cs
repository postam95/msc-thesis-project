using UnityEngine;

// In this state the Tiger FSM agent explores the forest.
public class TigerExploreState : State
{

    // Provides only one instance of this class, thread-safely.
    private static TigerExploreState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    TigerExploreState()
    {
    }

    // Handles the singleton object.
    public static TigerExploreState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TigerExploreState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;
        tiger.animator.SetBool("isWalking", true);
        tiger.CalmDown();
        PrepareMovementForExploring(tiger);

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is entering exploring state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;

        if (tiger.IsPlayerInDangerZone())
        {
            tiger.ChangeState(TigerHideState.Instance);
        }

        if (tiger.waypoints.Length == 0)
        {
            return;
        }

        if (!tiger.navMeshAgent.hasPath)
        {
            int currentWaypointIndex = PickRandomWaypoint(tiger);
            tiger.navMeshAgent.SetDestination(tiger.waypoints[currentWaypointIndex].transform.position);

            Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is in exploring to number " + currentWaypointIndex + " waypoint.");
        }

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is in exploring state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;
        tiger.animator.SetBool("isWalking", false);

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is leaving exploring state.");
    }

    // Setting the agent's moving parameters before it enters exploring state.
    private void PrepareMovementForExploring(Tiger tiger)
    {
        tiger.navMeshAgent.speed = 12;
        tiger.navMeshAgent.angularSpeed = 200;
        tiger.navMeshAgent.acceleration = 5;
    }

    // Picks a random waypoint to explore the forest
    // in an unexpected way.
    private int PickRandomWaypoint(Tiger tiger)
    {
        return Random.Range(0, tiger.waypoints.Length - 1);
    }

}
