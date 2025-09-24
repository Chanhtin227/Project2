using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<GameObject> enemies;  // list các prefab quái trong wave
}

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    public Transform spawnPoint;

    public float spawnInterval = 1f; // 1 giây giữa các quái
    public float waveTimeout = 20f;  // qua wave sau nếu 20s sau khi spawn xong mà quái chưa chết hết

    private int currentWaveIndex = 0;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool spawningWave = false;
    private float lastSpawnTime = 0f;
    private bool waveCompleted = false;

    void Update()
    {
        // Dọn list quái chết (null)
        aliveEnemies.RemoveAll(e => e == null);

        if (!spawningWave && !waveCompleted)
        {
            Debug.Log($"--- Wave {currentWaveIndex + 1} started ---");
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }

        // Điều kiện qua wave
        if (waveCompleted)
        {
            bool allDead = aliveEnemies.Count == 0;
            bool timeout = Time.time - lastSpawnTime >= waveTimeout;

            if (allDead || timeout)
            {
                Debug.Log($"--- Wave {currentWaveIndex + 1} ended ---");
                GoToNextWave();
            }
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        spawningWave = true;

        for (int i = 0; i < wave.enemies.Count; i++)
        {
            GameObject enemyPrefab = wave.enemies[i];
            if (enemyPrefab != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                aliveEnemies.Add(enemy);
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        spawningWave = false;
        waveCompleted = true;
        lastSpawnTime = Time.time;
    }

    void GoToNextWave()
    {
        waveCompleted = false;
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("=== All waves finished! ===");
        }
        else
        {
            Debug.Log($"Preparing wave {currentWaveIndex + 1}...");
        }
    }
}
