using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;
    private LayerMask enemyLayer;

    public void Initialize(Transform target, float damage, float speed, LayerMask enemyLayer)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
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
        
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        target.GetComponent<Enemy>()?.TakeDamage((int)damage);
        Debug.Log("Projectile hit: " + damage);

        PoolManager.Instance.Return(gameObject, "Arrow");
    }
}
