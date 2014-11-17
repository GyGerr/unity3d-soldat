using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
    public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
	public GameObject[] patrolWayPoints;                    // An array of transforms for the patrol route.
	private GameObject player;


    private EnemySight enemySight;
    public NavMeshAgent nav;
    
    private HP playerHealth;
    private LastPlayerSighting lastPlayerSighting;          // Last global sighting of the player.
    private float chaseTimer;
    private float patrolTimer;
    
	public int wayPointIndex;
	public GameObject ak47;
	private Ak47Controller ak;
    private Animator anim;
    public GameObject enemy;



    void Awake()
    {
        enemySight = GetComponent<EnemySight>();
        // nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HP>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
        patrolWayPoints = GameObject.FindGameObjectsWithTag ("PatrolWaypoint");
        ak = ak47.GetComponent<Ak47Controller>();
        anim = enemy.GetComponent<Animator>();
    }


    void Update()
    {     
        if (enemySight.playerInSight && playerHealth.hp > 0f)
        {
            Shooting();
        }           
        // If the player has been sighted and isn't dead...
		else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth && playerHealth.hp > 0f)
        {
            Chasing();
        }
        else
        {
            Patrolling();
        }
    }


    void Shooting()
    {
        nav.Stop();
        anim.SetFloat(Animator.StringToHash("Speed"), 0.0f);
        nav.destination = enemy.transform.position;
        ak.Shoot();
    }


    void Chasing()
    {
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - enemy.transform.position;
        anim.SetFloat(Animator.StringToHash("Speed"), 1.0f);

        // If the the last personal sighting of the player is not close...
        if (sightingDeltaPos.sqrMagnitude > 4f)
        {
            nav.destination = enemySight.personalLastSighting;
            // nav.SetDestination(enemySight.personalLastSighting);
        }

        nav.speed = chaseSpeed;

        // If near the last personal sighting
        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            // Chasing cooldown
            if (chaseTimer >= chaseWaitTime)
            {
                lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
                chaseTimer = 0f;
            }
        }
        else
        {
            // If not near the last sighting personal sighting of the player, reset the timer.
            anim.SetFloat(Animator.StringToHash("Speed"), 1.0f);
            chaseTimer = 0f;
        }
    }


    void Patrolling()
    {
        // Set an appropriate speed for the NavMeshAgent.
        nav.speed = patrolSpeed;
        
        // If near the next waypoint or there is no destination...
        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance <= nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                if (wayPointIndex == patrolWayPoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }

                patrolTimer = 0;
            }
        }
        else
        {
            // If not near a destination, reset the timer.
            anim.SetFloat(Animator.StringToHash("Speed"), 1.0f);
            patrolTimer = 0;
        }

        nav.destination = patrolWayPoints[wayPointIndex].transform.position;
    }
}