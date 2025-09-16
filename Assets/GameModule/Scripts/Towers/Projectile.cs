using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;
    private float splashRadius;
    private LayerMask enemyLayer;

    public void Initialize(Transform target, float damage, float speed, float splashRadius, LayerMask enemyLayer)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.splashRadius = splashRadius;
        this.enemyLayer = enemyLayer;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // bay về phía target
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (splashRadius > 0)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, splashRadius, enemyLayer);
            foreach (var enemy in enemies)
            {
                // gây damage
                enemy.GetComponent<enemy>()?.TakeDamage(damage);
            }
        }
        else
        {
            target.GetComponent<enemy>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
