using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OutlineController : MonoBehaviour
{
    [Header("Materials")]
    public Material defaultMat;
    public Material outlineMat;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (defaultMat == null)
            defaultMat = sr.material;
    }

    public void ShowOutline(bool show)
    {
        if (sr == null) return;
        sr.material = show ? outlineMat : defaultMat;
    }
}
