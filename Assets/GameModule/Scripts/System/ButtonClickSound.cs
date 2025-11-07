using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    void Start()
    {
        // Lấy button trên GameObject hiện tại
        Button btn = GetComponent<Button>();

        // Khi bấm nút, phát âm thanh click
        btn.onClick.AddListener(() =>
        {
            if (AudioManager.Instance != null && AudioManager.Instance.clickSound != null)
            {
                AudioManager.Instance.PlaySfx(AudioManager.Instance.clickSound);
            }
        });
    }
}
