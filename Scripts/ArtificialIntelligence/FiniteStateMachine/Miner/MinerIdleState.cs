using UnityEngine;

// In this state the Miner FSM agent is idle.
public class MinerIdleState : State
{
    // Provides only one instance of this class, thread-safely.
    private static MinerIdleState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    MinerIdleState()
    {
    }

    // Handles the singleton object.
    public static MinerIdleState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MinerIdleState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Miner miner = (Miner)character;
        miner.animator.SetBool("isIdle", true);

        PrepareMovementForIdle(miner);

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is entering idle state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Miner miner = (Miner)character;

        if (!miner.IsWarehouseFull())
        {
            miner.ChangeState(GoForMiningState.Instance);
        }

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is in idle state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Miner miner = (Miner)character;
        miner.animator.SetBool("isIdle", false);

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is leaving idle state.");
    }

    // Preparing agent movements for idle state.
    private void PrepareMovementForIdle(Miner miner)
    {
        miner.StopMoving();
    }

}
