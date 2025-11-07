using UnityEngine;
using System.Collections;

public class Boss : IEnemyAbility
{
    private Enemy boss;
    private EnemyStats stats;
    private float nextActionTime = 0;
    private bool isEnraged = false;
    private WaveManager waveManager;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isColorChanged = false;

    public void Init(Enemy boss, EnemyStats stats)
    {

        this.boss = boss;
        this.stats = stats;
        waveManager = GameObject.FindObjectOfType<WaveManager>();
        nextActionTime = Time.time + stats.cooldown;

        spriteRenderer = boss.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        AudioManager.Instance.StopMusic(); 
        AudioManager.Instance.PlayMusic(AudioManager.Instance.bossMusic);
    }

    public void Update()
    {
        if (boss.IsDead()) return;
        if (!isEnraged && boss.GetCurrentHP() <= boss.GetMaxHP() * 0.3f)
        {
            isEnraged = true;
            OnEnrage();
            Debug.Log($"{boss.name} đã trở nên điên cuồng!");
        }
        if (Time.time >= nextActionTime)
        {
            RandomActionSkill();
            nextActionTime = Time.time + stats.cooldown;
        }
    }

    private void RandomActionSkill()
    {
        if (boss == null || boss.IsDead()) return;
        int skillChoice = Random.Range(0, 3);
        switch (skillChoice)
        {
            case 0:
                DamageNearTowers();
                break;
            case 1:
                SummonEnemies();
                break;
            case 2:
                DisableTowers();
                break;
        }
    }

    private void DamageNearTowers()
    {
        float damage = stats.specialValue;
        if (isEnraged)
        {
            damage *= 2;
        }
        Collider2D[] towers = Physics2D.OverlapCircleAll(boss.transform.position, stats.buffRadius);
        int count = 0;
        foreach (var hit in towers)
        {
            BaseTower tower = hit.GetComponent<BaseTower>();
            if (tower != null && !tower.IsDestroyed())
            {
                tower.TakeDamage(damage);
                count++;
            }
        }
        Debug.Log($"{boss.name} đã gây sát thương cho {count} tháp gần đó!");
    }

    private void SummonEnemies()
    {
        if (waveManager == null) return;
        int countEnemy = isEnraged ? Random.Range(3, 6) : Random.Range(1, 4);
        for (int i = 0; i < countEnemy; i++)
        {
            Enemy randomEnemy = waveManager.GetRandomEnemyType(excludeBoss: true);
            if (randomEnemy == null) continue;
            Vector3 spawnPosition = boss.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject clone = GameObject.Instantiate(randomEnemy.gameObject, spawnPosition, Quaternion.identity);
            Enemy newEnemy = clone.GetComponent<Enemy>();
            if (newEnemy != null)
            {
                newEnemy.SetPath(boss.GetPath());
            }
        }
    }

    private void DisableTowers()
    {
        float radius = stats.buffRadius;
        float disableTime = stats.duration;
        int count = 0;
        Collider2D[] towers = Physics2D.OverlapCircleAll(boss.transform.position, radius);
        foreach (var hit in towers)
        {
            BaseTower tower = hit.GetComponent<BaseTower>();
            if (tower != null && !tower.IsDestroyed())
            {
                boss.StartCoroutine(DisableTowerTemporarily(tower, disableTime));
                count++;
            }
        }
        Debug.Log($"{boss.name} đã làm vô hiệu hóa {count} tháp gần đó trong {disableTime} giây!");
    }

    private IEnumerator DisableTowerTemporarily(BaseTower tower, float duration)
    {
        if (tower == null) yield break;
        tower.SetActiveStage(false);
        SpriteRenderer towerSprite = tower.GetComponent<SpriteRenderer>();
        Color oldColor = Color.white;
        if (towerSprite != null)
        {
            oldColor = towerSprite.color;
            towerSprite.color = Color.gray;
        }
        yield return new WaitForSeconds(duration);
        tower.SetActiveStage(true);
        if (towerSprite != null)
        {
            towerSprite.color = oldColor;
        }
    }

    private void OnEnrage()
    {
        if (spriteRenderer != null && !isColorChanged)
        {
            boss.StartCoroutine(EnrageEffect());
            isColorChanged = true;
        }
    }

    private IEnumerator EnrageEffect()
    {
        float t = 0f;
        while (isEnraged)
        {
            t += Time.deltaTime * 4f;
            float intensity = (Mathf.Sin(t) + 1f) * 0.5f;
            Color flashColor = Color.Lerp(originalColor, Color.red, intensity);
            spriteRenderer.color = flashColor;
            yield return null;
        }
    }
    
    public void OnDeath()
    {
        isEnraged = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
    }
}
