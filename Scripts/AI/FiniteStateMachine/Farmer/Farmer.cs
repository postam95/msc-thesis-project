using UnityEngine;
using UnityEngine.AI;

// Represents the Farmer FSM agent.
public class Farmer : BaseCharacter
{

    // The Farmer owns a finite state machine.
    private FiniteStateMachine finiteStateMachine;
    // The current world position of the Farmer.
    public Vector3 currentPosition;
    // Defines herbal level.
    public float herbalLevel;
    // Animator component of this character to manipulate
    // its movement.
    public Animator animator;
    // Handles the navigation component.
    public NavMeshAgent navMeshAgent;
    // Handles the navigation component.
    public GameObject farmCheckpoint;
    // Handles the navigation component.
    public GameObject herbalCheckpoint;
    // Show the herbal level during the gameplay.
    public Healthbar herbalLevelBar;
    // Defines the maximum number of herbals can be collected.
    private float bucketSize;
    // Whether herbal collecting is in progress.
    private bool isCollectingInProgess;

    // Start is called before the first frame update.
    void Start()
    {
        // Setting up the Farmer's FSM.
        finiteStateMachine = new FiniteStateMachine(this);
        finiteStateMachine.CurrentState = GoForHerbals.Instance;
        finiteStateMachine.PreviousState = GoForHerbals.Instance;
        GoForHerbals.Instance.BeforeState(this);

        // Initialization.
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        bucketSize = 100.0f;
        StopCollectingHerbal();
        EmptyHerbalBucket();
    }

    // Update is called once per frame.
    void Update()
    {
        UpdateCurrentPosition();
        finiteStateMachine.Update();
        if (isCollectingInProgess)
        {
            CollectHerbal();
        }
        UpdateHealthBar();
    }

    // Collects herbal while increasing herbal level.
    private void CollectHerbal()
    {
        herbalLevel += 0.1f;
    }

    // Starts the herbal collecting process.
    public void StartCollectingHerbal()
    {
        isCollectingInProgess = true;
    }

    // Stops the herbal collecting process.
    public void StopCollectingHerbal()
    {
        isCollectingInProgess = false;
    }

    // Sets zero the herbal level.
    public void EmptyHerbalBucket()
    {
        herbalLevel = 0.0f;
    }

    // Returns true or false whether the herbal
    // level reaches its high limit.
    public bool IsHerbalBucketFull()
    {
        if (herbalLevel > bucketSize)
        {
            return true;
        }
        return false;
    }

    // Returns a vector that means the position of
    // the spot where the farmer can collect herbals.
    public Vector3 GetHerbalPosition()
    {
        return herbalCheckpoint.transform.position;
    }

    // Returns a vector that means the position of
    // the spot where the farmer can put down the
    // collected herbals.
    public Vector3 GetFarmPosition()
    {
        return farmCheckpoint.transform.position;
    }

    // Updates the current position of the guard.
    void UpdateCurrentPosition()
    {
        currentPosition = this.transform.position;
    }

    // Updates the Farmer's healthbar which indicates
    // herbal level in the game.
    void UpdateHealthBar()
    {
        if (herbalLevel > bucketSize)
        {
            herbalLevelBar.UpdateHealth(1.0f);

        }
        else if (0.0f > herbalLevel)
        {
            herbalLevelBar.UpdateHealth(0.0f);
        }
        else
        {
            herbalLevelBar.UpdateHealth(herbalLevel / bucketSize);
        }
    }

    // Changes the Farmer's state.
    public override void ChangeState(State newState)
    {
        finiteStateMachine.ChangeState(newState);
    }

    // Returns to the previous state of the Farmer.
    public override void ReturnToPreviousState()
    {
        finiteStateMachine.ReturnToPreviousState();
    }

}
