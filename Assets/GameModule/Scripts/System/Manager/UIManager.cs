using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject dimBackground;
    public TowerPopupUI towerPopupUI;

    [Header("Canvas References")]
    public Canvas buildCanvas;
    public Canvas upgradeCanvas;

    [Header("Build UI")]
    public GameObject buildPanel;
    private BuildSpot currentSpot;
    public BuildTowerInfoUI buildTowerInfoUI;

    [Header("Gameplay UI")]
    public GameObject gameplayUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (dimBackground != null) dimBackground.SetActive(false);
        if (towerPopupUI != null) towerPopupUI.gameObject.SetActive(false);
        if (buildPanel != null) buildPanel.SetActive(false);

        AutoFindCanvases();
    }

    void AutoFindCanvases()
    {
        if (buildCanvas == null && buildPanel != null)
            buildCanvas = buildPanel.GetComponentInParent<Canvas>();
            
        if (upgradeCanvas == null && dimBackground != null)
            upgradeCanvas = dimBackground.GetComponentInParent<Canvas>();
    }

    public bool IsAnyPopupOpen()
    {
        bool towerPopupOpen = towerPopupUI != null && towerPopupUI.gameObject.activeSelf;
        bool buildPanelOpen = buildPanel != null && buildPanel.activeSelf;
        
        return towerPopupOpen || buildPanelOpen;
    }

    #region Tower Popup
    public void ShowTowerPopup(BaseTower tower)
    {
        if (dimBackground != null && towerPopupUI != null && tower != null)
        {
            dimBackground.SetActive(true);
            towerPopupUI.gameObject.SetActive(true);

            Canvas.ForceUpdateCanvases();
            
            // Set upgrade canvas có sort order cao hơn build canvas
            SetCanvasSortOrder(upgradeCanvas, 200);
            SetCanvasSortOrder(buildCanvas, 100);

            towerPopupUI.Show(tower);
        }
    }

    public void HideTowerPopup()
    {
        if (towerPopupUI != null) towerPopupUI.Close();
        if (dimBackground != null) dimBackground.SetActive(false);
        
        // Reset sort order về mức bình thường
        ResetCanvasSortOrder();
    }

    public void ShowDimBackground(bool show)
    {
        if (dimBackground != null) dimBackground.SetActive(show);
    }
    #endregion

    #region Build Tower Panel
    public void ShowBuildPanel(BuildSpot spot)
    {
        if (buildPanel != null)
        {
            currentSpot = spot;
            buildPanel.SetActive(true);
            ShowDimBackground(false);

            Canvas.ForceUpdateCanvases();

            SetCanvasSortOrder(buildCanvas, 200);
            SetCanvasSortOrder(upgradeCanvas, 100); 

            Time.timeScale = 0f;
        }
    }

    public void HideBuildPanel()
    {
        if (buildPanel != null) buildPanel.SetActive(false);
        currentSpot = null;
        buildTowerInfoUI.gameObject.SetActive(false);
        ShowDimBackground(false);
        
        // Reset sort order về mức bình thường
        ResetCanvasSortOrder();
        Time.timeScale = 1f;
    }

    public void OnSelectTower(TowerData data)
    {
        if (currentSpot == null) return;
        if (buildTowerInfoUI != null)
        {
            buildTowerInfoUI.Show(data, currentSpot);
        }
    
    }

    #endregion

    #region Gameplay UI
    public void ShowGameplayUI(bool show)
    {
        if (gameplayUI != null) gameplayUI.SetActive(show);
    }
    #endregion
    
    public void ShowSpellCastingUI(bool show)
    {
        if (dimBackground != null)
            dimBackground.SetActive(show);

        if (show)
        {
            SetCanvasSortOrder(upgradeCanvas, 300); 
            Time.timeScale = 0f;
        }
        else
        {
            ResetCanvasSortOrder();
            Time.timeScale = 1f;
        }
    }

    #region Canvas Management
    private void SetCanvasSortOrder(Canvas canvas, int sortOrder)
    {
        if (canvas != null)
        {
            canvas.sortingOrder = sortOrder;
            Debug.Log($"{canvas.name} sort order set to: {sortOrder}");
        }
    }

    private int GetCanvasSortOrder(Canvas canvas)
    {
        return canvas != null ? canvas.sortingOrder : -1;
    }

    private void ResetCanvasSortOrder()
    {
        // Reset về sort order mặc định
        SetCanvasSortOrder(buildCanvas, 0);
        SetCanvasSortOrder(upgradeCanvas, 0);
    }
    #endregion
}