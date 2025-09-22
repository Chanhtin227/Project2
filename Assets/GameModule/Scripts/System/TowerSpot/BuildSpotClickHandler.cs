using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BuildSpotClickHandler : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Kiểm tra xem có đang click vào UI không
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; // Không xử lý nếu đang click UI
            }

            // Kiểm tra xem có popup nào đang mở không
            if (UIManager.Instance.IsAnyPopupOpen())
            {
                return; // Không xử lý nếu có popup đang mở
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                BuildSpot spot = hit.collider.GetComponent<BuildSpot>();
                if (spot != null && !spot.isOccupied)
                {
                    Debug.Log("Click vào BuildSpot");
                    UIManager.Instance.ShowBuildPanel(spot);
                }
            }
        }
    }
}