#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class VegetationScatterWindow : EditorWindow
{
    [SerializeField] private GameObject[] prefabs = new GameObject[0];
    [SerializeField] private Collider[] surfaces = new Collider[0];
    [SerializeField] private int amount = 60;
    [SerializeField] private float spacing = 1.5f;
    [SerializeField] private Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    [SerializeField] private float maxSlope = 30f;
    [SerializeField] private float baseOffset = -0.02f;
    [SerializeField] private int seed = 123;
    private string result = "";

    [MenuItem("Tools/Cardboard/Distribuir vegetacion")]
    public static void Open()
    {
        GetWindow<VegetationScatterWindow>("Vegetacion");
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "Selecciona los Ground en la Hierarchy y pulsa Capturar suelos. " +
            "Se plantara sobre sus colliders Walkable, dentro de sus limites. " +
            "Cada generacion crea un grupo nuevo y admite Ctrl+Z.", MessageType.Info);

        SerializedObject settings = new SerializedObject(this);
        settings.Update();
        EditorGUILayout.PropertyField(settings.FindProperty("prefabs"),
            new GUIContent("Prefabs de plantas"), true);
        EditorGUILayout.PropertyField(settings.FindProperty("surfaces"),
            new GUIContent("Colliders de suelo"), true);
        settings.ApplyModifiedProperties();

        if (GUILayout.Button("Capturar suelos seleccionados"))
        {
            HashSet<Collider> selected = new HashSet<Collider>();
            foreach (GameObject obj in Selection.gameObjects)
                foreach (Collider col in obj.GetComponentsInChildren<Collider>())
                    if (col.enabled && !col.isTrigger && obj.scene.IsValid())
                        selected.Add(col);
            surfaces = new List<Collider>(selected).ToArray();
        }

        amount = EditorGUILayout.IntSlider("Cantidad", amount, 1, 500);
        spacing = Mathf.Max(0.1f, EditorGUILayout.FloatField("Separacion minima", spacing));
        scaleRange = EditorGUILayout.Vector2Field("Escala min / max", scaleRange);
        maxSlope = EditorGUILayout.Slider("Pendiente maxima", maxSlope, 0f, 60f);
        baseOffset = EditorGUILayout.FloatField("Ajuste vertical", baseOffset);
        seed = EditorGUILayout.IntField("Semilla aleatoria", seed);

        using (new EditorGUI.DisabledScope(EditorApplication.isPlayingOrWillChangePlaymode))
            if (GUILayout.Button("Generar vegetacion")) Generate();

        if (!string.IsNullOrEmpty(result))
            EditorGUILayout.HelpBox(result, MessageType.Info);
    }

    private void Generate()
    {
        int walkable = LayerMask.NameToLayer("Walkable");
        List<GameObject> usablePrefabs = new List<GameObject>();
        foreach (GameObject prefab in prefabs)
            if (prefab != null && PrefabUtility.IsPartOfPrefabAsset(prefab))
                usablePrefabs.Add(prefab);

        HashSet<Collider> validSurfaces = new HashSet<Collider>();
        Bounds bounds = new Bounds();
        bool hasBounds = false;
        Scene scene = SceneManager.GetActiveScene();
        foreach (Collider surface in surfaces)
        {
            if (surface == null || !surface.enabled || surface.isTrigger ||
                !surface.gameObject.activeInHierarchy ||
                surface.gameObject.layer != walkable || surface.gameObject.scene != scene)
                continue;
            validSurfaces.Add(surface);
            if (!hasBounds) { bounds = surface.bounds; hasBounds = true; }
            else bounds.Encapsulate(surface.bounds);
        }

        if (walkable < 0 || usablePrefabs.Count == 0 || !hasBounds ||
            scaleRange.x <= 0f || scaleRange.y < scaleRange.x)
        {
            EditorUtility.DisplayDialog("Revisa la configuracion",
                "Necesitas prefabs del panel Project, suelos con collider en la capa " +
                "Walkable de la escena activa y escalas positivas (min <= max).", "Aceptar");
            return;
        }

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Distribuir vegetacion");
        GameObject group = new GameObject("Vegetacion_Generada");
        Undo.RegisterCreatedObjectUndo(group, "Crear grupo de vegetacion");
        List<Vector3> points = new List<Vector3>();
        System.Random random = new System.Random(seed);
        int attempts = 0;
        int maxAttempts = amount * 100;
        Physics.SyncTransforms();

        try
        {
            while (points.Count < amount && attempts++ < maxAttempts)
            {
                Vector3 origin = new Vector3(
                    Mathf.Lerp(bounds.min.x, bounds.max.x, (float)random.NextDouble()),
                    bounds.max.y + 20f,
                    Mathf.Lerp(bounds.min.z, bounds.max.z, (float)random.NextDouble()));

                RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down,
                    bounds.size.y + 40f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
                RaycastHit? candidate = null;
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.transform.IsChildOf(group.transform)) continue;
                    // Obstacles above the selected ground prevent planting there.
                    if (validSurfaces.Contains(hit.collider)) candidate = hit;
                    break;
                }
                if (!candidate.HasValue) continue;
                RaycastHit ground = candidate.Value;
                if (Vector3.Angle(ground.normal, Vector3.up) > maxSlope) continue;

                bool tooClose = false;
                foreach (Vector3 point in points)
                {
                    Vector2 delta = new Vector2(point.x - ground.point.x,
                        point.z - ground.point.z);
                    if (delta.sqrMagnitude < spacing * spacing)
                    { tooClose = true; break; }
                }
                if (tooClose) continue;

                GameObject prefab = usablePrefabs[random.Next(usablePrefabs.Count)];
                GameObject plant = (GameObject)PrefabUtility.InstantiatePrefab(
                    prefab, group.transform);
                Undo.RegisterCreatedObjectUndo(plant, "Crear planta");
                plant.transform.position = ground.point;
                plant.transform.rotation = Quaternion.Euler(0f,
                    (float)random.NextDouble() * 360f, 0f);
                plant.transform.localScale = prefab.transform.localScale * Mathf.Lerp(
                    scaleRange.x, scaleRange.y, (float)random.NextDouble());

                // Position the visible base on the mesh even with an offset pivot.
                float minY = float.PositiveInfinity;
                foreach (Renderer renderer in plant.GetComponentsInChildren<Renderer>())
                    if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
                        minY = Mathf.Min(minY, renderer.bounds.min.y);
                if (!float.IsInfinity(minY))
                    plant.transform.position += Vector3.up *
                        (ground.point.y - minY + baseOffset);
                else plant.transform.position += Vector3.up * baseOffset;

                // Plants must not become teleport destinations, even if a prefab
                // was accidentally saved on Walkable.
                foreach (Transform child in plant.GetComponentsInChildren<Transform>(true))
                    if (child.gameObject.layer == walkable) child.gameObject.layer = 0;

                points.Add(ground.point);
            }

            if (points.Count == 0) Undo.DestroyObjectImmediate(group);
            else Selection.activeGameObject = group;
            EditorSceneManager.MarkSceneDirty(scene);
            result = "Creadas " + points.Count + " de " + amount +
                " plantas. Si faltan, reduce la separacion o revisa suelos y obstaculos. " +
                "Ctrl+Z deshace esta generacion.";
        }
        finally
        {
            Undo.CollapseUndoOperations(undoGroup);
        }
    }
}
#endif
