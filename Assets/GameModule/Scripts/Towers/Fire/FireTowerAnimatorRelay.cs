using UnityEngine;

public class FireTowerAnimatorRelay : MonoBehaviour
{
    [SerializeField] private FireTower parentTower; // Kéo thủ công trong Inspector

    void Awake()
    {
        if (parentTower == null)
            parentTower = GetComponentInParent<FireTower>();
    }

    // Gọi từ Animation Event
    public void OnShootEvent()
    {
        if (parentTower != null)
        {
            parentTower.SpawnProjectileAtTarget();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy FireTower trong parent hoặc chưa gán trong Inspector!");
        }
    }
}
