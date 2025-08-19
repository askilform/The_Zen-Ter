using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolChase : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;

    public float chaseRange = 10f;
    public float stopChaseRange = 15f;
    public float waitTime = 2f;

    // New: teleport target for the player
    public Transform playerRespawnPoint;

    private NavMeshAgent agent;
    private Transform currentPatrolTarget;
    private bool chasingPlayer = false;
    private bool waiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        currentPatrolTarget = pointA;
        GoTo(currentPatrolTarget);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!chasingPlayer && distanceToPlayer <= chaseRange)
        {
            chasingPlayer = true;
        }
        else if (chasingPlayer && distanceToPlayer >= stopChaseRange)
        {
            chasingPlayer = false;
            GoToClosestPatrolPoint();
        }

        if (chasingPlayer)
        {
            agent.SetDestination(player.position);
            FaceTarget(player.position);
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !waiting)
            {
                StartCoroutine(SwitchPatrolPointAfterDelay());
            }
        }
    }

    void GoTo(Transform target)
    {
        agent.SetDestination(target.position);
    }

    void GoToClosestPatrolPoint()
    {
        float distA = Vector3.Distance(transform.position, pointA.position);
        float distB = Vector3.Distance(transform.position, pointB.position);
        currentPatrolTarget = (distA < distB) ? pointA : pointB;
        GoTo(currentPatrolTarget);
    }

    System.Collections.IEnumerator SwitchPatrolPointAfterDelay()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
        GoTo(currentPatrolTarget);
        waiting = false;
    }

    void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // New method for detecting collision with player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerRespawnPoint != null)
            {
                // Teleport player to respawn point
                collision.gameObject.transform.position = playerRespawnPoint.position;
            }
            else
            {
                Debug.LogWarning("Player respawn point not assigned!");
            }
        }
    }
}
