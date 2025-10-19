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
            // dùng unscaledTime để bỏ qua Time.timeScale
            float alpha = (Mathf.Sin(Time.unscaledTime * speed) + 1f) / 2f;
            Color c = img.color;
            c.a = Mathf.Lerp(0.1f, 1f, alpha); // nhấp nháy giữa 10% và 100% sáng
            img.color = c;
        }
    }
}
