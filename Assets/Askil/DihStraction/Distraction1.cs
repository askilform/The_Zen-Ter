using UnityEngine;

public class Distraction1 : MonoBehaviour
{
    public bool Active = false;
    public GameObject ActiveImage;
    public GameObject HighlightImage;
    public float addToMultiplier;

    private Zen_Meter zen_Meter;
    private bool playerClose;
    private Distract_Manager distract_Manager;
    private AudioSource Sound;

    private void Start()
    {
        distract_Manager = GameObject.Find("Distraction-Manager").GetComponent<Distract_Manager>();
        Sound = GetComponent<AudioSource>();
        zen_Meter = GameObject.Find("Zen-Meter").GetComponent<Zen_Meter>();
        ActiveImage.SetActive(false);
        HighlightImage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackZone")
        {
            playerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "AttackZone")
        {
            playerClose = false;
        }
    }

    private void Update()
    {
        if (playerClose && Active)
        {
            HighlightImage.SetActive(true);

            if (Input.GetButtonDown("Interact"))
            {
                DeActivate();
            }
        }

        else
        {
            HighlightImage.SetActive(false);
        }
    }

    public void Activate()
    {
        Active = true;
        ActiveImage.SetActive(true);
        zen_Meter.ChangeMultiplier(addToMultiplier);
        Sound.Play();
    }

    public void DeActivate()
    {
        Active = false;
        ActiveImage.SetActive(false);
        HighlightImage.SetActive(false);
        distract_Manager.StartCoroutine(distract_Manager.DelayAndActivate());
        zen_Meter.ChangeMultiplier(-addToMultiplier);
        Sound.Stop();
    }
}
