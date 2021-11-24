using UnityEngine;

// In this state the Miner FSM agent goes for mining.
public class GoForMining : State
{
    // Provides only one instance of this class, thread-safely.
    private static GoForMining instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    GoForMining()
    {
    }

    // Handles the singleton object.
    public static GoForMining Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new GoForMining();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Miner miner = (Miner)character;
        miner.animator.SetBool("isWalking", true);
        PrepareMovementForGoForMining(miner);
        miner.navMeshAgent.SetDestination(miner.GetMinePosition());

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is entering go for mining state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Miner miner = (Miner)character;

        if (miner.navMeshAgent.remainingDistance < 2.0f)
        {
            miner.ChangeState(MiningState.Instance);
        }

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is in go for mining state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Miner miner = (Miner)character;
        miner.animator.SetBool("isWalking", false);

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is leaving go for mining state.");
    }

    // Preparing agent movements for going for mining.
    private void PrepareMovementForGoForMining(Miner miner)
    {
        miner.navMeshAgent.speed = 6.0f;
    }

}
