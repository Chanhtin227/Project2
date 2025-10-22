using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour
{
    private SpellData data;

    public void Setup(SpellData spell)
    {
        data = spell;
        
        // Xử lý hiệu ứng đặc biệt cho AxitRain
        if (data.type == SpellType.AxitRain)
        {
            SetupAxitRainAnimation();
        }
        
        // Apply effect lên enemies
        ApplyEffect();
    }

    private void SetupAxitRainAnimation()
    {
        Animator anim = GetComponent<Animator>();
        
        float fadeDuration = 2f;
        float loopTime = Mathf.Max(0, data.dotDuration - fadeDuration);

        // Tự hủy sau dotDuration
        gameObject.AddComponent<AutoDestroyAfterTime>().lifeTime = data.dotDuration;

        if (anim != null)
        {
            gameObject.AddComponent<SpellRainAnimator>().Init(anim, loopTime, "end");
        }
    }

    private void ApplyEffect()
    {
        if (data == null) return;

        // Gây hiệu ứng lên quái
        if (data.singleTarget)
        {
            Collider2D hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Enemy"));
            if (hit != null)
            {
                Enemy target = hit.GetComponent<Enemy>();
                if (target != null)
                    ApplyToEnemy(target);
            }
        }
        else
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, data.radius, LayerMask.GetMask("Enemy"));
            foreach (var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                    ApplyToEnemy(enemy);
            }
        }
    }

    private void ApplyToEnemy(Enemy e)
    {
        switch (data.type)
        {
            case SpellType.Rock:
                e.TakeDamage(Mathf.RoundToInt(data.damage));
                e.ApplyStun(2f);
                break;

            case SpellType.Zap:
                e.TakeDamage(Mathf.RoundToInt(data.damage));
                e.ApplyParalyze(1.5f);
                break;

            case SpellType.AxitRain:
                e.ApplyDOT(data.damagePerSecond, data.dotDuration);
                break;

            default:
                // Xử lý mặc định
                if (data.effectType == SpellEffectType.Damage)
                    e.TakeDamage(Mathf.RoundToInt(data.damage));
                else if (data.effectType == SpellEffectType.Slow)
                    e.ApplySlow(data.slowPercent, data.slowDuration);
                break;
        }
    }
}