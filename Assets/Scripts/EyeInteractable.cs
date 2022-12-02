using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EyeInteractable : MonoBehaviour
{
    [field: SerializeField]
    public bool IsHovered { get; private set; }

    [field: SerializeField]
    public bool IsSelected { get; private set; }

    [SerializeField]
    private UnityEvent<GameObject> OnObjectHover;

    [SerializeField]
    private UnityEvent<GameObject> OnObjectSelected;

    [SerializeField]
    private Material OnHoverActiveMaterial;

    [SerializeField]
    private Material OnSelectedActiveMaterial;

    [SerializeField]
    private Material OnPassiveStateMaterial;

    private MeshRenderer meshRenderer;

    private Transform originalAnchor;

    private TextMeshPro statusText;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        statusText = GetComponentInChildren<TextMeshPro>();
        originalAnchor = transform.parent;
    }

    public void Hover(bool state)
    {
        IsHovered = state;
    }

    public void Select(bool state, Transform anchor = null)
    {
        IsSelected = state;
        if(anchor) transform.SetParent(anchor);
        if(!IsSelected) transform.SetParent(originalAnchor);
    }

    private void Update()
    {
        if(IsHovered)
        {
            OnObjectHover?.Invoke(gameObject);
            meshRenderer.material = OnHoverActiveMaterial;
            if (statusText) statusText.text = "<color=\"yellow\">HOVERED</color>";
        }
        if (IsSelected)
        {
            OnObjectSelected?.Invoke(gameObject);
            meshRenderer.material = OnSelectedActiveMaterial;
            if (statusText) statusText.text = "<color=\"green\">SELECTED</color>";
        }
        if (!IsHovered && !IsSelected)
        {
            meshRenderer.material = OnPassiveStateMaterial;
            if (statusText) statusText.text = "<color=\"grey\">IDLE</color>";
        }
    }
}
