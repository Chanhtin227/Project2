using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickThroughUI : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Click on {gameObject.name} (highlight)");

        // Gọi click cho các Button bên dưới
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var hit in results)
        {
            if (hit.gameObject == gameObject) continue; // bỏ chính nó

            var button = hit.gameObject.GetComponent<Button>();
            if (button != null)
            {
                Debug.Log($"Also clicked underlying button: {button.name}");
                button.onClick.Invoke();
            }
        }
    }
}
