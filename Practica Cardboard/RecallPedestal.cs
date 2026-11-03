using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallPedestal : MonoBehaviour
{
    [Tooltip("Punto sobre el pedestal donde reaparecen los cristales.")]
    public Transform spawnPoint;
    public float flightSeconds = 2.5f;
    public float arrivalRadius = 1.5f;
    public float arcHeight = 1f;

    private bool busy;
    private readonly List<Collider> disabledColliders = new List<Collider>();

    public bool CanRecall(GazeCollector collector)
    {
        if (!isActiveAndEnabled || busy || collector == null) return false;
        foreach (GameObject crystal in collector.collectedObjects)
            if (crystal != null) return true;
        return false;
    }

    public void Recall(GazeCollector collector)
    {
        if (CanRecall(collector)) StartCoroutine(Fly(collector));
    }

    private IEnumerator Fly(GazeCollector collector)
    {
        busy = true;
        List<GameObject> crystals = new List<GameObject>();
        foreach (GameObject crystal in collector.collectedObjects)
            if (crystal != null && !crystals.Contains(crystal)) crystals.Add(crystal);
        collector.collectedObjects.Clear();

        Vector3 origin = spawnPoint != null ? spawnPoint.position :
            transform.position + Vector3.up * 2f;

        // Fix arrival positions when activated, so turning the head does not
        // drag the crystals around. They remain floating near eye height.
        Vector3 center = collector.transform.position - Vector3.up * 0.35f;
        Vector3 forward = Vector3.ProjectOnPlane(
            collector.transform.forward, Vector3.up).normalized;
        if (forward.sqrMagnitude < 0.01f) forward = Vector3.forward;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3[] starts = new Vector3[crystals.Count];
        Vector3[] ends = new Vector3[crystals.Count];

        for (int i = 0; i < crystals.Count; i++)
        {
            GameObject crystal = crystals[i];
            float angle = crystals.Count == 1 ? 0f :
                Mathf.Lerp(-70f, 70f, i / (float)(crystals.Count - 1));
            float radians = angle * Mathf.Deg2Rad;
            starts[i] = origin + Vector3.up * (i * 0.25f);
            ends[i] = center + (forward * Mathf.Cos(radians) +
                right * Mathf.Sin(radians)) * arrivalRadius;

            foreach (Collider col in crystal.GetComponentsInChildren<Collider>(true))
            {
                if (!col.enabled) continue;
                disabledColliders.Add(col);
                col.enabled = false;
            }
            crystal.transform.position = starts[i];
            crystal.SetActive(true);
        }

        float duration = Mathf.Max(0.1f, flightSeconds);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smooth = t * t * (3f - 2f * t);
            for (int i = 0; i < crystals.Count; i++)
            {
                if (crystals[i] == null) continue;
                crystals[i].transform.position = Vector3.Lerp(starts[i], ends[i], smooth)
                    + Vector3.up * (Mathf.Sin(t * Mathf.PI) * arcHeight);
            }
            yield return null;
        }

        for (int i = 0; i < crystals.Count; i++)
            if (crystals[i] != null) crystals[i].transform.position = ends[i];
        RestoreColliders();
        busy = false;
        Debug.Log("Cristales recuperados: " + crystals.Count);
    }

    private void RestoreColliders()
    {
        foreach (Collider col in disabledColliders)
            if (col != null) col.enabled = true;
        disabledColliders.Clear();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        RestoreColliders();
        busy = false;
    }

    // Compatible with the sample Cardboard reticle's interactive events.
    public void OnPointerEnter() { }
    public void OnPointerExit() { }
    public void OnPointerClick() { }
}
