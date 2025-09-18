using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject dimBackground;
    public TowerPopupUI towerPopupUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        if (dimBackground != null) dimBackground.SetActive(false);
        if (towerPopupUI != null) towerPopupUI.gameObject.SetActive(false);
    }

    public void ShowTowerPopup(BaseTower tower)
{
    
    if (dimBackground != null && towerPopupUI != null && tower != null)
    {
        dimBackground.SetActive(true);
        towerPopupUI.gameObject.SetActive(true);
        
        Canvas.ForceUpdateCanvases();
        towerPopupUI.Show(tower);
        
        dimBackground.transform.SetAsFirstSibling();
        towerPopupUI.transform.SetAsLastSibling();
    }
}

    public void HideTowerPopup()
    {
        if (towerPopupUI != null) towerPopupUI.Close();
        if (dimBackground != null) dimBackground.SetActive(false);
    }

    public void ShowDimBackground(bool show)
    {
        if (dimBackground != null) dimBackground.SetActive(show);
    }
}
