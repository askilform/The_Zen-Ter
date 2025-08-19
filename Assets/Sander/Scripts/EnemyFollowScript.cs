using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform player;
    public float StunTime = 3;

    private NavMeshAgent agent;
    private bool isStunned = false;

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
        if (player != null && !isStunned)
        {
            agent.SetDestination(player.position);
        }
    }
    
    public IEnumerator Stun()
    {
        isStunned = true;
        yield return new WaitForSeconds(10);
        isStunned = false;
    }
}
