using UnityEngine;

public enum EnemySpecialType
{
    None,
    AttackTower,   
    PoisonTower,  
    BuffSpeed,       
    BuffArmor,       
    HealOnDeath,    
    LowHpUpSpeed,        
    MagicOnlyDame,      
    Boss,  
}

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "TowerDefense/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Basic Stats")]
    public string enemyName;
    public int health;
    public float moveSpeed = 1f;
    public int armor;
    public int damageTower;
    public int goldReward;

    [Header("Special Ability")]
    public EnemySpecialType specialType = EnemySpecialType.None;
    public float specialValue;
    public float cooldown;
}
