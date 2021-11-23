using UnityEngine;

// In this state the Tiger FSM agent hides from the Player.
public class TigerHideState : State
{

    // Provides only one instance of this class, thread-safely.
    private static TigerHideState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    TigerHideState()
    {
    }

    // Handles the singleton object.
    public static TigerHideState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TigerHideState();
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
        PrepareMovementForHiding(tiger);

        tiger.StartFrustration();

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is entering hiding state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;

        if (!tiger.IsPlayerInDangerZone())
        {
            tiger.ChangeState(TigerExploreState.Instance);
        }

        if (tiger.IsAngry())
        {
            tiger.ChangeState(TigerChaseState.Instance);
        }

        Vector3 destinationPosition = CalculatePositionForHiding(tiger);
        tiger.navMeshAgent.SetDestination(destinationPosition);
        Debug.DrawRay(tiger.currentPosition, destinationPosition);

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is in hiding state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;
        tiger.animator.SetBool("isWalking", false);

        tiger.StopFrustration();

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is leaving hiding state.");
    }

    // Setting the agent's moving parameters before it enters patrolling state.
    private void PrepareMovementForHiding(Tiger tiger)
    {
        tiger.navMeshAgent.speed = 12;
        tiger.navMeshAgent.angularSpeed = 200;
        tiger.navMeshAgent.acceleration = 5;
    }

    // Calculates the hiding spot to hide from the Player
    // before the Tiger attacks.
    private Vector3 CalculatePositionForHiding(Tiger tiger)
    {
        // If the Tiger does NOT have any hiding spots.
        if (tiger.hidingPoints.Length == 0)
        {
            return tiger.currentPosition;
        }

        // Initialization.
        float distance = Mathf.Infinity;
        Vector3 hidingSpot = Vector3.zero;
        Vector3 fleeingDirection = Vector3.zero;
        GameObject chosenObjectToHide = tiger.hidingPoints[0];

        // Chosing the closest hiding spot.
        for (int i = 0; i < tiger.hidingPoints.Length; i++)
        {
            Vector3 hideDirection = tiger.hidingPoints[i].transform.position - tiger.playerPositon;
            Vector3 hidePosition = tiger.hidingPoints[i].transform.position + hideDirection.normalized * 10;

            if (Vector3.Distance(tiger.currentPosition, hidePosition) < distance)
            {
                hidingSpot = hidePosition;
                fleeingDirection = hideDirection;
                chosenObjectToHide = tiger.hidingPoints[i];
                distance = Vector3.Distance(tiger.currentPosition, hidePosition);
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

}