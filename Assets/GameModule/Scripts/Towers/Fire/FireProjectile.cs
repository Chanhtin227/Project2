using System.Collections;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator meteorAnimator;

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
    private float projectileSpeed;
    private bool hasHit = false;

    private Coroutine moveRoutine;

    void OnEnable()
    {
        // Reset state mỗi khi object được enable từ pool
        hasHit = false;
        // KHÔNG reset animator ở đây vì có thể gây lỗi
        // Sẽ reset trong Initialize() thay thế
    }

    private void SafeResetAnimator()
    {
        if (meteorAnimator != null && 
            meteorAnimator.runtimeAnimatorController != null && 
            meteorAnimator.isActiveAndEnabled &&
            gameObject.activeInHierarchy)
        {
            meteorAnimator.Rebind();
            meteorAnimator.Update(0f);
            meteorAnimator.Play("idle", 0, 0);
        }
    }

    void OnDisable()
    {
        // Dọn dẹp khi object bị disable
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }
        
        // Dừng tất cả coroutines
        StopAllCoroutines();
        
        hasHit = false;
        target = null;
    }

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
        float projectileSpeed)
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
        this.projectileSpeed = projectileSpeed;
        this.hasHit = false;

        // Reset animator ở đây thay vì OnEnable (an toàn hơn)
        SafeResetAnimator();

        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(FlyToTarget());
    }

    private IEnumerator FlyToTarget()
    {
        float travelTime = 0f;

        while (!hasHit && target != null && travelTime < lifetime)
        {
            Vector3 dir = (target.position - transform.position);
            float dist = dir.magnitude;

            // Xoay về phía target
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

            // Check đã đến gần target chưa
            if (dist < 0.2f)
            {
                HitTarget();
                yield break;
            }

            // Di chuyển theo hướng target với tốc độ cố định
            transform.position += dir.normalized * projectileSpeed * Time.deltaTime;
            travelTime += Time.deltaTime;
            yield return null;
        }

        // Nếu không hit được (target chết hoặc quá xa) thì return về pool
        if (!hasHit)
            PoolManager.Instance.Return(gameObject, poolKey);
    }

    private void HitTarget()
    {
        if (hasHit) return;
        hasHit = true;

        if (target == null)
        {
            StartCoroutine(ReturnAfterAnimation());
            return;
        }

        Vector3 hitPos = target.position;

        // Gây damage cho target chính
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(Mathf.RoundToInt(immediateDamage));
            //enemy.ApplyBurn(burnDamagePerTick, burnDuration, burnTickInterval);
        }

        // Tìm các enemy xung quanh để spread burn
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPos, spreadRadius, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.transform == target) continue; // Bỏ qua target chính
            
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null)
            {
                float spreadTick = burnDamagePerTick * spreadMultiplier;
                float spreadDur = burnDuration * 0.8f;
                //e.ApplyBurn(spreadTick, spreadDur, burnTickInterval);
            }
        }

        Debug.Log($"Meteor hit: {enemy.name} {immediateDamage} immediate damage, {burnDamagePerTick} burn/tick over {burnDuration}s");
        StartCoroutine(ReturnAfterAnimation());
    }

    private IEnumerator ReturnAfterAnimation()
    {
        if (meteorAnimator != null &&
            meteorAnimator.isActiveAndEnabled &&
            meteorAnimator.runtimeAnimatorController != null &&
            meteorAnimator.gameObject.activeInHierarchy)
        {

            // Trigger animation Shoot (explosion)
            meteorAnimator.SetTrigger("isShoot");

            // Đợi animation hoàn thành
            float clipLen = 0.5f;

            if (meteorAnimator.runtimeAnimatorController.animationClips != null)
            {
                foreach (var clip in meteorAnimator.runtimeAnimatorController.animationClips)
                {
                    if (clip.name == "Shoot")
                    {
                        clipLen = clip.length;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(clipLen);
        }
        else
        {
            yield return new WaitForSeconds(0.3f); // Default delay nếu không có animator
        }

        if (PoolManager.Instance != null && !string.IsNullOrEmpty(poolKey))
        {
            PoolManager.Instance.Return(gameObject, poolKey);
        }
    }

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        if (spreadRadius > 0)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Màu cam trong suốt
            Gizmos.DrawWireSphere(transform.position, spreadRadius);
        }
    }
}