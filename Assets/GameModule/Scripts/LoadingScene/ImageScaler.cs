using UnityEngine;

public class ImageScaler : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private float startScale = 0.5f;     // Bắt đầu từ 0.5x
    [SerializeField] private float endScale = 2f;         // Kết thúc ở 2x
    [SerializeField] private float duration = 2f;         // Thời gian để scale hết (giây)
    [SerializeField] private bool loop = false;            // Có lặp lại hay không
    [SerializeField] private bool pingPong = false;        // Có scale tới rồi scale ngược lại không

    private float timer = 0f;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = Vector3.one * startScale;
        transform.localScale = initialScale;
    }

    void Update()
    {
        if (duration <= 0f) return;

        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / duration);
        float currentScale = Mathf.Lerp(startScale, endScale, t);
        transform.localScale = Vector3.one * currentScale;

        // Nếu hoàn tất
        if (timer >= duration)
        {
            if (pingPong)
            {
                // Đảo hướng
                float temp = startScale;
                startScale = endScale;
                endScale = temp;
                timer = 0f;
            }
            else if (loop)
            {
                timer = 0f;
                transform.localScale = Vector3.one * startScale;
            }
        }
    }
}
