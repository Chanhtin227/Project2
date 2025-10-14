using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [Header("Drag your Setting_Panel here")]
    public GameObject settingPanel;

    // Bật/tắt panel (cho Setting_Button)
    public void ToggleSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(!settingPanel.activeSelf);
        }
    }

    // Chỉ đóng panel (cho button_close_0)
    public void CloseSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
            
        }
    }
}
