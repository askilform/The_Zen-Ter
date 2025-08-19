using UnityEngine;

public class HealingZone : MonoBehaviour
{
    private Zen_Meter zenScript;
    private bool playerInside = false;
    public float drainRate = 0.2f;  // Amount to drain per second

    private void Start()
    {
        zenScript = GameObject.Find("Zen-Meter").GetComponent<Zen_Meter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            // Drain zen meter over time
            zenScript.ChangeMultiplier(-drainRate * Time.deltaTime);
        }
    }
}
