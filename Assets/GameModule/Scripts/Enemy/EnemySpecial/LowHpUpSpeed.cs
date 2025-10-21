using UnityEngine;

public class LowHpUpSpeed : IEnemyAbility
{
    private Enemy enemy;
    private EnemyStats stats;
    //mốc hp
    private int stageHp = 0;
    //tốc độ hiện tại
    private float currentSpeed = 1;
    //mỗi mốc 10% hp
    private const float intervalHp = 0.1f;
    //mõi mốc 10% speed
    private const float intervalSpeed = 1.1f;

    public void Init(Enemy enemy, EnemyStats stats)
    {
        this.enemy = enemy;
        this.stats = stats;
    }

    public void Update()
    {
        float hpPercent = (float)enemy.GetCurrentHP() / stats.health;
        int currentStageHp = Mathf.FloorToInt((1 - hpPercent) / intervalHp);
        if (currentStageHp != stageHp)
        {
            float newSpeed = Mathf.Pow(intervalSpeed, currentStageHp);
            float deltaSpeed = newSpeed / currentSpeed;
            enemy.AddSpeedMultiplier(deltaSpeed);
            currentSpeed = newSpeed;
            stageHp = currentStageHp;
            Debug.Log($"LowHpUpSpeed: HP%={hpPercent}, Stage={stageHp}, Speed Multiplier={currentSpeed}");
        }
    }

    public void OnDeath()
    {
        
    }
}
