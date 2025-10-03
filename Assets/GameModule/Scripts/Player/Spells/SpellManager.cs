using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance { get; private set; }

    private bool waitingForClick = false;
    private SpellData pendingSpell;
    public GameObject indicatorPrefab; // gán SpellIndicator prefab trong inspector
    private GameObject activeIndicator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    private void Update()
    {
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
    /// Được gọi từ SpellButton khi người chơi bấm spell
    /// </summary>
    public void CastSpell(SpellData spell)
    {
        Debug.Log("Cast Spell: " + spell.spellName);

        switch (spell.effectType)
        {
            case SpellEffectType.Damage:
            case SpellEffectType.Slow:
            case SpellEffectType.Stun:
            case SpellEffectType.Debuff:
            waitingForClick = true;
            pendingSpell = spell;

            // UI overlay mờ + slow motion
            UIManager.Instance.ShowSpellCastingUI(true);

            // Spawn indicator
            if (indicatorPrefab != null)
            {
                activeIndicator = Instantiate(indicatorPrefab);
                activeIndicator.GetComponent<SpellTargetIndicator>().Setup(spell);
            }
            break;
        }
    }


    private void CastSpellAtPosition(SpellData spell, Vector3 pos)
    {
        if (spell.effectPrefab != null)
        {
            GameObject fx = Instantiate(spell.effectPrefab, pos, Quaternion.identity);
            SpellEffect effect = fx.AddComponent<SpellEffect>();
            effect.Setup(spell);
        }

        waitingForClick = false;
        pendingSpell = null;

        // Reset UI và hủy indicator
        UIManager.Instance.ShowSpellCastingUI(false);
        if (activeIndicator != null) Destroy(activeIndicator);
    }


    private void PrepareClickTarget(SpellData spell)
    {
        Debug.Log($"{spell.spellName} sẵn sàng, click để chọn vị trí!");
        waitingForClick = true;
        pendingSpell = spell;
    }
}
