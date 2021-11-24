using UnityEngine;

// In this state the Miner FSM agent is taking minerals home.
public class TakeMineralsHome : State
{
    // Provides only one instance of this class, thread-safely.
    private static TakeMineralsHome instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    TakeMineralsHome()
    {
    }

    // Handles the singleton object.
    public static TakeMineralsHome Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TakeMineralsHome();
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
        PrepareMovementForTakeMineralsHome(miner);
        miner.navMeshAgent.SetDestination(miner.GetWarehousePosition());

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is entering take minerals to the warehouse state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Miner miner = (Miner)character;

        if (miner.navMeshAgent.remainingDistance < 2.0f)
        {
            if (miner.IsWarehouseFull() || miner.IsWarehouseNearlyFull())
            {
                miner.ChangeState(MinerIdleState.Instance);
            }
            else
            {
                miner.ChangeState(GoForMining.Instance);
            }
        }

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is in take minerals to the warehouse state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Miner miner = (Miner)character;
        miner.animator.SetBool("isWalking", false);

        miner.PutMineralsDown();

        Debug.Log("FSM Id = " + miner.CharacterId + " miner is leaving take minerals to the warehouse state.");
    }

    // Preparing agent movements for taking minerals home.
    private void PrepareMovementForTakeMineralsHome(Miner miner)
    {
        miner.navMeshAgent.speed = 5.0f;
    }
}