using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;
    private float slowAmount;
    private float slowDuration;
    private LayerMask enemyLayer;

    public void Initialize(Transform target, float damage, float speed, LayerMask enemyLayer, float slowAmount, float slowDuration)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.enemyLayer = enemyLayer;
        this.slowAmount = slowAmount;
        this.slowDuration = slowDuration;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Xoay về hướng target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Nếu tới nơi -> va chạm
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Di chuyển
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage((int)damage);
            //enemy.ApplySlow(slowAmount, slowDuration);
            Debug.Log("Damaged: " + enemy.name + " for " + damage + " damage and slowed by " + (slowAmount * 100) + "% for " + slowDuration + " seconds.");
        }

        PoolManager.Instance.Return(gameObject, "IceProjectile");
    }
}
// add doan nay vao enemy
// public void ApplySlow(float slowAmount, float duration)
// {
//     if (isSlowed) return;
//     StartCoroutine(SlowRoutine(slowAmount, duration));
// }

// private IEnumerator SlowRoutine(float slowAmount, float duration)
// {
//     isSlowed = true;
//     float originalSpeed = moveSpeed;
//     moveSpeed *= (1f - slowAmount);

//     yield return new WaitForSeconds(duration);

//     moveSpeed = originalSpeed;
//     isSlowed = false;
// }

