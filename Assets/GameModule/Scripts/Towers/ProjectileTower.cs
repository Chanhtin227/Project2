using UnityEngine;

public class ProjectileTower : BaseTower
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public float splashRadius = 0f;

   protected override void Shoot()
{
    if (projectilePrefab == null || firePoint == null || target == null)
    {
        Debug.LogWarning("Shoot failed: missing prefab/firepoint/target");
        return;
    }

    Debug.Log("Tower shooting at: " + target.name);

    GameObject go = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    Projectile p = go.GetComponent<Projectile>();
    if (p != null)
    {
        p.Initialize(target, damage, projectileSpeed, splashRadius, enemyLayer);
    }
}

}
