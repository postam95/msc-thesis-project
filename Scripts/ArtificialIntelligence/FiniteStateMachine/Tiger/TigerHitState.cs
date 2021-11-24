using UnityEngine;

// In this state the Tiger FSM agent tries to hit the Player.
public class TigerHitState : State
{

    // Provides only one instance of this class, thread-safely.
    private static TigerHitState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    TigerHitState()
    {
    }

    // Handles the singleton object.
    public static TigerHitState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TigerHitState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;
        tiger.animator.SetBool("isAttacking", true);
        PrepareMovementForHiding(tiger);

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is entering hitting state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;

        if (!tiger.IsInAttackingDistance())
        {
            tiger.ChangeState(TigerChaseState.Instance);
        }

        tiger.navMeshAgent.SetDestination(tiger.playerPositon);

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is in hitting state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Tiger tiger = (Tiger)character;
        tiger.animator.SetBool("isAttacking", false);

        Debug.Log("FSM Id = " + tiger.CharacterId + " tiger is leaving hitting state.");
    }

    // Setting the agent's moving parameters before it enters hitting state.
    private void PrepareMovementForHiding(Tiger tiger)
    {
        tiger.navMeshAgent.speed = 18;
        tiger.navMeshAgent.angularSpeed = 250;
        tiger.navMeshAgent.acceleration = 8;
    }

}