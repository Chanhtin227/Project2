using UnityEngine;

public class LightningTower : BaseTower
{
    [Header("Lightning Settings")]
    public GameObject lightningPrefab;
    public int maxChains = 3;
    public float chainRange = 3f;

    protected override void Shoot()
    {
        if (target == null) return;

        LightningProjectile zap = PoolManager.Instance.Get<LightningProjectile>("Lightning");
        if (zap != null)
        {
            zap.Initialize(firePoint != null ? firePoint : transform, target, damage, maxChains, chainRange, enemyLayer);
        }
    }
}
