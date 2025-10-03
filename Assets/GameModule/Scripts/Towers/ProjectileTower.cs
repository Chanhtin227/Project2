using UnityEngine;

public class ProjectileTower : BaseTower
{
    [SerializeField] private Projectile projectilePrefab; // prefab đạn
    public float projectileSpeed = 8f;

    protected override void Shoot()
    {
        if (target == null) return;
        Debug.Log("Tower shooting at: " + target.name);

        Projectile p = PoolManager.Instance.Get<Projectile>("Arrow");
        p.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        p.gameObject.SetActive(true);
        if (p != null)
        {
            p.Initialize(target, damage, projectileSpeed, enemyLayer);
        }
    }

    public void shootEvent()
    {
        Shoot();
    }
}
