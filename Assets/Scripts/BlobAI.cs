using UnityEngine;
using UnityEngine.AI;

public class BlobAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] waypoints; 
    private int currentWaypointIndex = 0;

    [Header("Detection Settings")]
    public Transform player;      
    public float detectionRange = 8f; 

    private NavMeshAgent agent;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        GoToNextWaypoint();
    }

    void Update()
    {
        if (player == null) return;

        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

       
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            
            if (isChasing)
            {
                isChasing = false;
                GoToNextWaypoint();
            }
        }

       
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

       
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextWaypoint();
        }
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        
        agent.destination = waypoints[currentWaypointIndex].position;

        
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void ChasePlayer()
    {
      
        agent.destination = player.position;
    }

   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}