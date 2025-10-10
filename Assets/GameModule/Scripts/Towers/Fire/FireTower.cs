using UnityEngine;

public class FireTower : BaseTower
{
    [Header("Projectile")]
    [SerializeField] private string projectilePoolKey = "FireProjectile";
    public float projectileLifetime = 1.0f;

    [Header("Fireball Drop Settings")]
    public float spawnHeight = 3f;   // spawn cách đầu enemy bao nhiêu
    public float fallSpeed = 6f;     // tốc độ rơi xuống

    [Header("Immediate Damage (per level)")]
    public float[] immediateDamagePerLevel = { 15f, 25f, 40f };

    [Header("Burn Settings")]
    public float[] burnDamagePerTickPerLevel = { 2f, 3.5f, 5f };
    public float[] burnDurationPerLevel = { 4f, 5f, 6f };
    public float burnTickInterval = 1f;

    [Header("Spread Settings")]
    public float[] spreadRadiusPerLevel = { 1.5f, 2.0f, 2.5f };
    [Range(0f,1f)] public float[] spreadBurnMultiplierPerLevel = { 0.6f, 0.75f, 0.9f };

    protected override bool CanShoot()
    {
        return target != null;
    }

    protected override void Shoot()
    {
        if (archerAnimator != null)
        {
            archerAnimator.SetTrigger("isShoot");
        }
        else
        {
            // fallback nếu không có anim
            SpawnProjectileAtTarget();
        }
    }

    // Gọi từ Animation Event (qua FireTowerAnimatorRelay)
    public void SpawnProjectileAtTarget()
    {
        if (target == null) return;  // enemy có thể đã chết

        // ✅ Lấy vị trí hiện tại của enemy mỗi lần bắn
        Vector3 enemyPos = target.position;
        Vector3 spawnPos = enemyPos + Vector3.up * spawnHeight;

        GameObject proj = PoolManager.Instance.Get(projectilePoolKey, spawnPos, Quaternion.identity);

        int lvl = Mathf.Clamp(currentLevel, 0, immediateDamagePerLevel.Length - 1);

        FireProjectile fireProj = proj.GetComponent<FireProjectile>();
        fireProj.gameObject.SetActive(true);
        if (fireProj != null)
        {
            fireProj.Initialize(
                target,
                immediateDamagePerLevel[lvl],
                burnDamagePerTickPerLevel[lvl],
                burnDurationPerLevel[lvl],
                burnTickInterval,
                spreadRadiusPerLevel[lvl],
                spreadBurnMultiplierPerLevel[lvl],
                enemyLayer,
                projectileLifetime,
                projectilePoolKey,
                fallSpeed
            );
        }

        AudioManager.Instance?.PlaySfx(data.shootSfx);
    }
}
