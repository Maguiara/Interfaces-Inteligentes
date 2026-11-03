using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TapMovement : MonoBehaviour
{
    public Transform playerCamera;
    public LayerMask walkable;
    public float maxDistance = 8f;
    public float maxSlope = 35f;

    private CharacterController controller;
    private float verticalSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (playerCamera == null) return;
        if (controller.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -2f;
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        controller.Move(Vector3.up * verticalSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (!TapPressed() || !TryGetDestination(out Vector3 destination))
            return;

        controller.enabled = false;
        transform.position = destination;
        controller.enabled = true;
        verticalSpeed = 0f;
    }

    // Shared by movement and reticle: range, visibility, layer, slope and space.
    // Assumes an upright Player with scale (1,1,1), and its origin at its feet.
    public bool TryGetDestination(out Vector3 destination)
    {
        destination = default;
        if (!isActiveAndEnabled || playerCamera == null ||
            controller == null || !controller.enabled)
            return false;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            return false;

        if ((walkable.value & (1 << hit.collider.gameObject.layer)) == 0 ||
            Vector3.Angle(hit.normal, Vector3.up) > maxSlope)
            return false;

        destination = hit.point + Vector3.up * 0.05f;
        Vector3 center = destination + transform.rotation * controller.center;
        float radius = controller.radius;
        float halfSegment = Mathf.Max(0f, controller.height * 0.5f - radius);

        Collider[] obstacles = Physics.OverlapCapsule(
            center - Vector3.up * halfSegment,
            center + Vector3.up * halfSegment,
            radius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider obstacle in obstacles)
        {
            if (!obstacle.transform.IsChildOf(transform))
                return false;
        }
        return true;
    }

    private bool TapPressed()
    {
#if UNITY_EDITOR
        return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
#else
        return Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
#endif
    }
}
