using UnityEngine;
using UnityEngine.AI;
using Panda;

// Represents a Droid Agent driven by Behaviour
// Tree Decision Making AI method.
// It uses Panda Behaviour Tree Library.
public class DroidGuard : BehaviourTreeAgent
{

    // Stores the id of this agent.
    private int charahcterId;
    // The Player object to examine its position.
    public Transform player;
    // The Turret object to place the bullet.
    public Transform turret;
    // The Droid Guard has to go for medicine to
    // the market to reload its health level.
    public Transform market;
    // The Bullet object is fired by the  Droid Guard.
    public GameObject bullet;
    // The Droid Guard has to know about the medicine
    // level of the market. This object provides it.
    public Inventory marketInventory;
    // Shows health status in the game.
    public Healthbar healthbar;
    // It stores the waypoints the Droid Guard has to
    // reach while it is patrolling around the fortress.
    GameObject[] waypoints;
    // Handles the navigation component.
    NavMeshAgent agent;
    // Animator component of this character to manipulate
    // its movement.
    Animator animator;
    // Stores the position of the attacked object.
    // Currently it can only be the Player.
    Vector3 targetPositionForAttacking;
    // This agent can return to the last fighting
    // position after its health reloaded. This
    // object stores this position.
    Vector3 lastAttackingPosition;
    // Stores the health level of the Droid Guard.
    float health = 100.0f;
    // Defines how quickly the Droid Guard can turn.
    float rotateSpeed = 5.0f;
    // In this distance the Droid Guard can ssee
    // the player.
    float visibleRange = 120.0f;
    // In this distance the Droid Guard can shoot
    // at the Player.
    float shotRange = 100.0f;
    // It shows wheter the Droid Guard wants to
    // fight back.
    bool wantRevenge = false;

    // Start is called before the first frame update.
    void Start()
    {
        // Initialization.
        agent = this.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        waypoints = GameObject.FindGameObjectsWithTag("maintFortressWaypoint");
    }

    // It handles the situaton when the Droid Guard is
    // hit by a bullet and reduces its health.
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "bullet")
        {
            health -= 10;
            healthbar.UpdateHealth(health / 100.0f);
        }
    }

    // Stops the agent if it is moving.
    [Task]
    public void StopMoving()
    {
        agent.ResetPath();
        Task.current.Succeed();
    }

    // This is reponsible for the moving process.
    [Task]
    public void MoveToDestination()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Task.current.Succeed();
        }
    }

    // Sets the target position to the Player's
    // position.
    [Task]
    public void TargetPlayer()
    {
        targetPositionForAttacking = player.transform.position;
        Task.current.Succeed();
    }

    // When the Droid Guard is close enough to
    // the Player, it tries to look at the Player
    // before it shoots.
    [Task]
    public void LookAtTarget()
    {
        // Turns around to look at the Player.
        Vector3 direction = targetPositionForAttacking - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * rotateSpeed);

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("angle={0}",
                Vector3.Angle(this.transform.forward, direction));

        // It stops turning when the Droid Guard
        // looks at the Player.
        if (Vector3.Angle(this.transform.forward, direction) < 5.0f)
        {
            Task.current.Succeed();
        }
    }

    // Start to fire bullets for the Player.
    [Task]
    public bool Fire()
    {
        animator.SetBool("isShooting", true);
        GameObject bulletToFire = GameObject.Instantiate(bullet, turret.transform.position,
                                                           turret.transform.rotation);
        bulletToFire.GetComponent<Rigidbody>().AddForce(bulletToFire.transform.forward * 6000);
        return true;
    }

    // Returns a bool value whether the Droid Guard
    // can see the Player. If a 'wall' tagged object
    // with a collider component is between the Player
    // and the Droid Guard, they can't see each other.
    [Task]
    public bool SeePlayer()
    {
        Vector3 distance = player.transform.position - this.transform.position;

        RaycastHit hitPoint;
        bool seeWall = false;

        Debug.DrawRay(this.transform.position, distance, Color.red);

        if (Physics.Raycast(this.transform.position, distance, out hitPoint))
        {
            if (hitPoint.collider.gameObject.tag == "wall")
            {
                seeWall = true;
            }
        }

        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("wall={0}", seeWall);
        }

        if (distance.magnitude < visibleRange && !seeWall)
        {
            return true;
        }
        return false;
    }

    // Examines whether the health level of the
    // Droid Guard is less than a specific value.
    [Task]
    public bool IsHealthLessThan(float givenHealthLevel)
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("health={0}", this.health);

        return this.health < givenHealthLevel;
    }

    // Tries to run away from the Player. It runs
    // when the health of the Player is quite low.
    [Task]
    public void TakeCover()
    {
        Vector3 awayFromPlayer = this.transform.position - player.transform.position;
        Vector3 dest = this.transform.position + awayFromPlayer * 5;

        agent.SetDestination(dest);
        lastAttackingPosition = this.transform.position;
        wantRevenge = true;

        Invoke("CoolDown", 60);

        Task.current.Succeed();
    }

    // Chooses one of the waypoints to patrol there.
    [Task]
    public void PickWaypoint(int i)
    {
        Vector3 dest = waypoints[i - 1].transform.position;
        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    // Finishes the cooling down process by setting
    // the indicator to false.
    private void CoolDown()
    {
        wantRevenge = false;
    }

    // Returns whether the agent is still angry.
    [Task]
    public bool WantsRevenge()
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("angry={0}", wantRevenge);

        return wantRevenge;
    }

    // Prepares the agent's exploding process.
    [Task]
    public bool Explode()
    {
        animator.SetBool("isDead", true);
        Invoke("Die", 2);
        return true;
    }

    // Destroys this agent and its healtbar.
    void Die()
    {
        Destroy(healthbar.gameObject);
        Destroy(this.gameObject);
    }

    // Sets the target position for attacking.
    [Task]
    public void SetTargetDestination()
    {
        animator.SetBool("isShooting", false);
        agent.SetDestination(targetPositionForAttacking);
        Task.current.Succeed();
    }

    // Checks whether everything looks fine for
    // shooting. It requires proper distance and
    // angle to the Player.
    [Task]
    bool ShotLinedUp()
    {
        Vector3 distance = targetPositionForAttacking - this.transform.position;
        if (distance.magnitude < shotRange && Vector3.Angle(this.transform.forward, distance) < 2.0f)
        {
            return true;
        }
        return false;
    }

    // Moves to the market for medicine.
    [Task]
    public void MoveToMarket()
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            MarketInventory inventory = (MarketInventory)this.marketInventory;
            inventory.DecreaseMedicineLevel(1);
            health = 100.0f;
            healthbar.UpdateHealth(health / 100.0f);
            Task.current.Succeed();
        }
    }

    // Picks up medicine from the market.
    [Task]
    public void PickUpMedicine()
    {
        agent.SetDestination(market.transform.position);
        Task.current.Succeed();
    }

    // Returns true or false whether the market
    // market has available medicine.
    [Task]
    public bool IsMedicineAvailable()
    {
        MarketInventory inventory = (MarketInventory)this.marketInventory;
        if (inventory.hasMedicineAvailable())
        {
            return true;
        }
        return false;
    }

}