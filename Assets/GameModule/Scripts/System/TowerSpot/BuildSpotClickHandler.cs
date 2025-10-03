using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BuildSpotClickHandler : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (UIManager.Instance.IsAnyPopupOpen())
            {
                return;
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