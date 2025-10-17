using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;   // ⚡ Quan trọng – thêm dòng này để dùng New Input System

public class BuildSpotManager : MonoBehaviour
{
    [Header("All Build Spots in Scene")]
    public List<GameObject> buildSpots = new List<GameObject>();

    void Update()
    {
        // Kiểm tra xem chuột có tồn tại không (phòng trường hợp mobile không có chuột)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Lấy vị trí chuột theo màn hình
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // Chuyển sang toạ độ thế giới (2D)
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Bắn raycast 2D xuống tất cả collider tại vị trí đó
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider != null && buildSpots.Contains(hit.collider.gameObject))
                {
                    Debug.Log("Clicked BuildSpot: " + hit.collider.name);
                    OnBuildSpotClicked(hit.collider.gameObject);
                }
            }
        }

        // Nếu bạn muốn hỗ trợ cảm ứng trên điện thoại, thêm đoạn này:
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider != null && buildSpots.Contains(hit.collider.gameObject))
                {
                    Debug.Log("Touched BuildSpot: " + hit.collider.name);
                    OnBuildSpotClicked(hit.collider.gameObject);
                }
            }
        }
    }

    public void OnBuildSpotClicked(GameObject spot)
    {
        Debug.Log("==> " + spot.name + " được click!");
        // Logic xử lý build tower hoặc mở menu ở đây
        // spot.GetComponent<BuildSpot>().TryBuildTower();
    }
}
