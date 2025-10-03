using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    public bool isOccupied = false;
    public Transform spawnPoint; // tuỳ chọn, nếu null thì dùng chính transform

    public void PlaceTower(GameObject towerPrefab)
    {
        if (isOccupied) return;

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        GameObject towerObj = Instantiate(towerPrefab, pos, Quaternion.identity);

        towerObj.transform.SetParent(transform);

        BaseTower tower = towerObj.GetComponent<BaseTower>();
        if (tower != null)
            tower.AssignBuildSpot(this);

        isOccupied = true;
    }
}
