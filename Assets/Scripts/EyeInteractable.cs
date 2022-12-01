using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EyeInteractable : MonoBehaviour
{
    public bool IsHovered { get; private set; }

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

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalAnchor = transform.parent;
    }

    public void Hover(bool state)
    {
        IsHovered = state;
        transform.parent = originalAnchor;
    }

    public void Select(bool state, Transform anchor = null)
    {
        IsSelected = state;
        if(anchor) transform.parent = anchor;
    }

    public void ClearState()
    {
        IsHovered = IsSelected = false;
    }

    private void Update()
    {
        if(IsHovered)
        {
            OnObjectHover?.Invoke(gameObject);
            meshRenderer.material = OnHoverActiveMaterial;
        }
        else if (IsSelected)
        {
            OnObjectSelected?.Invoke(gameObject);
            meshRenderer.material = OnSelectedActiveMaterial;
        }
        else
            meshRenderer.material = OnPassiveStateMaterial;
    }
}
