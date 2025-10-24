using UnityEngine;

public class ExplosiveDamage : IEnemyAbility
{
    private Enemy enemy;
    private EnemyStats stats;
    private BaseTower targetTower;
    private float attackCooldown = 0f;
    private bool wasAttacking = false;
    private int hitCount = 0;
    private const int hitsToExplode = 5;

    public void Init(Enemy enemy, EnemyStats stats)
    {
        this.enemy = enemy;
        this.stats = stats;
        targetTower = FindNearestTower();
    }

    public void Update()
    {
        if (enemy.IsDead()) return;
        if (targetTower == null || targetTower.IsDestroyed())
        {
            if (wasAttacking)
            {
                enemy.SetAttackStage(false);
                enemy.ResumeMove();
                wasAttacking = false;
            }
            targetTower = FindNearestTower();
            return;
        }
        float distance = Vector2.Distance(enemy.transform.position, targetTower.transform.position);
        float attackRange = stats.buffRadius;
        if (distance <= attackRange)
        {
            if (!wasAttacking)
            {
                enemy.SetAttackStage(true);
                enemy.StopMove();
                wasAttacking = true;
            }
            if (Time.time >= attackCooldown)
            {
                Attack();
                attackCooldown = Time.time + stats.cooldown;
            }
        }
        else
        {
            if (wasAttacking)
            {
                enemy.SetAttackStage(false);
                enemy.ResumeMove();
                wasAttacking = false;
            }
        }
    }

    private void Attack()
    {
        if (targetTower == null) return;
        if (!targetTower.IsDestroyed())
        {
            hitCount++;
            float damage = stats.specialValue;
            targetTower.TakeDamage(damage);
            Debug.Log($"ExplosiveDamage: Tower hit {hitCount} times.");
            if (hitCount >= hitsToExplode)
            {
                float explosionDamage = stats.specialValue * 10;
                targetTower.TakeDamage(explosionDamage);
                Debug.Log($"ExplosiveDamage: Tower exploded for {explosionDamage} damage!");
                hitCount = 0;
            }
        }
        else
        {
            enemy.SetAttackStage(false);
            enemy.ResumeMove();
            targetTower = null;
        }
    }
    
    private BaseTower FindNearestTower()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Towers");
        BaseTower nearestTower = null;
        float nearestDist = float.MaxValue;
        foreach (GameObject obj in objects)
        {
            BaseTower tower = obj.GetComponent<BaseTower>();
            if (tower == null || tower.IsDestroyed()) continue;
            float distance = Vector2.Distance(enemy.transform.position, tower.transform.position);
            if (distance < nearestDist)
            {
                nearestDist = distance;
                nearestTower = tower;
            }
        }
        return nearestTower;
    }

    public void OnDeath()
    {
        
    }
}
