using UnityEngine;

public class IceTower : BaseTower
{
    [SerializeField] private GameObject iceProjectilePrefab;
    public float projectileSpeed = 7f;

    [Header("Slow Settings")]
    [Range(0f, 1f)] public float[] slowPercentPerLevel = { 0.2f, 0.4f, 0.6f };
    public float slowDuration = 2f;

    protected override void Shoot()
    {
        if (target == null) return;

        IceProjectile projectileObj = PoolManager.Instance.Get<IceProjectile>("IceProjectile");
        projectileObj.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        projectileObj.gameObject.SetActive(true);

        float slowAmount = slowPercentPerLevel[Mathf.Clamp(currentLevel, 0, slowPercentPerLevel.Length - 1)];

        IceProjectile iceProjectile = projectileObj.GetComponent<IceProjectile>();
        if (iceProjectile != null)
        {
            iceProjectile.Initialize(target, damage, projectileSpeed, enemyLayer, slowAmount, slowDuration);
        }

        AudioManager.Instance?.PlaySfx(data.shootSfx);
    }
    
}
