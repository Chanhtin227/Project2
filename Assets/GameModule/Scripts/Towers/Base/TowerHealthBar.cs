using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour
{
    public BaseTower tower; // trỏ tới tower
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 1.5f, 0); // vị trí trên đầu tower

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (tower == null)
            tower = GetComponentInParent<BaseTower>();
    }

    void Update()
    {
        if (tower == null) return;

        // cập nhật fill theo máu
        fillImage.fillAmount = Mathf.Clamp01(tower.CurrentHealth / tower.MaxHealth);

        // // xoay UI luôn hướng về camera
        // if (cam != null)
        //     transform.rotation = cam.transform.rotation;

        // offset vị trí
        transform.position = tower.transform.position + offset;
    }
}
