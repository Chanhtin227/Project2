using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerRangeManager : MonoBehaviour
{
    public static TowerRangeManager Instance { get; private set; }

    private List<BaseTower> towers = new List<BaseTower>();
    private bool showRanges = true; // Mặc định bật hiển thị

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleRanges();
        }
    }


    public void RegisterTower(BaseTower tower)
    {
        if (!towers.Contains(tower))
            towers.Add(tower);
            Debug.Log($"[TowerRangeManager] Tower registered: {tower.name} (Total={towers.Count})");
    }

    public void UnregisterTower(BaseTower tower)
    {
        if (towers.Contains(tower))
            towers.Remove(tower);
    }

    public void ToggleRanges()
    {
        showRanges = !showRanges;
        foreach (var tower in towers)
        {
            if (tower != null)
                tower.SetRangeVisible(showRanges);
        }

        Debug.Log($"[TowerRangeManager] Hiển thị tầm đánh: {showRanges}");
    }

    public void SetRangesVisible(bool visible)
    {
        showRanges = visible;
        foreach (var tower in towers)
        {
            if (tower != null)
                tower.SetRangeVisible(visible);
        }
    }
}
