using UnityEngine;

public class SpellEffect : MonoBehaviour
{
    private SpellData data;

    public void Setup(SpellData spell)
    {
        data = spell;
        ApplyEffect();
    }

    private void ApplyEffect()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            data.radius,
            LayerMask.GetMask("Enemy")
        );

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null) continue;

            switch (data.effectType)
            {
                case SpellEffectType.Damage:
                    enemy.TakeDamage(Mathf.RoundToInt(data.damage));
                    Debug.Log($"[spell effect]: {data.spellName} dealt {data.damage} damage to {enemy.name}");
                    break;
                case SpellEffectType.DOT:
                    // add logic DOT
                    break;
                case SpellEffectType.Slow:
                    enemy.ApplySlow(data.slowPercent, data.slowDuration);
                    if (data.damage > 0)
                        enemy.TakeDamage(Mathf.RoundToInt(data.damage));
                    break;
                case SpellEffectType.Stun:
                    // add logic stun
                    break;
                case SpellEffectType.Debuff:
                    // add logic debuff
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (data != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.radius);
        }
    }
}
