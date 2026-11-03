using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SelectionColor : MonoBehaviour
{
    public Color selectedColor = Color.green;

    private Material instanceMaterial;
    private Color originalColor;
    private int colorProperty;

    private void Awake()
    {
        instanceMaterial = GetComponent<Renderer>().material;

        colorProperty = instanceMaterial.HasProperty("_BaseColor")
            ? Shader.PropertyToID("_BaseColor")
            : Shader.PropertyToID("_Color");

        originalColor = instanceMaterial.GetColor(colorProperty);
    }

    public void Select()
    {
        instanceMaterial.SetColor(colorProperty, selectedColor);
    }

    public void Deselect()
    {
        instanceMaterial.SetColor(colorProperty, originalColor);
    }

    private void OnDestroy()
    {
        if (instanceMaterial != null)
            Destroy(instanceMaterial);
    }
}