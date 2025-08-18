using TMPro;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float ChargedFor;
    private int ChargeLevel = 0;
    private Collider ringCollider;

    public TextMeshProUGUI ChargeTimeTXT;
    public TextMeshProUGUI ChargeLevelTXT;
    public AudioSource riseSFX;
    public AudioSource smackSFX;

    private void Start()
    {
        ringCollider = GetComponent<Collider>();
        ChargeTimeTXT.text = "Charged For: " + Mathf.Round(ChargedFor * 100f) / 100f;
        ChargeLevelTXT.text = "ChargeLevel: " + ChargeLevel;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChargedFor = 0f;
            ChargeLevel = 1;
            riseSFX.Play();
        }

        if (Input.GetMouseButton(0))
        {
            ChargedFor += Time.deltaTime;
            ChargeTimeTXT.text = "Charged For: " + Mathf.Round(ChargedFor * 100f) / 100f;
            ChargeLevelTXT.text = "ChargeLevel: " + ChargeLevel;

            if (ChargedFor >= 2f && ChargeLevel < 2)
            {
                ChargeLevel = 2;
            }

            if (ChargedFor >= 4f && ChargeLevel < 3)
            {
                ChargeLevel = 3;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ChargedFor = 0f;
            ChargeLevel = 1;
            ChargeTimeTXT.text = "Charged For: " + Mathf.Round(ChargedFor * 100f) / 100f;
            ChargeLevelTXT.text = "ChargeLevel: " + ChargeLevel;
            riseSFX.Stop();
            smackSFX.Play();
        }
    }
}
