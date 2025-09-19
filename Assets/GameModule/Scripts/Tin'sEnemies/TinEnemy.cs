using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private int pathId = 0; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Transform[] path;
    private Transform checkpoint;
    private int index = 0;
    private int currentHP;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHP = stats.health;
    }

    void Start()
    {
        path = MultiPathCheckpointsManager.main.GetPath(pathId);

        if (path != null && path.Length > 0)
        {
            checkpoint = path[index];
        }
    }

    void Update()
    {
        if (checkpoint == null) return;

        // Khi tới checkpoint
        if (Vector2.Distance(transform.position, checkpoint.position) <= 0.2f)
        {
            index++;
            if (index >= path.Length)
            {
                Destroy(gameObject);
                return;
            }
            checkpoint = path[index];
        }
    }

    void FixedUpdate()
    {
        if (checkpoint == null) return;

        Vector2 direction = (checkpoint.position - transform.position).normalized;
        rb.linearVelocity = direction * stats.moveSpeed;

        if (rb.linearVelocity.x > 0.1f) spriteRenderer.flipX = false;
        else if (rb.linearVelocity.x < -0.1f) spriteRenderer.flipX = true;
    }

    // Hàm nhận sát thương từ tower
    public void TakeDamage(int dmg)
    {
        int finalDamage = Mathf.Max(0, dmg - stats.armor);
        currentHP -= finalDamage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // Xử lý khi quái chết
    private void Die()
    {
        // Cộng vàng cho người chơi
        Debug.Log($"{stats.enemyName} chết, nhận {stats.goldReward} vàng!");

        Destroy(gameObject);
    }
}
