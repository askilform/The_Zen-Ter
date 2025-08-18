
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    private float ChargedFor;
    private int ChargeLevel = 0;
    private Collider ringCollider;

    public Image ChargeColour;
    public TextMeshProUGUI ChargeTXT;

    public AudioSource riseSFX;
    public AudioSource smackSFX;

    private void Start()
    {
        ringCollider = GetComponent<Collider>();
        ChargeColour.color = Color.white;
        ChargeTXT.text = "";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChargedFor = 0f;
            ChargeLevel = 1;
            ChargeColour.color = Color.yellow;
            ChargeTXT.text = "Wait";
            riseSFX.Play();
        }

        if (Input.GetMouseButton(0))
        {
            ChargedFor += Time.deltaTime;
            

            if (ChargedFor >= 2f && ChargeLevel < 2)
            {
                ChargeLevel = 2;
                ChargeColour.color = Color.green;
                ChargeTXT.text = "Go!";
            }

            if (ChargedFor >= 3f && ChargeLevel < 3)
            {
                ChargeLevel = 3;
                ChargeColour.color = Color.red;
                ChargeTXT.text = "No Dont!";
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ChargedFor = 0f;
            ChargeLevel = 1;
            ChargeColour.color = Color.white;
            ChargeTXT.text = "";
            riseSFX.Stop();
            smackSFX.Play();
        }
    }
}
