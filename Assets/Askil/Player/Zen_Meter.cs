using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Zen_Meter : MonoBehaviour
{
    private bool hasDied = false;
    private bool Suspence = false;

    public float ZenLeft = 100;
    public float Reduction_Multiplier = 1;
    public Image ZenBar;
    public GameObject death_Prefab;
    public GameObject Owner;
    public AudioSource IncreaseSFX;
    public AudioSource SuspenceSFX;

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

        if (Reduction_Multiplier > 2 && !Suspence)
        {
            SuspenceSFX.Play();
            Suspence = true;
            yield return new WaitForSeconds(3f);
            Suspence = false;
        }

        if (ZenLeft > 100)
        {
            ZenLeft = 100;
        }

        if (ZenLeft < 0 && !hasDied)
        {
            Death();
        }

        RestartReduction();
    }

    private void Death()
    {
        hasDied = true;
        Instantiate (death_Prefab, Owner.transform.position, Owner.transform.rotation);
        Destroy(Owner.gameObject);
    }

    public void ChangeMultiplier (float add)
    {
        Reduction_Multiplier += add;
    }

    public void ChangeZenLevel (float add)

    {
        ZenLeft += add;
        if (add > 0)
        {
            IncreaseSFX.Play();
        }

        if (ZenLeft > 100)
        {
            ZenLeft = 100;
        }

        ZenBar.fillAmount = ZenLeft * 0.01f;
    }
}
