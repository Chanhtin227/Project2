using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    void Start()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(GoldManager.Instance.currentGold); // update ban đầu
        }
    }

    void UpdateGoldUI(int gold)
    {
        if (goldText != null)
            goldText.text = $"{gold}";
    }

    void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= UpdateGoldUI;
    }
}
