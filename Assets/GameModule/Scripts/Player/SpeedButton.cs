using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    public Button speedButton;
    public Image iconImage; 
    public float speedMultiplier = 2f;

    private bool isFast = false;

    private void Start()
    {
        if (speedButton != null)
            speedButton.onClick.AddListener(ToggleSpeed);
    }

    private void ToggleSpeed()
    {
        if (isFast)
        {
            Time.timeScale = 1f;
            isFast = false;
            SetIconAlpha(1f);   // icon sáng lại
        }
        else
        {
            Time.timeScale = speedMultiplier;
            isFast = true;
            SetIconAlpha(0.5f); // icon mờ đi
        }
    }

    private void SetIconAlpha(float alpha)
    {
        if (iconImage != null)
        {
            Color c = iconImage.color;
            c.a = alpha;
            iconImage.color = c;
        }
    }
}
