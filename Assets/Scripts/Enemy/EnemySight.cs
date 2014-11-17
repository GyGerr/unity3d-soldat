using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;           // Number of degrees, centred on forward, for the enemy see.
    public bool playerInSight;                      // Whether or not the player is currently sighted.
    public Vector3 personalLastSighting;            // Last place this enemy spotted the player.

    private NavMeshAgent nav;                       // Reference to the NavMeshAgent component.
    public SphereCollider col;                     // Reference to the sphere collider trigger component.
    private LastPlayerSighting lastPlayerSighting;
    public GameObject player;
    private HP playerHealth;              // Reference to the player's health script.
    //private HashIDs hash;                           // Reference to the HashIDs.
    public Vector3 previousSighting;               // Where the player was sighted last frame.
	private Animator anim;
    public GameObject enemy;


    void Awake()
    {
        // Setting up the references.
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HP>();
        anim = enemy.GetComponent<Animator>();
        //hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();

        // Set the personal sighting and the previous sighting to the reset position.
        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }


    void Update()
    {
        // If the last global sighting of the player has changed...
        if (lastPlayerSighting.position != previousSighting)
        {
            // ... then update the personal sighting to be the same as the global sighting.
            personalLastSighting = lastPlayerSighting.position;
        }

        // Set the previous sighting to the be the sighting from this frame.
        previousSighting = lastPlayerSighting.position;

		if (playerHealth && playerHealth.hp > 0f)
            // ... set the animator parameter to whether the player is in sight or not.
			anim.SetFloat(Animator.StringToHash("Speed"), 1.0f); // playerInSight
        else
            // ... set the animator parameter to false.
			anim.SetFloat(Animator.StringToHash("Speed"), 0.0f);
    }


    void OnTriggerStay(Collider other)
    {
		playerInSight = false;

        if (other.tag == "Player")
        {
            // Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = other.transform.position - enemy.transform.position;
            float angle = Vector3.Angle(direction, transform.forward); // up

            // If the angle between forward and where the player is, is less than half the angle of view...
            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
    
                if (Physics.Raycast(enemy.transform.position + enemy.transform.forward, direction.normalized, out hit, col.radius)) //col.radius * 5
                {
                    lastPlayerSighting.position = player.transform.position;
                    playerInSight = true;                     
                }
            }
            
            //int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).nameHash;
            //int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).nameHash;

            // If the player is attracting attention withing collider
            //if (playerLayerZeroStateHash == hash.locomotionState)
            //{
                // if (CalculatePathLength(player.transform.position) <= col.radius)
                    personalLastSighting = player.transform.position;
            //}
        }
    }

	// Player left colider, is not in sight.
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInSight = false;
        }
    }


    float CalculatePathLength(Vector3 targetPosition)
    {
        // Create a path and set it based on a target position.
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled) { 
            nav.CalculatePath(targetPosition, path);
        }

        // Create an array of points which is the length of the number of corners in the path + 2.
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        // The first point is the enemy's position.
        allWayPoints[0] = enemy.transform.position;

        // The last point is the target position.
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        // The points inbetween are the corners of the path.
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        // Create a float to store the path length that is by default 0.
        float pathLength = 0;

        // Increment the path length by an amount equal to the distance between each waypoint and the next.
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}