using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance { get; private set; }

    private bool waitingForClick = false;
    private SpellData pendingSpell;
    public GameObject indicatorPrefab;
    private GameObject activeIndicator;
    private SpellButton pendingButton;

    [Header("Cooldown & Unlock")]
    public float globalCooldown = 5f;
    [HideInInspector] public float globalCooldownTimer = 0f;

    [Header("All Spells In Game")]
    public List<SpellData> allSpells;
    [HideInInspector] public List<SpellType> unlockedSpells = new List<SpellType>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        AutoUnlockSpells();
    }

    private void Update()
    {
        if (globalCooldownTimer > 0)
            globalCooldownTimer -= Time.deltaTime;

        if (waitingForClick && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0f;
            CastSpellAtPosition(pendingSpell, mousePos);
            waitingForClick = false;
            pendingSpell = null;
        }
    }

    /// <summary>
    /// Mở sẵn các spell đã tick "isUnlocked" trong SpellData.
    /// Gọi một lần khi khởi tạo.
    /// </summary>
    private void AutoUnlockSpells()
    {
        foreach (var spell in allSpells)
        {
            if (spell != null && spell.isUnlocked && !unlockedSpells.Contains(spell.type))
            {
                unlockedSpells.Add(spell.type);
                Debug.Log($"[SpellManager] Tự động mở phép: {spell.spellName}");
            }
        }
    }

    public bool IsSpellUnlocked(SpellType type)
    {
        return unlockedSpells.Contains(type);
    }

    public void UnlockSpell(SpellType type)
    {
        if (!unlockedSpells.Contains(type))
        {
            unlockedSpells.Add(type);
            Debug.Log($"[SpellManager] Đã mở khóa phép: {type}");
        }
    }

    public void CastSpell(SpellData spell, SpellButton button)
    {
        if (globalCooldownTimer > 0)
        {
            Debug.Log("Phép đang hồi chiêu chung!");
            return;
        }

        if (!IsSpellUnlocked(spell.type))
        {
            Debug.Log($"Phép {spell.spellName} chưa được mở!");
            return;
        }

        Debug.Log("Cast Spell: " + spell.spellName);

        waitingForClick = true;
        pendingSpell = spell;
        pendingButton = button;
        UIManager.Instance.ShowSpellCastingUI(true);

        if (indicatorPrefab != null)
        {
            activeIndicator = Instantiate(indicatorPrefab);
            activeIndicator.GetComponent<SpellTargetIndicator>().Setup(spell);
        }
    }

    private void CastSpellAtPosition(SpellData spell, Vector3 pos)
    {
        if (spell.effectPrefab != null)
        {
            GameObject fx = Instantiate(spell.effectPrefab, pos, Quaternion.identity);
            SpellEffect effect = fx.AddComponent<SpellEffect>();
            effect.Setup(spell);
            pendingButton.StartCooldown();
            globalCooldownTimer = globalCooldown;
        }

        waitingForClick = false;
        pendingSpell = null;
        UIManager.Instance.ShowSpellCastingUI(false);
        if (activeIndicator != null) Destroy(activeIndicator);
    }
}
