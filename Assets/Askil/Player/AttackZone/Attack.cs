
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class Attack : MonoBehaviour
{
    private float ChargedFor;
    private int ChargeLevel = 0;
    private Collider ringCollider;
    [SerializeField] private List<GameObject> kidsInsideRing = new List<GameObject>();

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
        if (Input.GetButtonDown("Charge"))
        {
            ChargedFor = 0f;
            ChargeLevel = 1;
            ChargeColour.color = Color.yellow;
            ChargeTXT.text = "Wait";
            riseSFX.Play();
        }

        if (Input.GetButton("Charge"))
        {
            ChargedFor += Time.deltaTime;


            if (ChargedFor >= 2f && ChargeLevel < 2)
            {
                ChargeLevel = 2;
                ChargeColour.color = Color.green;
                ChargeTXT.text = "Go!";
            }

            if (ChargedFor >= 2.3f && ChargeLevel < 3)
            {
                ChargeLevel = 3;
                ChargeColour.color = Color.red;
                ChargeTXT.text = "No Dont!";
            }
        }

        if (Input.GetButtonUp("Charge"))
        {

            if (ChargeLevel == 2)
            {
                foreach (GameObject kids in kidsInsideRing)
                {
                    EnemyPathfinding KidMovement = kids.GetComponent<EnemyPathfinding>();
                    KidMovement.StartCoroutine(KidMovement.Stun());
                }
            }

            if (ChargeLevel == 3)
            {
                foreach (GameObject kids in kidsInsideRing)
                {
                    EnemyPathfinding KidMovement = kids.GetComponent<EnemyPathfinding>();
                    KidMovement.StartCoroutine(KidMovement.SmackedTooHard());
                }
            }

            ChargedFor = 0f;
            ChargeLevel = 1;
            ChargeColour.color = Color.white;
            ChargeTXT.text = "";
            riseSFX.Stop();
            smackSFX.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyPathfinding>() != null)
        {
            kidsInsideRing.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyPathfinding>() != null)
        {
            kidsInsideRing.Remove(other.gameObject);
        }
    }
}
