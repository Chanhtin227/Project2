using UnityEngine;

public class LightningTower : BaseTower
{
    [Header("Lightning Settings")]
    public LightningProjectile lightningPrefab; // prefab chứa script LightningProjectile
    public int maxChains = 3;                   // số enemy tối đa bị lan
    public float chainRange = 3f;               // tầm lan

    protected override void Shoot()
    {
        if (lightningPrefab == null || target == null) return;

        // Spawn tia sét
        LightningProjectile zap = Instantiate(lightningPrefab, transform.position, Quaternion.identity);
        zap.Initialize(firePoint != null ? firePoint : transform, target, damage, maxChains, chainRange, enemyLayer);
    }
}
