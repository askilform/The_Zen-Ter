using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;           // Movement speed
    public float gravity = -9.81f;     // Gravity strength
    public float rotationSpeed = 10f;  // How fast the player rotates
    public float rotationOffset = 90f; // Offset in degrees (adjust to your map)

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Move direction (world space)
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        if (move.magnitude >= 0.1f)
        {
            // Apply rotation offset to direction
            Quaternion offsetRotation = Quaternion.Euler(0, rotationOffset, 0);
            Vector3 adjustedMove = offsetRotation * move;

            // Rotate toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(adjustedMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Apply movement
            controller.Move(adjustedMove * speed * Time.deltaTime);
        }
    }
}
