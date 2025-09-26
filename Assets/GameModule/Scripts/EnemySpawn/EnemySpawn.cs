using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnInfo
{
    public EnemyStats enemyStats;
    public float spawnChance;
    public int amountPerWave;
}
public class EnemySpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public List<EnemySpawnInfo> enemyPool;
    public int totalWaves = 10;
    private int currentWave = 0;
    private bool isSpawning = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSpawning)
        {
            if (currentWave < totalWaves)
            {
                StartCoroutine(SpawnWave());
            }
            else
            {
                Debug.Log("All waves completed!");
            }
        }
    }

    private IEnumerator<System.Object> SpawnWave()
    {
        isSpawning = true;
        currentWave++;
        Debug.Log("Starting Wave " + currentWave);

        foreach (var enemyInfo in enemyPool)
        {
            for (int i = 0; i < enemyInfo.amountPerWave; i++)
            {
                if (Random.value * 100 <= enemyInfo.spawnChance)
                {
                    SpawnEnemy(enemyInfo.enemyStats);
                }
                yield return new WaitForSeconds(1f);
            }
        }

        isSpawning = false;
    }

    private void SpawnEnemy(EnemyStats stats)
    {
        GameObject enemyPrefab = Resources.Load<GameObject>("EnemyPrefab");
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SetStats(stats);
        }
    }
}
