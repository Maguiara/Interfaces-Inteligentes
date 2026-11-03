using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.InputSystem;
#endif

public class EditorPlayerControls : MonoBehaviour
{
    public Transform playerCamera;
    public float moveSpeed = 3f;
    public float mouseSensitivity = 0.15f;

#if UNITY_EDITOR
    private float pitch;

    private void Update()
    {
        if (playerCamera == null)
            return;

        // Mantén pulsado el botón derecho para mirar.
        if (Mouse.current != null &&
            Mouse.current.rightButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();

            transform.Rotate(
                Vector3.up,
                delta.x * mouseSensitivity,
                Space.World
            );

            pitch -= delta.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            playerCamera.localRotation =
                Quaternion.Euler(pitch, 0f, 0f);
        }

        if (Keyboard.current == null)
            return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;

        Vector3 forward = Vector3.ProjectOnPlane(
            playerCamera.forward, Vector3.up
        ).normalized;

        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 movement =
            forward * input.y + right * input.x;

        transform.position +=
            Vector3.ClampMagnitude(movement, 1f) *
            moveSpeed * Time.deltaTime;
    }
#endif
}