using UnityEngine;

public class HealingZone : MonoBehaviour
{
    private Zen_Meter ZenScript;
    private bool playerInside = false;
    public float drainRate = 0.2f;  // Amount to drain per second

    private void Start()
    {
        GameObject zenGO = GameObject.Find("Zen-Meter");
        if (zenGO != null)
        {
            ZenScript = zenGO.GetComponent<Zen_Meter>();
        }
        else
        {
            Debug.LogError("Zen-Meter GameObject not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered healing zone.");
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited healing zone.");
            playerInside = false;
        }
    }

    private void Update()
    {
        if (playerInside && zenScript != null)
        {
            ZenScript.ChangeMultiplier(-drainRate * Time.deltaTime);
        }
    }
}
