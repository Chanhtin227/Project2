using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float dmg = 10f;

    private float currentHealth;

    [Header("Waypoint")]
    private Transform checkpoint;
    private int index = 0;

    [Header("Attack Settings")]
    public float attackInterval = 1.5f;
    private float attackCooldown = 0f;
    public LayerMask towerLayer;
    public float attackRange = 0.5f;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void Start()
    {
        checkpoint = CheckpointsManager.main.checkpoints[index];
    }

    void Update()
    {
        // kiểm tra tới checkpoint chưa
        if (Vector2.Distance(transform.position, checkpoint.position) <= 0.2f)
        {
            index++;
            if (index >= CheckpointsManager.main.checkpoints.Length)
            {
                Destroy(gameObject); // hết đường thì tự hủy
                return;
            }
            checkpoint = CheckpointsManager.main.checkpoints[index];
        }

        // Attack tower nếu ở gần
        AttackNearbyTower();
    }

    void FixedUpdate()
    {
        // tính hướng đi và di chuyển bằng Rigidbody2D
        Vector2 direction = (checkpoint.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        // lật sprite theo hướng di chuyển
        if (rb.linearVelocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    void AttackNearbyTower()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Collider2D tower = Physics2D.OverlapCircle(transform.position, attackRange, towerLayer);
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

    // để debug thấy vùng attack trong Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
