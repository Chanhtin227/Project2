using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count;
    public float interval = 1f;
    public EnemySpawner spawner;
}

[System.Serializable]
public class Wave
{
    public List<EnemySpawnInfo> enemies;
}

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool waitingForNext = true;

    // Event để thông báo wave này đã spawn xong
    public UnityEvent OnWaveEnded;

    void Awake()
    {
        if (OnWaveEnded == null)
            OnWaveEnded = new UnityEvent();
    }

    // Gọi để bắt đầu wave kế tiếp (được gọi từ button)
    public void StartNextWave()
    {
        if (!isSpawning && waitingForNext && currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            waitingForNext = false;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;
        Debug.Log($"[{name}] Starting Wave {currentWaveIndex + 1}");

        foreach (var enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                if (enemyInfo.spawner != null && enemyInfo.enemyPrefab != null)
                {
                    enemyInfo.spawner.Spawn(enemyInfo.enemyPrefab);
                }
                yield return new WaitForSeconds(enemyInfo.interval);
            }
        }

        Debug.Log($"[{name}] Wave {currentWaveIndex + 1} ended");

        currentWaveIndex++;
        isSpawning = false;
        waitingForNext = true;

        // Thông báo cho listener biết wave đã xong
        OnWaveEnded?.Invoke();

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log($"[{name}] All waves completed!");
        }
    }

    // Expose trạng thái để StartButton kiểm tra
    public bool IsSpawning() => isSpawning;
    public bool IsWaitingForNext() => waitingForNext;
}
