using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPoints;
    public int pathId = 0;
    public void Spawn(GameObject enemyPrefab)
    {
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPoints.position, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SetPath(MultiPathCheckpointsManager.main.GetPath(pathId));
            GameManager.Instance?.RegisterEnemy();
        }
        
    }
}
