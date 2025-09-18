using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int startingGold = 500;
    public int currentGold;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        currentGold = startingGold;
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0) return true;
        if (currentGold >= amount)
        {
            currentGold -= amount;
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
    }
}
