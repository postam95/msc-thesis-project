using UnityEngine;

// In this state the Guard FSM agent attacks the player.
public class AttackState : State
{

    // Provides only one instance of this class, thread-safely.
    private static AttackState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    AttackState()
    {
    }

    // Handles the singleton object.
    public static AttackState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new AttackState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        guard.animator.SetBool("isAttacking", true);

        guard.SendMessage();
        guard.StartFiring();

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is entering attacking state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Guard guard = (Guard)character;

        if (guard.IsHealthLow())
        {
            guard.ChangeState(FleeState.Instance);
        }

        PrepareMovementForAttacking(guard);
        guard.LookAtTarget();

        if (!guard.IsInShootingDistance())
        {
            guard.ChangeState(ChaseState.Instance);
        }

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is in attacking state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        guard.StopFiring();
        guard.animator.SetBool("isAttacking", false);

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is leaving attacking state.");
    }

    // Preparing agent movements for attacking.
    private void PrepareMovementForAttacking(Guard guard)
    {
        guard.navMeshAgent.ResetPath();
    }

}
