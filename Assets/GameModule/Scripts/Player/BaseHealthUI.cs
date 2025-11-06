using UnityEngine;
using UnityEngine.UI;

public class BaseHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (GameManager.Instance == null || healthFill == null) return;

        float fillAmount = (float)GameManager.Instance.baseHealth / GameManager.Instance.maxBaseHealth;
        healthFill.fillAmount = Mathf.Clamp01(fillAmount);
    }
}
