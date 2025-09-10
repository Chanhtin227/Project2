using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [Header("UI References")]
    public Slider slider;
    public Button plusButton;
    public Button minusButton;

    [Header("Settings")]
    public float holdDelay = 0.3f;  // Thời gian chờ trước khi auto-repeat
    public float repeatRate = 0.05f; // Tốc độ lặp khi giữ nút

    private bool isHoldingPlus = false;
    private bool isHoldingMinus = false;
    private float holdTimer = 0f;

    void Start()
    {
        // Setup slider
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.wholeNumbers = true;

        // Gán sự kiện click
        plusButton.onClick.AddListener(IncreaseOnce);
        minusButton.onClick.AddListener(DecreaseOnce);
    }

    void Update()
    {
        if (isHoldingPlus)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDelay)
            {
                slider.value = Mathf.Min(slider.value + (Time.deltaTime / repeatRate), slider.maxValue);
            }
        }

        if (isHoldingMinus)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDelay)
            {
                slider.value = Mathf.Max(slider.value - (Time.deltaTime / repeatRate), slider.minValue);
            }
        }
    }

    private void IncreaseOnce()
    {
        slider.value = Mathf.Min(slider.value + 1, slider.maxValue);
    }

    private void DecreaseOnce()
    {
        slider.value = Mathf.Max(slider.value - 1, slider.minValue);
    }

    // Gọi khi bắt đầu giữ nút
    public void OnPlusDown()
    {
        isHoldingPlus = true;
        holdTimer = 0f;
    }

    public void OnPlusUp()
    {
        isHoldingPlus = false;
    }

    public void OnMinusDown()
    {
        isHoldingMinus = true;
        holdTimer = 0f;
    }

    public void OnMinusUp()
    {
        isHoldingMinus = false;
    }
}
