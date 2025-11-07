using UnityEngine;
using System;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    [Header("Gold Settings")]
    public int startGold = 1000;       //Vàng khởi đầu mỗi màn
    public int currentGold = 0;

    public event Action<int> OnGoldChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //Reset vàng về giá trị khởi đầu
    public void ResetGold()
    {
        currentGold = startGold;
        OnGoldChanged?.Invoke(currentGold);
        Debug.Log($"[GoldManager] Reset vàng về {currentGold}");
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold);
    }

    public bool TrySpend(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            OnGoldChanged?.Invoke(currentGold);
            return true;
        }
        return false;
    }
}
