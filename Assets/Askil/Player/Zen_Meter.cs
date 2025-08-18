using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Zen_Meter : MonoBehaviour
{
    private float ZenLeft = 100;
    private bool hasDied = false;

    public float Reduction_Multiplier = 1;
    public Image ZenBar;
    public GameObject death_Prefab;
    public GameObject Owner;

    private void Start()
    {
        StartCoroutine(ReduceZen());
    }

    private void RestartReduction()
    {
        StartCoroutine(ReduceZen());
    }

    IEnumerator ReduceZen()
    {
        ZenLeft -= 1 * Reduction_Multiplier;
        yield return new WaitForSeconds(1f);
        ZenBar.fillAmount = ZenLeft * 0.01f;
        RestartReduction();

        if (ZenLeft < 0 && !hasDied)
        {
            Death();
        }
    }

    private void Death()
    {
        hasDied = true;
        Instantiate (death_Prefab, Owner.transform.position, Owner.transform.rotation);
        Destroy(Owner.gameObject);
    }
}
