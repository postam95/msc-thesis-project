using UnityEngine;
using UnityEngine.AI;

// Represents the Miner FSM agent.
public class Miner : BaseCharacter
{

    // The Miner owns a finite state machine.
    private FiniteStateMachine finiteStateMachine;
    // The current world position of the Miner.
    public Vector3 currentPosition;
    // Defines mineral level.
    public float mineralLevel;
    // Whether mining is in progress.
    private bool isMiningInProgess;
    // Animator component of this character to manipulate
    // its movement.
    public Animator animator;
    // The Miner takes the minerals to the warehouse.
    // The Inventory stores information about the state
    // of the warehouse.
    public Inventory warehouseInventory;
    // Handles the navigation component.
    public NavMeshAgent navMeshAgent;
    // The Miner takes the minerals to the warehouse. It
    // stores the spot where it is.
    public GameObject warehouseCheckpoint;
    // The Miner works at the mine. It stores the position
    // of the mine.
    public GameObject mineCheckpoint;
    // Show the mineral level in the game.
    public Healthbar mineralLevelBar;
    // Defines the maximum number of minerals can be collected.
    private float bucketSize;

    // Start is called before the first frame update.
    void Start()
    {
        // Setting up the Miner's FSM.
        finiteStateMachine = new FiniteStateMachine(this);
        finiteStateMachine.CurrentState = GoForMiningState.Instance;
        finiteStateMachine.PreviousState = GoForMiningState.Instance;
        GoForMiningState.Instance.BeforeState(this);

        // Initialization.
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        bucketSize = 150.0f;
        StopMiningMineral();
        EmptyMineralBucket();
    }

    // Update is called once per frame.
    void Update()
    {
        UpdateCurrentPosition();
        finiteStateMachine.Update();
        if (isMiningInProgess)
        {
            CollectMineral();
        }
        UpdateHealthBar();
    }

    // Collects mineral while increasing mineral level.
    private void CollectMineral()
    {
        mineralLevel += 0.1f;
    }

    // Starts the mineral mining process.
    public void StartMiningMineral()
    {
        isMiningInProgess = true;
    }

    // Stops the mineral mining process.
    public void StopMiningMineral()
    {
        isMiningInProgess = false;
    }

    // Sets zero the mineral level.
    private void EmptyMineralBucket()
    {
        mineralLevel = 0.0f;
    }

    // Takes the mineral to the warehouse.
    // Sets zero the mineral level and
    // increases the mineral level of
    // the warehouse.
    public void PutMineralsDown()
    {
        IncreaseMineralLevelOfWarehouse();
        EmptyMineralBucket();
    }

    // Returns true or false according to the level
    // of the mineral in the warehouse is full.
    public bool IsWarehouseFull()
    {
        WarehouseInventory inventory = (WarehouseInventory)this.warehouseInventory;
        if (inventory.IsWarehouseFull())
        {
            return true;
        }
        return false;
    }

    // Returns true or false according to the level
    // of the mineral in the warehouse is nearly full.
    public bool IsWarehouseNearlyFull()
    {
        WarehouseInventory inventory = (WarehouseInventory)this.warehouseInventory;
        if (inventory.IsWarehouseNearlyFull())
        {
            return true;
        }
        return false;
    }

    // Increases the mineral level by one.
    private void IncreaseMineralLevelOfWarehouse()
    {
        WarehouseInventory inventory = (WarehouseInventory) this.warehouseInventory;
        inventory.IncreaseMineralLevel(1);
    }

    // Returns true or false whether the mineral
    // bucket is full.
    public bool IsMineralBucketFull()
    {
        if (mineralLevel > bucketSize)
        {
            return true;
        }
        return false;
    }

    // Returns the position of the mine.
    public Vector3 GetMinePosition()
    {
        return mineCheckpoint.transform.position;
    }

    // Returns the position of the warehouse.
    public Vector3 GetWarehousePosition()
    {
        return warehouseCheckpoint.transform.position;
    }

    // Updates the current position of the Farmer while its moving.
    void UpdateCurrentPosition()
    {
        currentPosition = this.transform.position;
    }

    // Updates the Farmer's healthbar.
    void UpdateHealthBar()
    {
        if (mineralLevel > bucketSize)
        {
            mineralLevelBar.UpdateHealth(1.0f);

        }
        else if (0.0f > mineralLevel)
        {
            mineralLevelBar.UpdateHealth(0.0f);
        }
        else
        {
            mineralLevelBar.UpdateHealth(mineralLevel / bucketSize);
        }
    }

    // Stops the Miner if it's moving.
    public void StopMoving()
    {
        if (this.navMeshAgent.hasPath)
        {
            this.navMeshAgent.ResetPath();
        }
    }

    // Changes the Miner's state.
    public override void ChangeState(State newState)
    {
        finiteStateMachine.ChangeState(newState);
    }

    // Returns to the previous state of the Miner.
    public override void ReturnToPreviousState()
    {
        finiteStateMachine.ReturnToPreviousState();
    }

}
