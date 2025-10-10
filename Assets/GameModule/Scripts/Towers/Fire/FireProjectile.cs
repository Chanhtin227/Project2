using System.Collections;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator meteorAnimator; // kéo MeteorVisual vào đây

    private Transform target;
    private float immediateDamage;
    private float burnDamagePerTick;
    private float burnDuration;
    private float burnTickInterval;
    private float spreadRadius;
    private float spreadMultiplier;
    private LayerMask enemyLayer;
    private float lifetime;
    private string poolKey;
    private float fallSpeed;
    private bool hasHit = false;

    private Coroutine moveRoutine;

    public void Initialize(
        Transform target,
        float immediateDamage,
        float burnDamagePerTick,
        float burnDuration,
        float burnTickInterval,
        float spreadRadius,
        float spreadMultiplier,
        LayerMask enemyLayer,
        float lifetime,
        string poolKey,
        float fallSpeed)
    {
        this.target = target;
        this.immediateDamage = immediateDamage;
        this.burnDamagePerTick = burnDamagePerTick;
        this.burnDuration = burnDuration;
        this.burnTickInterval = burnTickInterval;
        this.spreadRadius = spreadRadius;
        this.spreadMultiplier = spreadMultiplier;
        this.enemyLayer = enemyLayer;
        this.lifetime = lifetime;
        this.poolKey = poolKey;
        this.fallSpeed = fallSpeed;
        this.hasHit = false;

        // Play animation từ đầu mỗi lần spawn
        if (meteorAnimator != null)
            meteorAnimator.Play(0, -1, 0);

        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(FallToEnemy());
    }

    private IEnumerator FallToEnemy()
    {
        while (!hasHit && target != null)
        {
            Vector3 dir = (target.position - transform.position);
            float dist = dir.magnitude;

            if (dist < 0.1f)
            {
                HitTarget();
                yield break;
            }

            transform.position += dir.normalized * fallSpeed * Time.deltaTime;
            yield return null;
        }

        if (!hasHit)
            PoolManager.Instance.Return(gameObject, poolKey);
    }

    private void HitTarget()
    {
        if (hasHit) return;
        hasHit = true;

        if (target == null)
        {
            PoolManager.Instance.Return(gameObject, poolKey);
            return;
        }

        Vector3 hitPos = target.position;

        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(Mathf.RoundToInt(immediateDamage));
            //enemy.ApplyBurn(burnDamagePerTick, burnDuration, burnTickInterval);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPos, spreadRadius, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.transform == target) continue;
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null)
            {
                float spreadTick = burnDamagePerTick * spreadMultiplier;
                float spreadDur = burnDuration * 0.8f;
                //e.ApplyBurn(spreadTick, spreadDur, burnTickInterval);
            }
        }

        // Chờ animation kết thúc mới Return
        StartCoroutine(ReturnAfterAnimation());
    }

    private IEnumerator ReturnAfterAnimation()
    {
        if (meteorAnimator != null)
        {
            // Lấy thời gian clip hiện tại
            float clipLen = meteorAnimator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(clipLen);
        }
        else
        {
            // fallback nếu thiếu animator
            yield return new WaitForSeconds(lifetime);
        }

        PoolManager.Instance.Return(gameObject, poolKey);
    }
}
