using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildTowerButton : MonoBehaviour
{
    public TowerData towerData;
    public Button button;
    public TextMeshProUGUI costText;
    // private float lastClickTime = 0f;
    // private float doubleClickThreshold = 0.3f; // ngưỡng thời gian giữa 2 lần click để tính là double click

    private int towerCost;

    void Start()
    {
        if (button == null) button = GetComponent<Button>();

        // Lấy cost của level 1
        if (towerData != null && towerData.levels.Length > 0)
        {
            towerCost = towerData.levels[0].cost;
            if (costText != null) costText.text = $"Cost: {towerCost}";
        }

        button.onClick.AddListener(onClick);
    }

    void Update()
    {
        // Kiểm tra vàng hiện tại
        if (GoldManager.Instance != null)
        {
            if (GoldManager.Instance.currentGold < towerCost)
                button.interactable = false;
            else
                button.interactable = true;
        }
    }

    private void onClick()
    {
        UIManager.Instance.OnSelectTower(towerData);
    }
}
