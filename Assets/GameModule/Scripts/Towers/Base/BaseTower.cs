using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    #region === Tower Data ===
    [Header("Tower Data")]
    public TowerData data;
    public int currentLevel = 0;

    protected float range;
    public float Range => range;
    protected float damage;
    protected float fireRate;
    #endregion

    #region === Health ===
    [Header("Health")]
    protected float maxHealth;
    public float currentHealth;
    public bool isDestroyed { get; private set; }
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    #endregion

    #region === Targeting ===
    [Header("Targeting")]
    public LayerMask enemyLayer;
    public Transform firePoint;
    protected Transform target;
    float fireCooldown;
    private bool isPlayingShootAnim = false;
    #endregion

    #region === Visuals ===
    [Header("Visual (2D)")]
    [SerializeField] private SpriteRenderer towerBaseRenderer;
    [SerializeField] private Sprite[] baseSprites;
    [SerializeField] public Animator archerAnimator;
    [SerializeField] public RuntimeAnimatorController[] archerAnimators;
    #endregion

    #region === Build & Sell ===
    [Header("Sell Settings")]
    [Range(0f, 1f)] public float sellRefundPercent = 0.5f;
    public BuildSpot buildSpot { get; private set; }

    public void AssignBuildSpot(BuildSpot spot)
    {
        buildSpot = spot;
    }
    #endregion

    #region === Range Visual (Runtime) ===
    [Header("Range Visual")]
    public GameObject rangeVisualPrefab;
    private GameObject rangeVisualInstance;
    #endregion

    #region === Unity Lifecycle ===
    protected virtual void Start()
    {
        ApplyStats();
        currentHealth = maxHealth;
        CreateRangeVisual();
    }

    protected virtual void Update()
    {
        if (isDestroyed) return;

        UpdateTarget();
        fireCooldown -= Time.deltaTime;

        if (target == null || Vector2.Distance(transform.position, target.position) > range)
            AcquireTarget();

        if (target == null)
        {
            isPlayingShootAnim = false;
            return;
        }

        if (fireCooldown <= 0f && !isPlayingShootAnim)
        {
            if (CanShoot())
            {
                if (archerAnimator != null)
                {
                    isPlayingShootAnim = true;
                    archerAnimator.SetTrigger("isShoot");
                }
                else
                {
                    Shoot();
                }
                fireCooldown = fireRate;
            }
        }

        if (isPlayingShootAnim && archerAnimator != null && archerAnimator.runtimeAnimatorController != null)
        {
            AnimatorStateInfo stateInfo = archerAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("idle") ||
                (stateInfo.IsName("Shoot") && stateInfo.normalizedTime >= 0.95f))
            {
                isPlayingShootAnim = false;
            }
        }
    }
    #endregion

    #region === Targeting Logic ===
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

        target = nearestEnemy;
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
    #endregion

    #region === Stats & Upgrade ===
    public void Upgrade()
    {
        if (currentLevel + 1 < data.levels.Length)
        {
            currentLevel++;
            AudioManager.Instance.PlaySfx(data.upgradeSfx);
            Debug.Log($"{data.towerName} upgraded to level {currentLevel + 1}.");
            ApplyStats();

            TowerHealthBar healthBar = GetComponentInChildren<TowerHealthBar>();
            if (healthBar != null)
                healthBar.UpdateOffset();
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
        UpdateRangeVisualSize();
    }

    void UpdateVisual(int level)
    {
        if (towerBaseRenderer != null && level < baseSprites.Length)
            towerBaseRenderer.sprite = baseSprites[level];

        if (archerAnimator != null && archerAnimators != null
            && level < archerAnimators.Length
            && archerAnimators[level] != null)
        {
            archerAnimator.runtimeAnimatorController = archerAnimators[level];
        }
    }
    #endregion

    #region === Combat & Health ===
    public virtual void TakeDamage(float amount)
    {
        if (isDestroyed) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if (isDestroyed) return;

        isDestroyed = true;
        Debug.Log($"{data.towerName} đã bị phá hủy!");

        if (buildSpot != null)
            buildSpot.isOccupied = false;

        Destroy(gameObject);
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
    #endregion

    #region === Sell & Refund ===
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

        if (buildSpot != null)
            buildSpot.isOccupied = false;

        Destroy(gameObject);
    }
    #endregion

    #region === Range Visual Runtime ===
    void CreateRangeVisual()
    {
        if (rangeVisualPrefab == null) return;

        rangeVisualInstance = Instantiate(rangeVisualPrefab, transform);
        rangeVisualInstance.transform.localPosition = Vector3.zero;

        UpdateRangeVisualSize();
        rangeVisualInstance.SetActive(true);
    }

    void UpdateRangeVisualSize()
    {
        if (rangeVisualInstance != null)
        {
            float scale = range * 1.4f;
            rangeVisualInstance.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
    #endregion

    #region === Gizmos (Editor Only) ===
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    #endregion

    #region === Range Visual Manager Integration ===
    protected virtual void OnEnable()
    {
        if (TowerRangeManager.Instance != null)
        {
            TowerRangeManager.Instance.RegisterTower(this);
        }
        else
        {
            StartCoroutine(WaitForManagerAndRegister());
        }
    }

    private System.Collections.IEnumerator WaitForManagerAndRegister()
    {
        yield return new WaitUntil(() => TowerRangeManager.Instance != null);
        TowerRangeManager.Instance.RegisterTower(this);
    }

    protected virtual void OnDisable()
    {
        if (TowerRangeManager.Instance != null)
            TowerRangeManager.Instance.UnregisterTower(this);
    }

    public void SetRangeVisible(bool visible)
    {
        if (rangeVisualInstance != null)
            rangeVisualInstance.SetActive(visible);
    }
    #endregion
}
