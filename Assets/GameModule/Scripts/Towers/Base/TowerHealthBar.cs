using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour
{
    [Header("References")]
    public BaseTower tower;
    public Image fillImage;

    [Header("Offset per Level")]
    public Vector3[] levelOffsets = new Vector3[]
    {
        new Vector3(0, 1.2f, 0), // Level 1
        new Vector3(0, 1.5f, 0), // Level 2
        new Vector3(0, 1.8f, 0)  // Level 3
    };

    private Vector3 currentOffset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (tower == null)
            tower = GetComponentInParent<BaseTower>();

        UpdateOffset();
    }

    void Update()
    {
        if (tower == null) return;

        fillImage.fillAmount = Mathf.Clamp01(tower.CurrentHealth / tower.MaxHealth);
        transform.position = tower.transform.position + currentOffset;
        transform.rotation = Quaternion.identity; // Giữ hướng đúng
    }

    /// <summary>
    /// Cập nhật offset theo cấp độ tower hiện tại
    /// </summary>
    public void UpdateOffset()
    {
        if (tower == null) return;

        int lvl = tower.currentLevel;
        if (lvl < levelOffsets.Length)
            currentOffset = levelOffsets[lvl];
        else
            currentOffset = levelOffsets[levelOffsets.Length - 1]; // Nếu vượt giới hạn, lấy offset cuối
    }
}
