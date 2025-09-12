using UnityEngine;
using UnityEngine.UI;

public class MusicSlider_Script : MonoBehaviour
{
    public Slider Music_Slider;

    public float holdSpeed = 50f; // % mỗi giây khi nhấn giữ

    private bool isIncreasing = false;
    private bool isDecreasing = false;

    void Start()
    {
        if (Music_Slider != null)
        {
            Music_Slider.minValue = 0;
            Music_Slider.maxValue = 100;
            Music_Slider.wholeNumbers = true;
            Music_Slider.value = 50; // giá trị ban đầu
        }
    }

    void Update()
    {
        if (isIncreasing)
        {
            Music_Slider.value += holdSpeed * Time.deltaTime;
        }

        if (isDecreasing)
        {
            Music_Slider.value -= holdSpeed * Time.deltaTime;
        }

        // Giữ trong khoảng 0–100
        Music_Slider.value = Mathf.Clamp(Music_Slider.value, 0, 100);
    }

    // ====== Nút cộng ======
    public void OnPlusDown()
    {
        Music_Slider.value += 1; // tăng ngay 1%
        isIncreasing = true;
    }

    public void OnPlusUp()
    {
        isIncreasing = false;
    }

    // ====== Nút trừ ======
    public void OnMinusDown()
    {
        Music_Slider.value -= 1; // giảm ngay 1%
        isDecreasing = true;
    }

    public void OnMinusUp()
    {
        isDecreasing = false;
    }
}
