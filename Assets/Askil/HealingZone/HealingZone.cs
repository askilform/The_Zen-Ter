using UnityEngine;

public class HealingZone : MonoBehaviour
{
    private Zen_Meter zenScript;
    public float impact;

    private void Start()
    {
        zenScript = GameObject.Find("Zen-Meter").GetComponent<Zen_Meter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player"))
        {
            zenScript.ChangeMultiplier (-impact);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag == "Player"))
        {
            zenScript.ChangeMultiplier (impact);
        }
    }
}
