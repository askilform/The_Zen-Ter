using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Distract_Manager : MonoBehaviour
{
    [SerializeField] private List<GameObject> distractionsInScene = new List<GameObject>();
    public int minimumTimeSpace;
    public int maximumTimeSpace;
    public GameObject PanickUI;
    public GameObject Arrow;

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
    }

    private void Update()
    {
        if (ActiveDistractionLocation != null)
        {
            Arrow.transform.LookAt(ActiveDistractionLocation);
        }
    }

    public IEnumerator DelayAndActivate()
    {
        PanickUI.SetActive(false);
        Arrow.SetActive(false);

        yield return new WaitForSeconds(Random.Range(minimumTimeSpace - 1, maximumTimeSpace));

        GameObject ActiveDistraction = distractionsInScene[Random.Range(0, DistractionAmount)];
        Distraction1 ChosenDistractionScript = ActiveDistraction.GetComponent<Distraction1>();
        ActiveDistractionLocation = ActiveDistraction.transform.position;

        ChosenDistractionScript.Activate();
        PanickUI.SetActive(true);
        Arrow.SetActive(true);
    }
}
