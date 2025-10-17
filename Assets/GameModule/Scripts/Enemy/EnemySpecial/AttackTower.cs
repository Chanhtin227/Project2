using UnityEngine;

public class AttackTower : IEnemyAbility
{
    private Enemy enemy;
    private EnemyStats stats;
    private BaseTower targetTower;
    float attackCooldown = 0f;
    private bool wasAttacking = false;

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
            float damage = stats.specialValue;
            targetTower.TakeDamage(damage);
            Debug.Log($"[AttackTower] {enemy.name} attacked {targetTower.name} for {damage} damage!");
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
