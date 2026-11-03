using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GazeMovement : MonoBehaviour
{
    public Transform playerCamera;
    public float speed = 2f;

    [Tooltip("Inclinación hacia abajo necesaria para avanzar.")]
    public float lookDownAngle = 25f;

    private CharacterController controller;
    private float verticalSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (playerCamera == null)
            return;

        Vector3 forward = playerCamera.forward;

        float downwardAngle = Mathf.Asin(
            Mathf.Clamp(-forward.y, -1f, 1f)
        ) * Mathf.Rad2Deg;

        Vector3 horizontalVelocity = Vector3.zero;

        if (downwardAngle >= lookDownAngle)
        {
            Vector3 direction = Vector3.ProjectOnPlane(
                forward, Vector3.up
            ).normalized;

            horizontalVelocity = direction * speed;
        }

        if (controller.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -2f;

        verticalSpeed += Physics.gravity.y * Time.deltaTime;

        Vector3 velocity = horizontalVelocity
                         + Vector3.up * verticalSpeed;

        controller.Move(velocity * Time.deltaTime);
    }
}