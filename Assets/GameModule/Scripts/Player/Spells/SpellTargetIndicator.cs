using UnityEngine;

public class SpellTargetIndicator : MonoBehaviour
{
    private SpellData data;

    public void Setup(SpellData spell)
    {
        data = spell;
        UpdateScale();
    }

    private void UpdateScale()
    {
        // Scale theo bán kính spell
        float diameter = data.radius * 2f;
        transform.localScale = new Vector3(diameter, diameter, 1f);
    }

    private void Update()
    {
        // Theo chuột
        if (Camera.main != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
            mousePos.z = 0f;
            transform.position = mousePos;
        }
    }
}
