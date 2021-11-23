using UnityEngine;
using UnityEngine.AI;
using Panda;

// Represents a Droid Agen driven by Behaviour Tree
// Decision Making AI method.
// It uses Panda Behaviour Tree Library.
public class DroidBehaviourTreeAgent : MonoBehaviour
{
    public Transform player;
    public Transform bulletSpawn;
    public Transform market;
    public GameObject bulletPrefab;
    public Inventory marketInventory;
    public Healthbar healthbar;

    GameObject[] waypoints;
    NavMeshAgent agent;
    Animator animator;
    public Vector3 destination; // The movement destination.
    public Vector3 target;      // The position to aim to.
    float health = 100.0f;
    float rotSpeed = 5.0f;


    float visibleRange = 120.0f;
    float shotRange = 100.0f;

    Vector3 lastAttackingPos;
    bool angry = false;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        waypoints = GameObject.FindGameObjectsWithTag("maintFortressWaypoint");
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(this.transform.position);
    }

    void UpdateHealth()
    {
        if (health < 100)
            health++;
    }

    // It handles the situaton when the Droid is hit by a bullet
    // and reduces its health.
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "bullet")
        {
            health -= 10;
            healthbar.UpdateHealth(health / 100.0f);
        }
    }

    [Task]
    public void StopMoving()
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        agent.ResetPath();
        Task.current.Succeed();
    }

    [Task]
    public void PickDestination(float x, float z)
    {
        Vector3 dest = new Vector3(x, 0, z);
        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void PickRandomDestination()
    {
        Vector3 dest = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void MoveToFightPosition()
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void MoveToDestination()
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void TargetPlayer()
    {
        target = player.transform.position;
        Task.current.Succeed();
    }

    [Task]
    bool Turn(float angle)
    {
        var p = this.transform.position + Quaternion.AngleAxis(angle, Vector3.up) * this.transform.forward;
        target = p;
        return true;
    }

    [Task]
    public void LookAtTarget()
    {
        Vector3 direction = target - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * rotSpeed);

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("angle={0}",
                Vector3.Angle(this.transform.forward, direction));

        if (Vector3.Angle(this.transform.forward, direction) < 5.0f)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public bool Fire()
    {
        animator.SetBool("isShooting", true);
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position,
                                                           bulletSpawn.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 6000);
        return true;
    }

    [Task]
    bool SeePlayer()
    {
        Vector3 distance = player.transform.position - this.transform.position;

        RaycastHit hit;
        bool seeWall = false;

        Debug.DrawRay(this.transform.position, distance, Color.red);

        if (Physics.Raycast(this.transform.position, distance, out hit))
        {
            if (hit.collider.gameObject.tag == "wall")
            {
                seeWall = true;
            }
        }

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("wall={0}", seeWall);

        if (distance.magnitude < visibleRange && !seeWall)
            return true;
        else
            return false;
    }

    [Task]
    public bool IsHealthLessThan(float health)
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("health={0}", this.health);

        return this.health < health;
    }

    [Task]
    public bool InDanger(float minDist)
    {
        Vector3 distance = player.transform.position - this.transform.position;
        return (distance.magnitude < minDist);
    }

    [Task]
    public void TargetAttackPos()
    {
        target = lastAttackingPos;
        Task.current.Succeed();
    }

    [Task]
    public void TakeCover()
    {
        Vector3 awayFromPlayer = this.transform.position - player.transform.position;

        //increased the flee range to the agent
        //has further to come back
        Vector3 dest = this.transform.position + awayFromPlayer * 5;
        agent.SetDestination(dest);

        //remember where we were before fleeing
        //and get angry
        lastAttackingPos = this.transform.position;
        angry = true;
        //don't be angry after 30 seconds
        //make sure this is longer than it takes for
        //health to be restored
        Invoke("CoolDown", 30);

        Task.current.Succeed();
    }

    [Task]
    public void PickWaypoint(int i)
    {
        Vector3 dest = waypoints[i - 1].transform.position;
        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    void CoolDown()
    {
        angry = false;
    }

    [Task]
    public bool IsAngry()
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("angry={0}", angry);

        return angry;
    }

    [Task]
    public bool Explode()
    {
        animator.SetBool("isDead", true);
        Invoke("Die", 2);
        return true;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    [Task]
    public void SetTargetDestination()
    {
        animator.SetBool("isShooting", false);
        agent.SetDestination(target);
        Task.current.Succeed();
    }

    [Task]
    bool ShotLinedUp()
    {
        Vector3 distance = target - this.transform.position;
        Debug.Log("ANGLE:  " + Vector3.Angle(this.transform.forward, distance) + " DISTANCE: " + distance.magnitude);
        if (distance.magnitude < shotRange &&
            Vector3.Angle(this.transform.forward, distance) < 2.0f)
            return true;
        else
            return false;
    }

    [Task]
    public void MoveToMedicalCentre()
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            //marketInventory.medicineLevel--;
            health = 100.0f;
            healthbar.UpdateHealth(health / 100.0f);
            Task.current.Succeed();
        }
    }

    [Task]
    public void PickUpMedicine()
    {
        agent.SetDestination(market.transform.position);
        Task.current.Succeed();
    }

    [Task]
    public bool IsMedicineAvailable()
    {
        //if (marketInventory.medicineLevel > 0)
        //    return true;
        return false;
    }

}

