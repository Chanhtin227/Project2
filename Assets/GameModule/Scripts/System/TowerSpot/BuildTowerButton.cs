using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildTowerButton : MonoBehaviour
{
    public TowerData towerData;
    public Button button;
    public TextMeshProUGUI costText;

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

        // Gắn sự kiện click
        button.onClick.AddListener(() =>
        {
            UIManager.Instance.OnSelectTower(towerData);
        });
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
}
