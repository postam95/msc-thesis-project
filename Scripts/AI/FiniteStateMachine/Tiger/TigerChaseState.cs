using UnityEngine;

// In this state the Tiger FSM agent chases the Player.
public class TigerChaseState : State
{
    // Provides only one instance of this class, thread-safely.
    private static TigerChaseState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    TigerChaseState()
    {
    }

    // Handles the singleton object.
    public static TigerChaseState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TigerChaseState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;
        tiger.animator.SetBool("isRunning", true);
        PrepareMovementForHiding(tiger);

        // TODO

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is entering chasing state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;

        if (!tiger.IsInChasingDistance())
        {
            tiger.ChangeState(TigerExploreState.Instance);
        }

        if (tiger.IsInAttackingDistance())
        {
            tiger.ChangeState(TigerHitState.Instance);
        }

        tiger.navMeshAgent.SetDestination(tiger.playerPositon);

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is in chasing state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;
        tiger.animator.SetBool("isRunning", false);

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is leaving chasing state.");
    }

    // Setting the agent's moving parameters before it enters patrolling state.
    private void PrepareMovementForHiding(Tiger tiger)
    {
        tiger.navMeshAgent.speed = 18;
        tiger.navMeshAgent.angularSpeed = 250;
        tiger.navMeshAgent.acceleration = 8;
    }

}