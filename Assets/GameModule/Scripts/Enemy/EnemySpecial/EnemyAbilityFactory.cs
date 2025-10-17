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
            default:
                return null;
        }
    }
}
