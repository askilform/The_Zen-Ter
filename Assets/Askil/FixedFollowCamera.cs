using UnityEngine;

public class FixedFollowCamera : MonoBehaviour
{
    public Transform target;                       // The player
    public Vector3 offset = new Vector3(0, 5, -8); // Fixed offset
    public float smoothSpeed = 5f;                 // Smooth follow speed
    public float rotationOffset = 90f;             // Extra Y rotation in degrees

    void LateUpdate()
    {
        if (!target) return;

        // Apply rotation offset to the offset vector
        Quaternion offsetRotation = Quaternion.Euler(0, rotationOffset, 0);
        Vector3 rotatedOffset = offsetRotation * offset;

        // Desired camera position (with rotation offset applied)
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smoothly move the camera toward the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Rotate camera to look at player with same offset applied
        Vector3 lookTarget = target.position + Vector3.up * 2f;
        transform.rotation = Quaternion.LookRotation(lookTarget - transform.position);
    }
}
