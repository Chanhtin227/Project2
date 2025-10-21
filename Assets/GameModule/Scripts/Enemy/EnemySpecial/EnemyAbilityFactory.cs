public static class EnemyAbilityFactory
{
    public static IEnemyAbility CreateAbility(EnemySpecialType type)
    {
        switch (type)
        {
            case EnemySpecialType.BuffSpeed:
                return new BuffSpeedAbility();
            case EnemySpecialType.BuffArmor:
                return new BuffArmorAbility();
            case EnemySpecialType.LowHpUpSpeed:
                return new LowHpUpSpeed();
            case EnemySpecialType.HealOnDeath:
                return new HealOnDeath();
            case EnemySpecialType.AttackTower:
                return new AttackTower();
            default:
                return null;
        }
    }
}
