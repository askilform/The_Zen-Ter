using UnityEngine;

public class FixedFollowCamera : MonoBehaviour
{
    public Transform target;                 // The player
    public Vector3 offset = new Vector3(0, 5, -8); // Fixed world offset (not relative to player rotation)
    public float smoothSpeed = 5f;           // Smooth follow speed

    void LateUpdate()
    {
        if (!target) return;

        // Desired camera position (fixed offset in world space)
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera toward the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Keep rotation fixed (camera always looks forward at the player)
        transform.LookAt(target.position + Vector3.up * 2f); // Aim slightly above player
    }
}
