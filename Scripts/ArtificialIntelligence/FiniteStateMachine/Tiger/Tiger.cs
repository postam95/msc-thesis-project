using UnityEngine;
using UnityEngine.AI;

// Represents the Tiger FSM agent.
public class Tiger : BaseCharacter
{

    // The Tiger owns a finite state machine.
    private FiniteStateMachine finiteStateMachine;
    // The current world position of the Tiger.
    public Vector3 currentPosition;
    // It points to the Player from the Tiger.
    private Vector3 vectorToPlayer;
    // The current world position of the Player.
    public Vector3 playerPositon;
    // The distance from the Tier to the Player.
    public float distanceToPlayer;
    // It defines the radius of the circle where
    // the Player is in the danger.
    public float dangerZone;
    // Animator component of this character to manipulate
    // its movement.
    public Animator animator;
    // Handles the navigation component.
    public NavMeshAgent navMeshAgent;
    // The Player object to examine its position.
    public GameObject player;
    // Showing the frustration level during the game.
    public Healthbar frustrationLevelBar;
    // It stores the waypoints the Tiger has to reach
    // while it is exploring.
    public GameObject[] waypoints;
    // Hiding points for the Tiger.
    public GameObject[] hidingPoints;
    // Defines when the Tiger attacks the Player.
    public float attackingDistance;
    // Stores the frustration level of the Tiger.
    public float frustrationLevel;
    // Stores the high limit of the frustration
    // level of the Tiger.
    public float maximumFrustrationLevel;
    // Whether the Tiger is in fear.
    private bool isInFear;

    // Start is called before the first frame update.
    void Start()
    {
        // Setting up the Tiger's FSM.
        finiteStateMachine = new FiniteStateMachine(this);
        finiteStateMachine.CurrentState = TigerExploreState.Instance;
        finiteStateMachine.PreviousState = TigerExploreState.Instance;

        // Inizialization.
        TigerExploreState.Instance.BeforeState(this);
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        waypoints = GameObject.FindGameObjectsWithTag("tiger");
        hidingPoints = GameObject.FindGameObjectsWithTag("tigerHide");
        frustrationLevel = 0.0f;
        UpdateFrustrationBar();
        dangerZone = 100.0f;
        attackingDistance = 20.0f;
        isInFear = false;
        maximumFrustrationLevel = 10.0f;
    }

    // Update is called once per frame.
    void Update()
    {
        UpdateCurrentPosition();
        UpdatePlayerPosition();
        CalculateVectorToPlayer();
        CalculateDistanceToPlayer();
        finiteStateMachine.Update();

        if (isInFear)
        {
            if (!IsAngry())
            {
                IncreaseFrustration();
            }
        }
    }

    // Increases the frustration level of the Tiger.
    private void IncreaseFrustration()
    {
        frustrationLevel += 0.01f;
        UpdateFrustrationBar();
    }

    // Returns whether the Player is in the danger
    // zone.
    public bool IsPlayerInDangerZone()
    {
        if (distanceToPlayer < dangerZone)
        {
            return true;
        }
        return false;
    }

    // Returns whether the Tiger is angry.
    public bool IsAngry()
    {
        if (frustrationLevel > maximumFrustrationLevel)
        {
            return true;
        }
        return false;
    }

    // Reduces the frustration level.
    public void CalmDown()
    {
        frustrationLevel = 0.01f;
        UpdateFrustrationBar();
    }

    // Starts increasing frustration level.
    public void StartFrustration()
    {
        isInFear = true;
    }

    // Stops increasing frustration level.
    public void StopFrustration()
    {
        isInFear = false;
    }

    // Updates the current position of the Player.
    void UpdatePlayerPosition()
    {
        playerPositon = player.transform.position;
    }

    // Updates the current position of the Tiger.
    void UpdateCurrentPosition()
    {
        currentPosition = this.transform.position;
    }

    // Calculates a 3D Vector which points from the Tiger
    // to the Player.
    void CalculateVectorToPlayer()
    {
        vectorToPlayer = playerPositon - currentPosition;
    }

    // Calculates the distance from the Tiger to the Player.
    void CalculateDistanceToPlayer()
    {
        distanceToPlayer = vectorToPlayer.magnitude;
    }

    // Whether the distance is close enough to chase.
    public bool IsInChasingDistance()
    {
        if (distanceToPlayer < dangerZone)
        {
            return true;
        }
        return false;
    }

    // Whether the distance is close enough to attack.
    public bool IsInAttackingDistance()
    {
        if (distanceToPlayer < attackingDistance)
        {
            return true;
        }
        return false;
    }

    // Updates the frustration bar of the Tiger.
    void UpdateFrustrationBar()
    {
        if (frustrationLevel > 0.0f && frustrationLevel < maximumFrustrationLevel)
        {
            frustrationLevelBar.UpdateHealth(frustrationLevel / maximumFrustrationLevel);
        }
    }

    // When the Tiger is hitting it has to look at the Player first.
    public void LookAtTarget()
    {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                Quaternion.LookRotation(vectorToPlayer),
                                                Time.deltaTime * 5.0f);
    }

    // Changes the Tiger's state.
    public override void ChangeState(State newState)
    {
        finiteStateMachine.ChangeState(newState);
    }

    // Returns to the previous state of the Tiger.
    public override void ReturnToPreviousState()
    {
        finiteStateMachine.ReturnToPreviousState();
    }

}
