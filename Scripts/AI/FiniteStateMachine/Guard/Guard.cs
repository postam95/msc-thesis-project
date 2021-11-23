using UnityEngine;
using UnityEngine.AI;

// Represents the Guard FSM agent.
public class Guard : BaseCharacter
{

    // The Guard owns a finite state machine.
    private FiniteStateMachine finiteStateMachine;
    // The current world position of the Guard.
    public Vector3 currentPosition;
    // It points to the player from the Guard.
    private Vector3 vectorToPlayer;
    // The current world position of the Player.
    public Vector3 playerPositon;
    // Healthstate of the Guard.
    private float health;
    // The distance from the Guard to the player.
    private float distanceToPlayer;
    // Animator component of this character to manipulate
    // its movement.
    public Animator animator;
    // Handles the navigation component.
    public NavMeshAgent navMeshAgent;
    // The Player object to examine its position.
    public GameObject player;
    // The Bullet object is fired by the Guard.
    public GameObject bullet;
    // The Turret object to place the bullet.
    public GameObject turret;
    // Showing the health status during the game.
    public Healthbar healthbar;
    // It stores the waypoints the Guard has to reach
    // while patrolling.
    public GameObject[] waypoints;
    // Hiding points for the agent.
    public GameObject[] hidingPoints;
    // Maximum health of the Guard.
    public float maximumHealth;
    // Defines the amount of health can be reloaded during
    // an update cycle.
    // Rate must be positive and less than maximumHealth / 20.0.
    public float healthReloadStep;
    // Defines the amount of damage.
    // Rate must be positive and less than maximumHealth / 20.0.
    public float damageStep;
    // Defines when the guard start to chase the player.
    public float chasingDistance;
    // Defines when the guard start to shoot at the player.
    public float shootingDistance;

    // Start is called before the first frame update.
    void Start()
    {
        // Setting up the Guard's FSM.
        finiteStateMachine = new FiniteStateMachine(this);
        finiteStateMachine.CurrentState = PatrolState.Instance;
        finiteStateMachine.PreviousState = PatrolState.Instance;
        PatrolState.Instance.BeforeState(this);

        // Sign up for the messaging system.
        EntityManager.Instance.AddFsmCharacter(this);

        // Inizialization.
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        hidingPoints = GameObject.FindGameObjectsWithTag("hide");
        health = maximumHealth;
    }

    // Update is called once per frame.
    void Update()
    {
        UpdateCurrentPosition();
        UpdatePlayerPosition();
        CalculateVectorToPlayer();
        CalculateDistanceToPlayer();
        ReloadHealth();
        finiteStateMachine.Update();
    }

    // It handles the situaton when the Guard is hit by a bullet
    // and reduces its health.
    void OnCollisionEnter(Collision colliderObject)
    {
        if (colliderObject.gameObject.tag == "bullet")
        {
            if (health > 0.0f)
            {
                if (damageStep < 0.0f || damageStep > maximumHealth / 20.0f)
                {
                    damageStep = maximumHealth / 20.0f;
                }
            }
            UpdateHealth(health - damageStep);
        }
        Debug.Log(health + " " + damageStep);
        IsHealthCloseToZero();
        UpdateHealthBar();
    }

    // Updates the current position of the Guard while it's moving.
    void UpdatePlayerPosition()
    {
        playerPositon = player.transform.position;
    }

    // Updates the current position of the Guard while it's moving.
    void UpdateCurrentPosition()
    {
        currentPosition = this.transform.position;
    }

    // Calculates the 3D Vector which points from the Guard to the Player.
    void CalculateVectorToPlayer()
    {
        vectorToPlayer = playerPositon - currentPosition;
    }

    // Calculates the distance from the Guard to the Player.
    void CalculateDistanceToPlayer()
    {
        distanceToPlayer = vectorToPlayer.magnitude;
    }

    // Whether the distance is close enough to chase.
    public bool IsInChasingDistance()
    {
        if (distanceToPlayer < chasingDistance)
        {
            return true;
        }
        return false;
    }

    // Whether the distance is close enough to shoot.
    public bool IsInShootingDistance()
    {
        if (distanceToPlayer < shootingDistance)
        {
            return true;
        }
        return false;
    }

    // Reloads the Guard's health continuously at a specific rate until
    // it reaches the maximumHealth.
    void ReloadHealth()
    {
        if (health < maximumHealth)
        {
            if (healthReloadStep < 0.0f || healthReloadStep > maximumHealth / 20.0f)
            {
                healthReloadStep = maximumHealth / 20.0f;
            }
            if (health + healthReloadStep > maximumHealth)
            {
                UpdateHealth(maximumHealth);
            }
            else
            {
                UpdateHealth(health + healthReloadStep);
            }
        }
        else
        {
            UpdateHealth(maximumHealth);
        }
        UpdateHealthBar();
    }

    // Checks whether the health level is too low.
    void IsHealthCloseToZero()
    {
        if (health < 1.0f)
            Die();
    }

    // It destroys the game object.
    void Die()
    {
        Destroy(this.gameObject);
    }

    // Updates the health level.
    void UpdateHealth(float newHealth)
    {
        health = newHealth;
    }

    // Updates the Guard's healthbar.
    void UpdateHealthBar()
    {
        if (health > 1.0f && health < maximumHealth)
        {
            healthbar.UpdateHealth(health / maximumHealth);
        }
    }

    // Checks whether the health level is low.
    public bool IsHealthLow()
    {
        if (health < maximumHealth / 5 * 2)
        {
            return true;
        }
        return false;
    }

    // Checks whether the health level is high enough.
    public bool IsHealthHighEnough()
    {
        if (health > maximumHealth / 5 * 2.5)
        {
            return true;
        }
        return false;
    }

    // When the Guard is shooting it has to look at the Player first.
    public void LookAtTarget()
    {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                Quaternion.LookRotation(vectorToPlayer),
                                                Time.deltaTime * 5.0f);
    }

    // Making a flying bullet.
    void Fire()
    {
        GameObject flyingBullet = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        flyingBullet.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 5000);
    }

    // Stops firing.
    public void StopFiring()
    {
        CancelInvoke("Fire");
    }

    // Starts firing at a specific rate.
    public void StartFiring()
    {
        InvokeRepeating("Fire", 0.5f, 0.5f);
    }

    // Changes the Guard's state.
    public override void ChangeState(State newState)
    {
        finiteStateMachine.ChangeState(newState);
    }

    // Returns to the previous state of the Guard.
    public override void ReturnToPreviousState()
    {
        finiteStateMachine.ReturnToPreviousState();
    }

    // The Guard can receive messages. The message in
    // this case means one thing: ALARM.
    public override void HandleMessage(Message message)
    {
        switch(message.messageContent)
        {
            case MessageContent.ALARM:
                {
                    if (finiteStateMachine.CurrentState == PatrolState.Instance)
                    {
                        HelpState.Instance.targetPosition = message.extraContent;
                        ChangeState(HelpState.Instance);
                    }
                    return;
                }
        }
        return;
    }

    // The Guard can send messages to the others.
    public override void SendMessage()
    {
        MessageDispatcher.Instance.Broadcast(CharacterId, MessageContent.ALARM, playerPositon);
    }

}
