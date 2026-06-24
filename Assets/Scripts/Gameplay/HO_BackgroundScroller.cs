using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HO_BackgroundScroller : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float scrollSpeed = 0.35f;
    [SerializeField] private Vector2 textureScale = new Vector2(3f, 5f);

    private const string BaseMapProperty = "_BaseMap";
    private const string MainTextureProperty = "_MainTex";

    private Material targetMaterial;
    private float currentOffsetY;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
        ApplyTextureState();
    }

    private void Update()
    {
        if (targetMaterial == null)
        {
            return;
        }

        currentOffsetY = Mathf.Repeat(currentOffsetY - (scrollSpeed * Time.deltaTime), 1f);
        ApplyTextureState();
    }

    private void OnValidate()
    {
        textureScale.x = Mathf.Max(0.1f, textureScale.x);
        textureScale.y = Mathf.Max(0.1f, textureScale.y);

        if (!Application.isPlaying)
        {
            return;
        }

        ApplyTextureState();
    }

    private void Initialize()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (meshRenderer == null)
        {
            return;
        }

        targetMaterial = meshRenderer.material;
        ApplyTextureState();
    }

    private void ApplyTextureState()
    {
        if (targetMaterial == null)
        {
            return;
        }

        SetTextureScale(targetMaterial, textureScale);
        SetTextureOffset(targetMaterial, new Vector2(0f, currentOffsetY));
    }

    private static void SetTextureScale(Material material, Vector2 scale)
    {
        if (material.HasProperty(BaseMapProperty))
        {
            material.SetTextureScale(BaseMapProperty, scale);
        }

        if (material.HasProperty(MainTextureProperty))
        {
            material.SetTextureScale(MainTextureProperty, scale);
        }
    }

    private static void SetTextureOffset(Material material, Vector2 offset)
    {
        if (material.HasProperty(BaseMapProperty))
        {
            material.SetTextureOffset(BaseMapProperty, offset);
        }

        if (material.HasProperty(MainTextureProperty))
        {
            material.SetTextureOffset(MainTextureProperty, offset);
        }
    }
}
