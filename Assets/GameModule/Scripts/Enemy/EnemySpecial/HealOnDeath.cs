using UnityEngine;

public class HealOnDeath : IEnemyAbility
{
    private Enemy enemy;
    private EnemyStats stats;

    public void Init(Enemy enemy, EnemyStats stats)
    {
        this.enemy = enemy;
        this.stats = stats;
    }

    public void Update()
    {
        
    }

    public void OnDeath()
    {
        float radius = stats.buffRadius;
        Enemy nearestEnemy = FindNearestEnemy(radius);
        if (nearestEnemy != null)
        {
            int healAmount = Mathf.CeilToInt(nearestEnemy.GetMaxHP() * 0.5f);
            int oldHP = nearestEnemy.GetCurrentHP();
            nearestEnemy.Heal(healAmount);
            int newHP = nearestEnemy.GetCurrentHP();
            Debug.Log($"HealOnDeath: Healed {healAmount} HP to {nearestEnemy.name}. {oldHP} -> {newHP}");
        }
        else
        {
            Debug.Log("HealOnDeath: No nearby enemy to heal.");
        }
    }
    
    private Enemy FindNearestEnemy(float radius)
    {
        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy nearest = null;
        float nearestDistance = float.MaxValue;
        foreach (Enemy e in allEnemies)
        {
            if (e == enemy || e.IsDead() || e == null) continue;
            float distance = Vector2.Distance(enemy.transform.position, e.transform.position);
            if (distance < nearestDistance && distance <= radius)
            {
                nearestDistance = distance;
                nearest = e;
            }
        }
        return nearest;
    }
}
