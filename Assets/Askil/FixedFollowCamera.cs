using UnityEngine;

public class FixedFollowCamera : MonoBehaviour
{
    public Transform target;                       // The player
    public Vector3 offset = new Vector3(0, 5, -8); // Final fixed offset
    public float smoothSpeed = 5f;                 // Smooth follow speed
    public float rotationOffset = 90f;             // Extra Y rotation in degrees

    [Header("Cinematic Intro")]
    public Vector3 startOffset = new Vector3(0, 20, -20); // Starting camera offset (high above)
    public float introDuration = 2.5f;                     // Time to zoom in
    private float introTimer = 0f;
    private bool introPlaying = true;

    void Start()
    {
        if (target)
        {
            // Place camera at start offset instantly
            Quaternion offsetRotation = Quaternion.Euler(0, rotationOffset, 0);
            Vector3 rotatedStart = offsetRotation * startOffset;
            transform.position = target.position + rotatedStart;
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        // Apply rotation offset to final offset
        Quaternion offsetRotation = Quaternion.Euler(0, rotationOffset, 0);
        Vector3 rotatedOffset = offsetRotation * offset;

        Vector3 desiredPosition;

        if (introPlaying)
        {
            // Animate from startOffset to offset
            introTimer += Time.deltaTime;
            float t = Mathf.Clamp01(introTimer / introDuration);

            Vector3 rotatedStart = offsetRotation * startOffset;
            Vector3 cinematicPos = Vector3.Lerp(target.position + rotatedStart, target.position + rotatedOffset, t);
            desiredPosition = cinematicPos;

            if (t >= 1f) introPlaying = false;
        }
        else
        {
            // Normal follow
            desiredPosition = target.position + rotatedOffset;
        }

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Rotate camera to look at player
        Vector3 lookTarget = target.position + Vector3.up * 2f;
        transform.rotation = Quaternion.LookRotation(lookTarget - transform.position);
    }
}
