using UnityEngine;
using System.Collections.Generic;

public class BuffSpeedAbility : IEnemyAbility
{
    private Enemy enemy;
    private EnemyStats stats;
    private float lastBuffTime = 0;
    private List<Enemy> buffedEnemies = new List<Enemy>();

    public void Init(Enemy enemy, EnemyStats stats)
    {
        this.enemy = enemy;
        this.stats = stats;
    }

    public void Update()
    {
        if (Time.time - lastBuffTime >= stats.cooldown)
        {
            ApplyBuff();
            lastBuffTime = Time.time;
        }
        if (buffedEnemies.Count > 0 && Time.time - lastBuffTime >= stats.duration)
        {
            RemoveBuff();
        }
    }

    private void ApplyBuff()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.transform.position, stats.buffRadius);
        buffedEnemies.Clear();
        foreach (var col in colliders)
        {
            Enemy e = col.GetComponent<Enemy>();
            if (e != null && e != enemy)
            {
                e.AddSpeedMultiplier(stats.specialValue);
                buffedEnemies.Add(e);
            }
        }
        Debug.Log($"{enemy.name} kích hoạt BuffSpeed — tăng tốc trong {stats.duration}s cho {buffedEnemies.Count} quái xung quanh.");
    }

    private void RemoveBuff()
    {
        foreach (var e in buffedEnemies)
        {
            if (e != null)
            {
                e.ResetSpeedMultiplier();
            }
        }
        buffedEnemies.Clear();
        Debug.Log($"{enemy.name} kết thúc BuffSpeed.");
    }
    
    public void OnDeath()
    {
        if(buffedEnemies.Count > 0)
        {
            RemoveBuff();
        }
    }
}
