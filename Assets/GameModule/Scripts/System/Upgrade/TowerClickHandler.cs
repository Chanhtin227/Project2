using UnityEngine;
using UnityEngine.InputSystem;

public class TowerClickHandler : MonoBehaviour
{
    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                BaseTower tower = hit.collider.GetComponent<BaseTower>();
                if (tower != null)
                {
                    Debug.Log("Tower clicked: " + tower.data.towerName);
                    UIManager.Instance.ShowTowerPopup(tower);
                }
            }
        }
    }
}
