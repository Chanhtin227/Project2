using UnityEngine;
using UnityEngine.UI; // Dùng Text legacy

public class LoadingDots : MonoBehaviour
{
    public Text loadingText;   // Kéo Text legacy vào đây trong Inspector
    private int dotCount = 0;
    private bool increasing = true;

    void Start()
    {
        InvokeRepeating(nameof(UpdateLoadingText), 0f, 0.3f); // Gọi mỗi 0.3 giây
    }

    void UpdateLoadingText()
    {
        // Tăng hoặc giảm số chấm
        if (increasing)
        {
            dotCount++;
            if (dotCount >= 3)
                increasing = false;
        }
        else
        {
            dotCount--;
            if (dotCount <= 1)
                increasing = true;
        }

        // Tạo chuỗi dấu chấm và gán vào Text
        string dots = new string('.', dotCount);
        loadingText.text = "Loading" + dots;
    }
}
