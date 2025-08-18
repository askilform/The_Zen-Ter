using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}
