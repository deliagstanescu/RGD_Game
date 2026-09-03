using UnityEngine;
using UnityEngine.AI;

public class BlobAI : MonoBehaviour
{
    public bool isActivated = false;

    [Header("Patrol Settings")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Detection Settings")]
    public Transform player;
    public float detectionRange = 8f;
    public float lostSightTimeLimit = 3f;

    private NavMeshAgent agent;
    private bool isChasing = false;
    private float lostSightTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (isActivated)
        {
            GoToNextWaypoint();
        }
    }

    void Update()
    {
        if (!isActivated)
            return;

        if (player == null)
            return;
        Vector2 enemyPos2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerPos2D = new Vector2(player.position.x, player.position.z);
        float distanceToPlayer = Vector2.Distance(enemyPos2D, playerPos2D);

        if (distanceToPlayer <= 1.8f)
        {
            if (GameOverManager.Instance != null)
            {
                GameOverManager.Instance.TriggerGameOver("You were caught by the puppet!");
            }
            return;
        }
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            lostSightTimer = 0f;
        }
        else
        {
            if (isChasing)
            {
                lostSightTimer += Time.deltaTime;

                if (lostSightTimer >= lostSightTimeLimit)
                {
                    isChasing = false;
                    lostSightTimer = 0f;
                    GoToNextWaypoint();
                }
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

    public void ActivateEnemy()
    {
        if (isActivated) return;

        isActivated = true;
        GoToNextWaypoint();
    }

    void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextWaypoint();
        }
    }

    void GoToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

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
    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated) return;

        if (other.CompareTag("Player"))
        {
            if (GameOverManager.Instance != null)
            {
                GameOverManager.Instance.TriggerGameOver("Ai fost prins de papusa!");
            }
        }
    }
}