using UnityEngine;

public class ZenDrainObject : MonoBehaviour
{
    public float drainMultiplier = -2f;

    private Zen_Meter zenMeter;
    private bool isActive = false;
    private bool hasAppliedMultiplier = false;
    private bool playerInRange = false;

    private void Start()
    {
        zenMeter = GameObject.Find("Zen-Meter").GetComponent<Zen_Meter>();

        if (zenMeter == null)
        {
            Debug.LogError("Zen_Meter not found in scene. Check object name or script.");
        }

        ZenDrainManager.RegisterObject(this);
    }

    private void Update()
    {
        if (!isActive) return;

        if (!hasAppliedMultiplier && zenMeter != null)
        {
            zenMeter.ChangeMultiplier(drainMultiplier);
            hasAppliedMultiplier = true;
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        isActive = true;
        hasAppliedMultiplier = false;

        // Optional: visual feedback
        // gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    public void Deactivate()
    {
        isActive = false;

        if (hasAppliedMultiplier && zenMeter != null)
        {
            zenMeter.ChangeMultiplier(-drainMultiplier); // Reset it
        }

        ZenDrainManager.Instance.OnObjectDeactivated();

        // Optional: visual feedback
        // gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}
