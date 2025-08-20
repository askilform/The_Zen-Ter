using UnityEngine;

public class Distraction1 : MonoBehaviour
{
    public bool Active = false;
    public GameObject ActiveImage;
    public GameObject HighlightImage;
    public float addToMultiplier;

    private Zen_Meter zen_Meter;
    private bool playerClose;

    private void Start()
    {
        zen_Meter = GameObject.Find("Zen-Meter").GetComponent<Zen_Meter>();
        ActiveImage.SetActive(false);
        HighlightImage.SetActive(false);
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
    }

    public void DeActivate()
    {
        Active = false;
        ActiveImage.SetActive(false);
        HighlightImage.SetActive(false);
    }
}
