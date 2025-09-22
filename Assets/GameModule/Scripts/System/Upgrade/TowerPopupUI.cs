using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerPopupUI : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public TextMeshProUGUI sellText;
    public Button upgradeButton;
    public Button closeButton;
    public Button sellButton;

    private BaseTower currentTower;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        if (sellButton != null)
        sellButton.onClick.AddListener(OnSellClicked);
        closeButton.onClick.AddListener(Close);
    }

    void Update()
    {
        // Nếu popup đang mở thì update text liên tục
        if (gameObject.activeSelf && currentTower != null)
        {
            UpdateUI();
        }
    }


    public void Show(BaseTower tower)
    {
        currentTower = tower;
        Debug.Log("TowerPopupUI.Show được gọi với: " + tower.name);
        UpdateUI();

        if (cam == null) cam = Camera.main;
        if (cam == null) cam = FindAnyObjectByType<Camera>();
        Canvas parentCanvas = GetComponentInParent<Canvas>();

        if (parentCanvas != null && parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(tower.transform.position);
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                screenPoint,
                parentCanvas.worldCamera,
                out localPoint
            );

            transform.localPosition = localPoint + new Vector2(0, 80f);
        }
        else
        {
            Vector3 screenPos = cam.WorldToScreenPoint(tower.transform.position);
            transform.position = screenPos + new Vector3(0, 80f, 0);
        }

        Time.timeScale = 0.2f;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        currentTower = null;
        UIManager.Instance.ShowDimBackground(false);
        Time.timeScale = 1f;
    }

    private void UpdateUI()
    {
        if (currentTower == null) return;

        var data = currentTower.data;
        int lvl = currentTower.currentLevel;
        
        // Nếu còn level để nâng cấp
        if (lvl + 1 < data.levels.Length)
        {
            int cost = data.levels[lvl + 1].cost;
            costText.text = $"-{cost}";
            // disable nếu không đủ tiền
            if (GoldManager.Instance != null && GoldManager.Instance.currentGold < cost)
                upgradeButton.interactable = false;
            else
                upgradeButton.interactable = true;
        }
        else
        {
            costText.text = "Max";
            upgradeButton.interactable = false;
        }

        if (sellButton != null)
        {
            int refund = currentTower.GetRefund();
            sellText.text = $"+{refund}";
        }
    }

    private void OnUpgradeClicked()
    {
        if (currentTower != null)
        {
            int nextCost = 0;
            if (currentTower.currentLevel + 1 < currentTower.data.levels.Length)
                nextCost = currentTower.data.levels[currentTower.currentLevel + 1].cost;

            if (GoldManager.Instance != null && !GoldManager.Instance.TrySpend(nextCost))
            {
                Debug.Log("Not enough gold to upgrade!");
                upgradeButton.interactable = false;
                return;
            }
            currentTower.Upgrade();
            UpdateUI();
        }
    }
    private void OnSellClicked()
    {
        Debug.Log("Sell button clicked!");
        if (currentTower != null)
        {
            currentTower.Sell();
            Close();
        }
    }
}
