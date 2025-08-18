using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3.0f;         // Movement speed of the enemy
    public float stoppingDistance = 1.5f; // Distance to stop from the player

    private Transform player;

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // Calculate distance to player
        float distance = Vector3.Distance(transform.position, player.position);

        // Move towards player if further than stoppingDistance
        if (distance > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        // Optional: Make enemy look at the player
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // Keep the enemy upright
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);
    }
}
