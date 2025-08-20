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

    [SerializeField] private int DistractionAmount;

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

    public IEnumerator DelayAndActivate()
    {
        PanickUI.SetActive(false);
        yield return new WaitForSeconds(Random.Range(minimumTimeSpace - 1, maximumTimeSpace));

        Distraction1 ChosenDistractionScript = distractionsInScene[Random.Range(0, DistractionAmount)].GetComponent<Distraction1>();
        ChosenDistractionScript.Activate();
        PanickUI.SetActive(true);
    }
}
