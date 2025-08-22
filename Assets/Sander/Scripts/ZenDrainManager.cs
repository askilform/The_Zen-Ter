using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZenDrainManager : MonoBehaviour
{
    public static ZenDrainManager Instance;

    private static List<ZenDrainObject> allObjects = new List<ZenDrainObject>();
    private ZenDrainObject currentActive = null;

    public float timeBetweenActivations = 10f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void RegisterObject(ZenDrainObject obj)
    {
        if (!allObjects.Contains(obj))
        {
            allObjects.Add(obj);
        }
    }

    private void Start()
    {
        StartCoroutine(ActivationLoop());
    }

    private IEnumerator ActivationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenActivations);

            if (currentActive == null)
            {
                ActivateRandomObject();
            }
        }
    }

    private void ActivateRandomObject()
    {
        if (allObjects.Count == 0) return;

        List<ZenDrainObject> inactive = allObjects.FindAll(obj => !obj.IsActive());

        if (inactive.Count == 0) return;

        int index = Random.Range(0, inactive.Count);
        currentActive = inactive[index];
        currentActive.Activate();
    }

    public void OnObjectDeactivated()
    {
        currentActive = null;
    }
}
