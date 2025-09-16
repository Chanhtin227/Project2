using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    [System.Serializable]
    public class TowerLevel
    {
        public string name = "Level 1";
        public float damage = 10f;
        public float range = 3f;
        public float fireRate = 1f;
        public int cost = 50;
        public float maxHealth = 100f; // 🆕 Máu cho tower
    }

    [Header("Tower Info")]
    public string towerName = "Basic Tower";
    public Sprite icon;
    public GameObject prefab; // prefab trụ trong game
    public TowerLevel[] levels;
}
