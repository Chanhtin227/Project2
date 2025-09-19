using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Tower Data")]
    public TowerData data;
    public int currentLevel = 0;


    protected float range;
    protected float damage;
    protected float fireRate;

    [Header("Health")]
    protected float maxHealth;
    public float currentHealth;
    public bool isDestroyed { get; private set; }
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    [Header("Targeting")]
    public LayerMask enemyLayer;
    public Transform firePoint;

    protected Transform target;
    float fireCooldown;

    [Header("Visual (2D)")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] levelSprites;

    [Header("Sell Settings")]
    [Range(0f, 1f)]
    public float refundPercent = 0.5f; // 1f = hoàn 100%, 0.5f = hoàn 50%

    protected virtual void Start()
    {
        ApplyStats();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        UpdateTarget();
        if (isDestroyed) return;

        fireCooldown -= Time.deltaTime;

        if (target == null || Vector2.Distance(transform.position, target.position) > range)
            AcquireTarget();

        if (target == null) return;

        if (fireCooldown <= 0f)
        {
            if (CanShoot())
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
        }
    }

    void UpdateTarget()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    void AcquireTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        float bestDist = Mathf.Infinity;
        Transform best = null;

        foreach (var h in hits)
        {
            float d = Vector2.Distance(transform.position, h.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = h.transform;
            }
        }

        target = best;
    }

    protected virtual bool CanShoot() => true;

    protected abstract void Shoot();

    public void Upgrade()
    {
        if (currentLevel + 1 < data.levels.Length)
        {
            currentLevel++;
            ApplyStats();
        }
        else
        {
            Debug.Log($"{data.towerName} đã đạt cấp tối đa!");
        }
    }

    void ApplyStats()
    {
        TowerData.TowerLevel lvl = data.levels[currentLevel];
        range = lvl.range;
        damage = lvl.damage;
        fireRate = lvl.fireRate;
        maxHealth = lvl.maxHealth;
        currentHealth = maxHealth;

        UpdateVisual(currentLevel);
    }

    void UpdateVisual(int level)
    {
        if (spriteRenderer != null && level < levelSprites.Length)
        {
            spriteRenderer.sprite = levelSprites[level];
        }
    }



    public virtual void TakeDamage(float amount)
    {
        if (isDestroyed) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetRefund()
    {
        if (data == null || data.levels == null || currentLevel >= data.levels.Length)
            return 0;

        int currentCost = data.levels[currentLevel].cost;
        return Mathf.RoundToInt(currentCost * refundPercent);
    }


    public void Sell()
    {
        int refund = GetRefund();

        if (GoldManager.Instance != null)
            GoldManager.Instance.AddGold(refund);

        Debug.Log($"{data.towerName} sold for {refund} gold (level {currentLevel + 1}).");

        Destroy(gameObject); // hoặc trả về pool nếu dùng pooling
    }

    protected virtual void Die()
    {
        isDestroyed = true;
        Debug.Log($"{data.towerName} đã bị phá hủy!");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
