using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTower : BaseTower
{
    [Header("Buff Settings")]
    public float buffRange = 4f;
    [Range(0f, 1f)] public float attackBuffPercent = 0.25f;
    [Range(0f, 1f)] public float fireRateBuffPercent = 0.25f;
    public float buffDuration = 5f;
    public float buffInterval = 6f;

    [Header("Buff Visual")]
    public GameObject buffRangePrefab; // Prefab hình tròn cho vùng buff
    private GameObject buffVisual;

    private float buffTimer;

    protected override void Start()
    {
        base.Start();
        buffTimer = buffInterval;

        // --- Tạo vòng hiển thị buff ---
        if (buffRangePrefab != null)
        {
            buffVisual = Instantiate(buffRangePrefab, transform);
            buffVisual.transform.localPosition = Vector3.zero;
            float scale = buffRange * 2f;
            buffVisual.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    protected override void Update()
    {
        base.Update();

        buffTimer -= Time.deltaTime;
        if (buffTimer <= 0f)
        {
            ApplyBuffToAllTowers();
            buffTimer = buffInterval;
        }
    }

    protected override void Shoot() { } // Support tower không bắn

    void ApplyBuffToAllTowers()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, buffRange);

        foreach (var hit in hits)
        {
            BaseTower tower = hit.GetComponent<BaseTower>();
            if (tower != null && tower != this)
            {
                StartCoroutine(BuffTowerTemporary(tower));
            }
        }
    }

    IEnumerator BuffTowerTemporary(BaseTower tower)
    {
        TowerData.TowerLevel lvl = tower.data.levels[tower.currentLevel];

        float originalDamage = lvl.damage;
        float originalFireRate = lvl.fireRate;

        lvl.damage = originalDamage * (1f + attackBuffPercent);
        lvl.fireRate = originalFireRate * (1f - fireRateBuffPercent);

        typeof(BaseTower).GetMethod("ApplyStats",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(tower, null);

        yield return new WaitForSeconds(buffDuration);

        lvl.damage = originalDamage;
        lvl.fireRate = originalFireRate;

        typeof(BaseTower).GetMethod("ApplyStats",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(tower, null);
    }

    // --- Vẽ vòng phạm vi trong Editor ---
    void OnDrawGizmos()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, buffRange);
    }
}
