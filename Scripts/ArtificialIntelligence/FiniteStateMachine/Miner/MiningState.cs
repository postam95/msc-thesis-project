using UnityEngine;

// In this state the Miner FSM agent is mining.
public class MiningState : State
{
    // Provides only one instance of this class, thread-safely.
    private static MiningState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    MiningState()
    {
    }

    // Handles the singleton object.
    public static MiningState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MiningState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Miner miner = (Miner)character;
        miner.animator.SetBool("isMining", true);

        miner.StartMiningMineral();

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is entering mining state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Miner miner = (Miner)character;

        if (miner.IsMineralBucketFull())
        {
            miner.StopMiningMineral();
            miner.ChangeState(TakeMineralsHome.Instance);
        }

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is in mining state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Miner miner = (Miner)character;
        miner.animator.SetBool("isMining", false);

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is leaving mining state.");
    }

}
