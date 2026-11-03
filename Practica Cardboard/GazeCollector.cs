using System.Collections.Generic;
using UnityEngine;

public class GazeCollector : MonoBehaviour
{
    public LayerMask collectibleLayer;
    public float maxDistance = 3f;
    public float holdSeconds = 5f;
    public List<GameObject> collectedObjects = new List<GameObject>();

    private GameObject currentTarget;
    private float gazeTime;
    public float GazeProgress => currentTarget == null ? 0f :
        Mathf.Clamp01(gazeTime / Mathf.Max(holdSeconds, 0.01f));

    private void Update()
    {
        GameObject target = null;
        RecallPedestal pedestal = null;
        if (Physics.Raycast(transform.position, transform.forward,
            out RaycastHit hit, maxDistance, Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            pedestal = hit.collider.GetComponentInParent<RecallPedestal>();
            if (pedestal != null)
            {
                if (pedestal.CanRecall(this)) target = pedestal.gameObject;
            }
            else if ((collectibleLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                target = hit.collider.gameObject;
        }

        if (target != currentTarget)
        {
            currentTarget = target;
            gazeTime = 0f;
        }
        if (currentTarget == null) return;

        gazeTime += Time.deltaTime;
        if (gazeTime < Mathf.Max(holdSeconds, 0.01f)) return;

        GameObject selected = currentTarget;
        currentTarget = null;
        gazeTime = 0f;
        if (pedestal != null)
        {
            pedestal.Recall(this);
            return;
        }

        if (!collectedObjects.Contains(selected)) collectedObjects.Add(selected);
        selected.SetActive(false);
        Debug.Log("Cristal recogido. Total: " + collectedObjects.Count);
    }

    private void OnDisable()
    {
        currentTarget = null;
        gazeTime = 0f;
    }
}
