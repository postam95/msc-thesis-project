using UnityEngine;

// In this state the Guard FSM agent flees from the player.
public class FleeState : State
{

    // Provides only one instance of this class, thread-safely.
    private static FleeState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    FleeState()
    {
    }

    // Handles the singleton object.
    public static FleeState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new FleeState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        guard.animator.SetBool("isFleeing", true);

        PrepareMovementForFleeing(guard);

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is entering fleeing state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        Vector3 destination = CalculatePositionForHiding(guard);

        guard.navMeshAgent.SetDestination(destination);

        if (guard.IsHealthHighEnough())
        {
           guard.ReturnToPreviousState();
        }

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is in fleeing state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        guard.animator.SetBool("isFleeing", false);

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is leaving fleeing state.");
    }

    // Calculates the hiding spot to hide from the Player
    // while the health is low.
    private Vector3 CalculatePositionForHiding(Guard guard)
    {
        // If the Guard does NOT have any hiding spots.
        if (guard.hidingPoints.Length == 0)
        {
            return guard.currentPosition;
        }

        // Initialization.
        float distance = Mathf.Infinity;
        Vector3 hidingSpot = Vector3.zero;
        Vector3 fleeingDirection = Vector3.zero;
        GameObject chosenObjectToHide = guard.hidingPoints[0];

        // Chosing the closest hiding spot.
        for (int i = 0; i < guard.hidingPoints.Length; i++)
        {
            Vector3 hideDirection = guard.hidingPoints[i].transform.position - guard.playerPositon;
            Vector3 hidePosition = guard.hidingPoints[i].transform.position + hideDirection.normalized * 10;

            if (Vector3.Distance(guard.currentPosition, hidePosition) < distance)
            {
                hidingSpot = hidePosition;
                fleeingDirection = hideDirection;
                chosenObjectToHide = guard.hidingPoints[i];
                distance = Vector3.Distance(guard.currentPosition, hidePosition);
            }
        }

        // Calculates the point where the ray from the Player to the
        // hiding spot hits the collider of the hiding spot.
        Collider hidingSpotCollider = chosenObjectToHide.GetComponent<Collider>();
        Ray backRay = new Ray(hidingSpot, -fleeingDirection.normalized);
        RaycastHit thePointWhereTheRayHitsTheCollider;
        float lengthsOfTheRay = 100.0f;
        hidingSpotCollider.Raycast(backRay, out thePointWhereTheRayHitsTheCollider, lengthsOfTheRay);

        return thePointWhereTheRayHitsTheCollider.point + fleeingDirection.normalized * 2;
    }

    // Preparing agent movements for fleeing.
    private void PrepareMovementForFleeing(Guard guard)
    {
        guard.navMeshAgent.speed = 21;
        guard.navMeshAgent.angularSpeed = 230;
        guard.navMeshAgent.acceleration = 6;
    }

}