using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    [Header("Loading Bar Elements")]
    [SerializeField] private RectTransform fillBar;
    [SerializeField] private RectTransform runner;
    [SerializeField] private Text percentText; // Hiển thị %
    [SerializeField] private float maxWidth = 1500f;

    [Header("Behavior Settings")]
    [SerializeField] private bool loop = true; // ⚡ Có lặp lại sau khi đầy hay không
    [SerializeField] private float fillSpeed = 0.2f; // tốc độ tăng thanh

    private float progress = 0f;
    private bool canStart = false;

    IEnumerator Start()
    {
        // Bắt đầu từ 0%
        UpdateBar(0f);

        // Đợi 1 frame để tránh lỗi layout chưa sẵn sàng
        yield return null;
        canStart = true;
    }

    void Update()
    {
        if (!canStart) return;

        // Tăng progress
        progress += Time.unscaledDeltaTime * fillSpeed;

        if (progress > 1f)
        {
            if (loop)
                progress = 0f; // Nếu loop → reset lại
            else
                progress = 1f; // Nếu không loop → giữ ở 100%
        }

        UpdateBar(progress);
    }

    public void UpdateBar(float value)
    {
        value = Mathf.Clamp01(value);
        fillBar.sizeDelta = new Vector2(maxWidth * value, fillBar.sizeDelta.y);

        // Cập nhật runner (nếu có)
        if (runner != null)
        {
            runner.anchoredPosition = new Vector2(maxWidth * value, runner.anchoredPosition.y);
        }

        // Cập nhật text %
        if (percentText != null)
        {
            percentText.text = Mathf.RoundToInt(value * 100f) + "%";
        }
    }

    // ⚙️ Gọi hàm này từ script khác nếu muốn bật/tắt loop khi đang chạy
    public void SetLoop(bool enable)
    {
        loop = enable;
    }
}
