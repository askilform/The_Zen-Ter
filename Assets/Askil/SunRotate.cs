using UnityEngine;

public class SunRotate : MonoBehaviour
{
    [SerializeField] public float rotationSpeed; // Degrees per second
    [SerializeField] private Vector3 rotationAxis = Vector3.right; // Default: rotate around X axis

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
