using UnityEngine;

// Runs after the sample reticle and TapMovement have updated.
[DefaultExecutionOrder(100)]
[RequireComponent(typeof(Renderer))]
public class ReticleMovementFeedback : MonoBehaviour
{
    public TapMovement movement;
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;

    private Renderer reticleRenderer;
    private MaterialPropertyBlock properties;
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    private void Awake()
    {
        reticleRenderer = GetComponent<Renderer>();
        properties = new MaterialPropertyBlock();
    }

    private void LateUpdate()
    {
        bool valid = movement != null &&
            movement.TryGetDestination(out _);

        reticleRenderer.GetPropertyBlock(properties);
        properties.SetColor(ColorId, valid ? validColor : invalidColor);
        reticleRenderer.SetPropertyBlock(properties);
    }

    private void OnDisable()
    {
        if (reticleRenderer != null)
            reticleRenderer.SetPropertyBlock(null);
    }
}
