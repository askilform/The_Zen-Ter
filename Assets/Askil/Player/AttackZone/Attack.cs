using UnityEngine;

public class Attack : MonoBehaviour
{
    private float ChargedFor;
    private int ChargeLevel = 0;
    private Collider ringCollider;

    private void Start()
    {
        ringCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChargedFor = 0f; // reset charge when starting
            ChargeLevel = 1;
            print("Charge Level: " + ChargeLevel);
        }

        if (Input.GetMouseButton(0))
        {
            ChargedFor += Time.deltaTime;

            if (ChargedFor >= 2f && ChargeLevel < 2)
            {
                ChargeLevel = 2;
                print("Charge Level: " + ChargeLevel);
            }

            if (ChargedFor >= 4f && ChargeLevel < 3)
            {
                ChargeLevel = 3;
                print("Charge Level: " + ChargeLevel);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            print("Attack! Charge Level: " + ChargeLevel);
        }
    }
}
