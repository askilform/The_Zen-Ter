using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    public float StunTime = 3;
    public Animator Animator;
    public ParticleSystem SlapParticles;

    private Transform player;
    private NavMeshAgent agent;
    private bool isStunned = false;
    private Zen_Meter zenScript;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        zenScript = GameObject.Find("Zen-Meter").GetComponent<Zen_Meter>();
    }

    void Update()
    {
        Animator.SetBool("Stunned", isStunned);
        if (player != null && !isStunned)
        {
            agent.SetDestination(player.position);
        }
    }
    
    public IEnumerator Stun()
    {
        zenScript.ChangeZenLevel(+10);
        isStunned = true;
        SlapParticles.Play();
        yield return new WaitForSeconds(StunTime);
        isStunned = false;
    }

    public IEnumerator SmackedTooHard()
    {
        zenScript.ChangeZenLevel(-10);

        isStunned = true;
        yield return new WaitForSeconds(10);
        isStunned = false;
    }
}
