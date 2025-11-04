using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    public EnemyStats Stats => stats;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator _anim;
    private Transform[] path;
    private Transform checkpoint;
    private int index = 0;
    private int currentHP;
    private bool isSlowed = false;
    private IEnemyAbility ability;
    private float baseSpeed;
    private float currentSpeedMultiplier = 1f;
    private float currentMoveSpeed;
    private float baseArmor;
    private float currentArmor;
    private bool isAttacking = false;
    //add
    private OutlineController outlineController;
    private Coroutine dotCoroutine;
    private bool isStunned = false;
    private bool isParalyzed = false;
    [SerializeField] private GoldPopup goldPopupPrefab;


    public void AddSpeedMultiplier(float speed)
    {
        currentSpeedMultiplier *= speed;
        currentMoveSpeed = baseSpeed * currentSpeedMultiplier;
    }

    public void ResetSpeedMultiplier()
    {
        currentSpeedMultiplier = 1f;
        currentMoveSpeed = baseSpeed;
    }

    public void AddArmorMultiplier(float armor)
    {
        currentArmor += armor;
    }

    public void ResetArmorMultiplier(float armor)
    {
        currentArmor -= armor;
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, stats.health);
    }

    public void SetAttackStage(bool value)
    {
        isAttacking = value;
        if (_anim != null)
        {
            _anim.SetBool("isAttack", value);
        }
    }

    public void StopMove()
    {
        rb.linearVelocity = Vector2.zero;
        currentMoveSpeed = 0;
        _anim?.SetBool("isWalk", false);
        isAttacking = true;
    }

    public void ResumeMove()
    {
        currentMoveSpeed = baseSpeed;
        _anim?.SetBool("isWalk", true);
        isAttacking = false;
    }
    public int GetCurrentHP()
    {
        return currentHP;
    }

    public int GetMaxHP()
    {
        return stats.health;
    }

    public Transform[] GetPath()
    {
        return path;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        baseSpeed = stats.moveSpeed;
        currentMoveSpeed = baseSpeed;
        baseArmor = stats.armor;
        currentArmor = baseArmor;
        currentHP = stats.health;
        ability = EnemyAbilityFactory.CreateAbility(stats.specialType);
        if (ability != null)
        {
            ability.Init(this, stats);
        }
        // add
        outlineController = GetComponent<OutlineController>();
    }

    void Start()
    {
        if (path != null && path.Length > 0)
        {
            checkpoint = path[index];
        }

    }

    void Update()
    {
        ability?.Update();
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
        if (checkpoint == null || isAttacking) return;

        Vector2 direction = (checkpoint.position - transform.position).normalized;
        rb.linearVelocity = direction * currentMoveSpeed;

        if (rb.linearVelocity.x > 0.1f) spriteRenderer.flipX = false;
        else if (rb.linearVelocity.x < -0.1f) spriteRenderer.flipX = true;
    }

    public void SetPath(Transform[] newPath)
    {
        path = newPath;
        index = 0;
        if (path != null && path.Length > 0)
        {
            checkpoint = path[0];
            index = 0;
        }
    }

    public void TakeDamage(int dmg)
    {
        // Công thức giảm dần: 100 / (100 + armor)
        float multiplier = 100f / (100f + currentArmor);
        int finalDamage = Mathf.RoundToInt(dmg * multiplier);
        currentHP -= finalDamage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ability?.OnDeath();
        _anim.SetTrigger("isDead");

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(stats.goldReward);
        }

        // 🟡 Hiển thị popup vàng
        if (goldPopupPrefab != null)
        {
            var popup = Instantiate(goldPopupPrefab, transform.position, Quaternion.identity);
            popup.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            popup.Setup(stats.goldReward);
        }

        Debug.Log($"{stats.enemyName} chết, nhận {stats.goldReward} vàng!");
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        this.enabled = false;
        Destroy(gameObject, 1.3f);
    }


    public void ApplySlow(float slowAmount, float duration)
    {
        if (isSlowed) return;
        StartCoroutine(SlowRoutine(slowAmount, duration));
    }
    private IEnumerator SlowRoutine(float slowAmount, float duration)
    {
        isSlowed = true;
        currentMoveSpeed *= (1f - slowAmount);

        yield return new WaitForSeconds(duration);

        currentMoveSpeed = baseSpeed;
        isSlowed = false;
    }



    //================================= add=====================================
    public void SetOutline(bool show)
    {
        if (outlineController != null)
            outlineController.ShowOutline(show);
    }

    // DOT áp dụng damage over time
    public void ApplyDOT(float dps, float duration)
    {
        if (dotCoroutine != null)
            StopCoroutine(dotCoroutine);
        dotCoroutine = StartCoroutine(DOTRoutine(dps, duration));
    }

    private IEnumerator DOTRoutine(float dps, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration && !IsDead())
        {
            int dmg = Mathf.RoundToInt(dps * 1f); // tick mỗi 1s
            TakeDamage(dmg);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        dotCoroutine = null;
    }
    
    public void ApplyStun(float duration)
    {
        if (isStunned) return;
        StartCoroutine(StunRoutine(duration));
    }

    public void ApplyParalyze(float duration)
    {
        if (isParalyzed) return;
        StartCoroutine(ParalyzeRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        currentMoveSpeed = 0f;
        // có thể thêm animation "Stunned" ở đây
        yield return new WaitForSeconds(duration);
        currentMoveSpeed = baseSpeed;
        isStunned = false;
    }

    private IEnumerator ParalyzeRoutine(float duration)
    {
        isParalyzed = true;
        currentMoveSpeed *= 0.2f; // di chuyển chậm 80%
        // có thể thêm hiệu ứng điện ở đây
        yield return new WaitForSeconds(duration);
        currentMoveSpeed = baseSpeed;
        isParalyzed = false;
    }

}