using UnityEngine;

public class LevelSceneManager : MonoBehaviour
{
    [Header("Keo Stage Menu vo day")]
    public GameObject levelMenu;

    // Bật/tắt panel (cho Setting_Button)
    public void ToggleLevelMenu()
    {
        if (levelMenu != null)
        {
            levelMenu.SetActive(!levelMenu.activeSelf);
        }
    }

    // Chỉ đóng panel (cho button_close_0)
    public void CloseLevelMenu()
    {
        if (levelMenu != null)
        {
            levelMenu.SetActive(false);

        }
    }
}
