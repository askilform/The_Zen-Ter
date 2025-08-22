using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Distract_Manager : MonoBehaviour
{
    [SerializeField] private List<GameObject> distractionsInScene = new List<GameObject>();
    public int minimumTimeSpace;
    public int maximumTimeSpace;

    public GameObject PanickUI;
    public GameObject Arrow;
    public float CautionTimer;
    public TextMeshProUGUI CautionTimeTXT;
    private bool RadioActive;

    [SerializeField] private int DistractionAmount;
    private Zen_Meter zen_Meter;
    Vector3 ActiveDistractionLocation;

    private void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Distraction1>() != null)
            {
                distractionsInScene.Add(obj);
            }
        }

        DistractionAmount = distractionsInScene.Count;
        StartCoroutine(DelayAndActivate());
        CautionTimer = 20;
    }

    private void Update()
    {
        if (ActiveDistractionLocation != null)
        {
            Arrow.transform.LookAt(ActiveDistractionLocation);
        }

        if (RadioActive)
        {
            CautionTimer -= Time.deltaTime;
            CautionTimeTXT.text = CautionTimer.ToString("F0");

            if (CautionTimer < 0)
            {
                zen_Meter.Death();
                RadioActive = false;
            }
        }
    }
   
    public IEnumerator DelayAndActivate()
    {
        PanickUI.SetActive(false);
        Arrow.SetActive(false);
        RadioActive = false;
        CautionTimer = 20;

        yield return new WaitForSeconds(Random.Range(minimumTimeSpace - 1, maximumTimeSpace));

        GameObject ActiveDistraction = distractionsInScene[Random.Range(0, DistractionAmount)];
        Distraction1 ChosenDistractionScript = ActiveDistraction.GetComponent<Distraction1>();
        ActiveDistractionLocation = ActiveDistraction.transform.position;

        ChosenDistractionScript.Activate();
        PanickUI.SetActive(true);
        Arrow.SetActive(true);
        RadioActive = true;
    }
}
