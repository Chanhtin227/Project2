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
    private RectTransform rectTransform;
    
    [Header("Popup Settings")]
    [SerializeField] private Vector2 offset = new Vector2(0, 80f); // Khoảng cách từ tower
    [SerializeField] private float screenPadding = 20f; // Padding từ cạnh màn hình

    void Start()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        if (sellButton != null)
            sellButton.onClick.AddListener(OnSellClicked);
        closeButton.onClick.AddListener(Close);
    }

    void Update()
    {
        if (gameObject.activeSelf && currentTower != null)
        {
            UpdateUI();
            UpdatePosition();
        }
    }

    public void Show(BaseTower tower)
    {
        currentTower = tower;
        Debug.Log("TowerPopupUI.Show được gọi với: " + tower.name);
        UpdateUI();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (currentTower == null || rectTransform == null) return;
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Canvas parentCanvas = GetComponentInParent<Canvas>();
        
        if (parentCanvas != null)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(currentTower.transform.position);
            Vector2 localPoint;
            
            if (parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.transform as RectTransform,
                    screenPoint,
                    parentCanvas.worldCamera,
                    out localPoint
                );
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.transform as RectTransform,
                    screenPoint,
                    null,
                    out localPoint
                );
            }

            Vector2 desiredPosition = localPoint + offset;
            desiredPosition = ClampToScreen(desiredPosition, parentCanvas);
            transform.localPosition = desiredPosition;
        }
    }

    private Vector2 ClampToScreen(Vector2 position, Canvas canvas)
    {
        if (rectTransform == null || canvas == null) return position;

        RectTransform canvasRect = canvas.transform as RectTransform;
        if (canvasRect == null) return position;

        Vector2 popupSize = rectTransform.rect.size;
        
        Vector2 canvasSize = canvasRect.rect.size;
        
        float minX = -canvasSize.x / 2f + popupSize.x / 2f + screenPadding;
        float maxX = canvasSize.x / 2f - popupSize.x / 2f - screenPadding;
        float minY = -canvasSize.y / 2f + popupSize.y / 2f + screenPadding;
        float maxY = canvasSize.y / 2f - popupSize.y / 2f - screenPadding;

        // Clamp position
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector2(clampedX, clampedY);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        currentTower = null;
        UIManager.Instance.ShowDimBackground(false);
    }

    private void UpdateUI()
    {
        if (currentTower == null) return;

        var data = currentTower.data;
        int lvl = currentTower.currentLevel;
        
        if (lvl + 1 < data.levels.Length)
        {
            int cost = data.levels[lvl + 1].cost;
            costText.text = $"-{cost}";
            
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