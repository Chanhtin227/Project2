using UnityEngine;

public class FireTower : BaseTower
{
    [Header("Projectile")]
    [SerializeField] private string projectilePoolKey = "FireProjectile";
    public float projectileLifetime = 1.0f;
    public float projectileSpeed;

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
        if (target == null) return;

        GameObject proj = PoolManager.Instance.Get(projectilePoolKey, firePoint.position, firePoint.rotation);
        
        proj.SetActive(true);

        int lvl = Mathf.Clamp(currentLevel, 0, immediateDamagePerLevel.Length - 1);

        FireProjectile fireProj = proj.GetComponent<FireProjectile>();
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
                projectileSpeed
            );
        }
        AudioManager.Instance?.PlaySfx(data.shootSfx);
    }
    
    public void shootEvent()
    {
        Shoot();
    }
}