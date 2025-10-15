using UnityEngine;
using UnityEngine.UI;

public class ArrowFlash : MonoBehaviour
{
    public float speed = 2f; // tốc độ nhấp nháy
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (img != null)
        {
            float alpha = (Mathf.Sin(Time.time * speed) + 1f) / 2f; // 0 → 1 → 0
            Color c = img.color;
            c.a = Mathf.Lerp(0.1f, 1f, alpha); // nhấp nháy giữa 40% và 100% sáng
            img.color = c;
        }
    }
}
