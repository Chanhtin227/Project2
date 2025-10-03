using UnityEngine;

public enum SpellType
{
    Fire,
    Freeze,
    Zap,
    ArmorBreak,
    Enhance,
    Rock,
    AxitRain
}

public enum SpellEffectType
{
    Damage,
    DOT,
    Slow,
    Stun,
    Buff,
    Debuff
}

[CreateAssetMenu(fileName = "NewSpell", menuName = "Tower Defense/Spell")]
public class SpellData : ScriptableObject
{
    [Header("Basic Info")]
    public string spellName = "New Spell";
    public SpellType type;
    public SpellEffectType effectType;

    [Header("Spell Settings")]
    public float cooldown = 5f;        // thời gian hồi chiêu
    public float damage = 0f;          // dùng cho spell gây dmg
    public float damagePerSecond = 0f; // dùng cho DOT (damage over time)
    public float slowPercent = 0f;     // dùng cho slow (0.5 = giảm 50%)
    public float radius = 2f;          // bán kính tác dụng

    [Header("Visual")]
    public GameObject effectPrefab;    // prefab hiệu ứng (explosion, ice, lightning...)
}
