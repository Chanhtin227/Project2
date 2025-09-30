using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPoints;
    public void Spawn(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, spawnPoints.position, Quaternion.identity);
    }
}
