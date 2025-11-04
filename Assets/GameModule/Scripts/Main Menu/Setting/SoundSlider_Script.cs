using UnityEngine;
using UnityEngine.UI;

public class SoundSlider_Script : MonoBehaviour
{
    public Slider Sound_Slider;

    public float holdSpeed = 50f; // % mỗi giây khi nhấn giữ

    private bool isIncreasing = false;
    private bool isDecreasing = false;

    void Start()
    {
        if (Sound_Slider != null)
        {
            Sound_Slider.minValue = 0;
            Sound_Slider.maxValue = 100;
            Sound_Slider.wholeNumbers = true;
            Sound_Slider.value = 100;
        }
        Debug.Log("[SoundSlider] Start: slider assigned? " + (Sound_Slider != null));
    }

    void Update()
    {
        if (Sound_Slider == null) return;

        if (isIncreasing)
        {
            Sound_Slider.value += holdSpeed * Time.deltaTime;
        }

        if (isDecreasing)
        {
            Sound_Slider.value -= holdSpeed * Time.deltaTime;
        }

        Sound_Slider.value = Mathf.Clamp(Sound_Slider.value, 0, 100);
    }

    public void OnPlusDown()
    {
        Debug.Log("[SoundSlider] OnPlusDown called");
        if (Sound_Slider == null) { Debug.LogWarning("[SoundSlider] slider NULL in OnPlusDown"); return; }
        Sound_Slider.value = Mathf.Clamp(Sound_Slider.value + 10, 0, 100);
        isIncreasing = true;
    }

    public void OnPlusUp()
    {
        Debug.Log("[SoundSlider] OnPlusUp called");
        isIncreasing = false;
    }

    public void OnMinusDown()
    {
        Debug.Log("[SoundSlider] OnMinusDown called");
        if (Sound_Slider == null) { Debug.LogWarning("[SoundSlider] slider NULL in OnMinusDown"); return; }
        Sound_Slider.value = Mathf.Clamp(Sound_Slider.value - 10, 0, 100);
        isDecreasing = true;
    }

    public void OnMinusUp()
    {
        Debug.Log("[SoundSlider] OnMinusUp called");
        isDecreasing = false;
    }
}





