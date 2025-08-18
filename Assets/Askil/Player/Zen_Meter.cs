using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Zen_Meter : MonoBehaviour
{
    private float ZenLeft = 100;

    public float Reduction_Multiplier = 1;
    public Image ZenBar;
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
        yield return new WaitForSeconds(1);
        ZenBar.fillAmount = ZenLeft * 0.01f;
        RestartReduction();
    }
}
