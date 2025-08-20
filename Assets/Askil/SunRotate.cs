using UnityEngine;

public class SunRotate : MonoBehaviour
{
    public float rotateSpeed;
    private Vector3 addRotation = new Vector3(1f, 0f, 0f);

    private void Update()
    {
        transform.Rotate(addRotation * rotateSpeed * Time.deltaTime);
    }
}
