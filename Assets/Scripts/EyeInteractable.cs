using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EyeInteractable : MonoBehaviour
{
    public bool IsHovered { get; set; }

    [SerializeField]
    private UnityEvent OnObjectHover;

    [SerializeField]
    private Material OnHoverActiveMaterial;

    [SerializeField]
    private Material OnHoverInactiveMaterial;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(IsHovered)
        {
            OnObjectHover?.Invoke();
            meshRenderer.material = OnHoverActiveMaterial;
        }
        else
        {
            meshRenderer.material = OnHoverInactiveMaterial;
        }
    }
}
