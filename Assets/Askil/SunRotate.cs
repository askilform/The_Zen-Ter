using UnityEngine;

public class SunRotate : MonoBehaviour
{
    [SerializeField] public float rotationSpeed = 10f; // Degrees per second
    private float currentY = 0f;

    void Update()
    {
        currentY += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(50f, currentY, 0f);
    }
}
