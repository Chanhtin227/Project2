using UnityEngine;

public class enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxHealth = 50f;
    public float dmg = 10f;
    public float range;
    private float currentHealth;

    public Transform[] waypoints; // gán trong Inspector
    private int waypointIndex = 0;
    public LayerMask towerLayer;

    // Thêm biến cooldown
    public float attackInterval = 1.5f;
    private float attackCooldown = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        MoveAlongPath();
        AttackNearbyTower();
    }

    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;

        Transform targetPoint = waypoints[waypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                Destroy(gameObject); // tới cuối đường thì biến mất
            }
        }
    }

    // Hàm tấn công tự động
    void AttackNearbyTower()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Collider2D tower = Physics2D.OverlapCircle(transform.position, 0.5f, towerLayer);
            if (tower != null)
            {
                BaseTower bt = tower.GetComponent<BaseTower>();
                if (bt != null && !bt.isDestroyed)
                {
                    bt.TakeDamage(dmg);
                    Debug.Log($"{gameObject.name} attacked {tower.name} for {dmg} damage.");
                    attackCooldown = attackInterval;
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP left: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
