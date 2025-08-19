using UnityEngine;

public class HealingZone : MonoBehaviour
{
    private Zen_Meter zenScript;

    private void Start()
    {
        zenScript = GameObject.Find("Zen-Meter").GetComponent<Zen_Meter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player"))
        {
            zenScript.ChangeMultiplier (-2f);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag == "Player"))
        {
            zenScript.ChangeMultiplier (2f);
        }
    }
}
