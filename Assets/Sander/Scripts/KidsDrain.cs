using UnityEngine;

public class DrainZone : MonoBehaviour
{
    private Zen_Meter zenScript;
    private bool playerInside = false;
    public float drainRate = 0.2f;  // Amount to drain per second

    private void Start()
    {
        GameObject zenGO = GameObject.Find("Zen-Meter");
        if (zenGO != null)
        {
            zenScript = zenGO.GetComponent<Zen_Meter>();
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
            zenScript.Reduction_Multiplier += 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited healing zone.");
            zenScript.Reduction_Multiplier -= 0.5f;
        }
    }

}
