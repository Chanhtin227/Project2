using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private int pathId = 0;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator _anim;

    private Transform[] path;
    private Transform checkpoint;
    private int index = 0;
    private int currentHP;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
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
        // Công thức giảm dần: 100 / (100 + armor)
        float multiplier = 100f / (100f + stats.armor);
        int finalDamage = Mathf.RoundToInt(dmg * multiplier);
        currentHP -= finalDamage;
        if (currentHP <= 0)
        {
            Die();
        }
    }


    // Xử lý khi quái chết
    private void Die()
    {
        _anim.SetTrigger("isDead");
        // Cộng vàng cho người chơi
        Debug.Log($"{stats.enemyName} chết, nhận {stats.goldReward} vàng!");

        StartCoroutine(DestroyAfterAnimation());
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        this.enabled = false;
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorClipInfo(0).Length); // Giả sử animation chết dài 1 giây
        Destroy(gameObject);
    }
    
    public void SetStats(EnemyStats newStats)
    {
        stats = newStats;
        currentHP = stats.health;
    }
}
