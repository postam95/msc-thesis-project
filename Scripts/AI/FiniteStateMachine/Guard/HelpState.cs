using UnityEngine;

// In this state the Guard FSM agent helps the other guards.
public class HelpState : State
{

    // Provides only one instance of this class, thread-safely.
    private static HelpState instance = null;
    private static readonly object padlock = new object();
    // When the Guard enters into this state, it immediately
    // goes to this position to help out the others.
    public Vector3 targetPosition;

    // Initiates the this state.
    HelpState()
    {
    }

    // Handles the singleton object.
    public static HelpState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new HelpState();
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

        guard.navMeshAgent.SetDestination(targetPosition);

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is entering helping state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Guard guard = (Guard)character;

        if (guard.IsInShootingDistance())
        {
            guard.ChangeState(AttackState.Instance);
            return;
        }
        if (guard.IsInChasingDistance())
        {
            guard.ChangeState(ChaseState.Instance);
            return;
        }

        if (!guard.navMeshAgent.hasPath)
        {

            guard.ChangeState(PatrolState.Instance);
        }

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is in helping state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Guard guard = (Guard)character;
        guard.animator.SetBool("isChasing", false);

        Debug.Log("FSM Id = " + guard.CharacterId + " guard is leaving helping state.");
    }

}