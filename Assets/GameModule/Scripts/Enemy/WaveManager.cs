using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public float waveDelay = 15;
}

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    void Update()
    {
        if (!isSpawning && currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }
        IEnumerator SpawnWave(Wave wave)
        {
            isSpawning = true;
            Debug.Log($"Starting Wave {currentWaveIndex + 1}");
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
            Debug.Log($"Wave {currentWaveIndex + 1} ended");
            currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            yield return new WaitForSeconds(wave.waveDelay);
            isSpawning = false;
        }
        else
        {
            Debug.Log("All waves completed!");
        }
        }
}