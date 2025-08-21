using UnityEngine;

public class SunRotate : MonoBehaviour
{
    [SerializeField] public float rotationSpeed = 10f; // Degrees per second
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Rotate around Y axis

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
