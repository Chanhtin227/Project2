public interface IEnemyAbility
{
    void Init(Enemy enemy, EnemyStats stats);
    void Update();
    void OnDeath();
}
