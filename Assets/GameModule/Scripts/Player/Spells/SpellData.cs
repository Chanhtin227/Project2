// SpellData.cs (thêm các trường mới)
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
    public float cooldown = 5f;
    public float damage = 0f;
    public float damagePerSecond = 0f; // DOT
    public float dotDuration = 5f;     // DOT tổng thời gian
    public float slowPercent = 0f;
    public float slowDuration = 0f;
    public float radius = 2f;

    [Header("Targeting")]
    public bool singleTarget = false;

    [Header("Visual")]
    public GameObject effectPrefab;

    [Header("Unlock Settings")]
    public bool isUnlocked = false;
    public int unlockLevel = 1;
}
