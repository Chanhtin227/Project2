using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildTowerInfoUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI costText;
    public Button placeButton;

    private TowerData currentTower;
    private BuildSpot currentSpot;

    public void Show(TowerData tower, BuildSpot spot)
    {
        currentTower = tower;
        currentSpot = spot;

        var lvl = tower.levels[0];
        if (icon) icon.sprite = tower.icon;
        if (nameText) nameText.text = tower.towerName;
        if (damageText) damageText.text = $"DMG: {lvl.damage}";
        if (rangeText) rangeText.text = $"Range: {lvl.range}";
        if (fireRateText) fireRateText.text = $"Fire rate: {lvl.fireRate}";
        if (healthText) healthText.text = $"HP: {lvl.maxHealth}";
        if (costText) costText.text = $"Cost: {lvl.cost}";

        gameObject.SetActive(true);

        placeButton.onClick.RemoveAllListeners();
        placeButton.onClick.AddListener(PlaceTower);
    }

    private void PlaceTower()
    {
        if (currentTower == null || currentSpot == null) return;

        int cost = currentTower.levels[0].cost;
        if (GoldManager.Instance != null && GoldManager.Instance.TrySpend(cost))
        {
            currentSpot.PlaceTower(currentTower.prefab);
            UIManager.Instance.HideBuildPanel();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Không đủ vàng để xây!");
        }
    }
}
