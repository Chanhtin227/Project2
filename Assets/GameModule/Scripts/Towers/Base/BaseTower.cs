using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Tower Data")]
    public TowerData data;
    public int currentLevel = 0;

    protected float range; 
    public float Range => range;
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
    private Animator _anim;

    [Header("Visual (2D)")]
    [SerializeField] private SpriteRenderer towerBaseRenderer; // phần trụ
    [SerializeField] private Sprite[] baseSprites;             // sprite trụ theo cấp

    [SerializeField] private Animator archerAnimator;          // Animator cho cung thủ (nếu có)
    [SerializeField] private RuntimeAnimatorController[] archerAnimators; // anim theo cấp


    [Header("Sell Settings")]
    [Range(0f, 1f)]

    public BuildSpot buildSpot { get; private set; }
    public void AssignBuildSpot(BuildSpot spot)
    {
        buildSpot = spot;
    }

    protected virtual void Start()
    {
        ApplyStats();
        currentHealth = maxHealth;
    }

    // thoi gina ban va thoi gian anim khong dong bo nen firerate bi phe
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
                if (archerAnimator != null)
                {
                    archerAnimator.SetTrigger("isShoot"); // chỉ gọi anim
                }
                else
                {
                    Shoot(); // fallback nếu không có animator
                }
                fireCooldown = fireRate;
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
            AudioManager.Instance.PlaySfx(data.upgradeSfx);
            Debug.Log($"{data.towerName} upgraded to level {currentLevel + 1}.");
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
        // Update base tower sprite
        if (towerBaseRenderer != null && level < baseSprites.Length)
        {
            towerBaseRenderer.sprite = baseSprites[level];
        }

        // Update animation (nếu có)
        if (archerAnimator != null && archerAnimators != null 
            && level < archerAnimators.Length 
            && archerAnimators[level] != null)
        {
            archerAnimator.runtimeAnimatorController = archerAnimators[level];
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
        return Mathf.RoundToInt(currentCost * data.sellRefundPercentage);
    }


    public void Sell()
    {
        int refund = GetRefund();

        if (GoldManager.Instance != null)
            GoldManager.Instance.AddGold(refund);
        AudioManager.Instance.PlaySfx(data.sellSfx);

        Debug.Log($"{data.towerName} sold for {refund} gold (level {currentLevel + 1}).");
        if (buildSpot != null) buildSpot.isOccupied = false;

        Destroy(gameObject);
    }

    protected virtual void Die()
    {
        isDestroyed = true;
        Debug.Log($"{data.towerName} đã bị phá hủy!");
        if (buildSpot != null) buildSpot.isOccupied = false;
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
