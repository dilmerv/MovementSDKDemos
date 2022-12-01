using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EyeTrackingRay : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 1.0f;

    [SerializeField]
    private float rayWidth = 0.01f;

    [SerializeField]
    private LayerMask layersToInclude;

    [SerializeField]
    private Color rayColorDefaultState = Color.yellow;

    [SerializeField]
    private Color rayColorHoverState = Color.red;

    [SerializeField]
    private OVRHand handUsedForPinchSelection;

    [SerializeField]
    private bool mockPinch;

    private bool allowPinchSelection;

    private bool rayIntercepting;

    private LineRenderer lineRenderer;

    private Dictionary<int, EyeInteractable> interactables = new Dictionary<int, EyeInteractable>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        allowPinchSelection = handUsedForPinchSelection != null;
        SetupRay();
    }

    private void SetupRay()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColorDefaultState;
        lineRenderer.endColor = rayColorDefaultState;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistance));
    }

    private void Update()
    {
        if (IsPinching())
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
        }

        if(!rayIntercepting)
        {
            lineRenderer.startColor = lineRenderer.endColor = rayColorDefaultState;
            lineRenderer.SetPosition(1, new Vector3(0, 0, transform.position.z + rayDistance));
            ClearStates();
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.TransformDirection(Vector3.forward) * rayDistance;

        // Check if eye ray intersects with any objects included in the layersToInclude
        rayIntercepting = Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, layersToInclude);
        if (rayIntercepting)
        {
            ClearStates();

            lineRenderer.startColor = rayColorHoverState;
            lineRenderer.endColor = rayColorHoverState;
            var eyeInteractable = hit.transform.GetComponent<EyeInteractable>();
            var toLocalSpace = transform.InverseTransformPoint(eyeInteractable.transform.position);
            lineRenderer.SetPosition(1, new Vector3(0, 0, toLocalSpace.z));
            if (!interactables.ContainsKey(eyeInteractable.GetHashCode()))
            {
                interactables.Add(eyeInteractable.GetHashCode(), eyeInteractable);
            }
            eyeInteractable.Hover(true);
        }
    }

    private void ClearStates()
    {
        foreach (var interactable in interactables)
        {
            interactable.Value.Hover(false);
        }
    }

    private void OnDestroy()
    {
        interactables.Clear();
    }

    private bool IsPinching()
    {
        return (allowPinchSelection && handUsedForPinchSelection.GetFingerIsPinching(OVRHand.HandFinger.Index)) || mockPinch;
    }
}
