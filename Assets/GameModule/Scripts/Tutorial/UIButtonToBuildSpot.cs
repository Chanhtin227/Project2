using UnityEngine;
using UnityEngine.UI;

public class UIButtonToBuildSpot : MonoBehaviour
{
    [Header("Build Spot bạn muốn giả lập click")]
    public BuildSpot targetSpot;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnClickTrigger);
        else
            Debug.LogWarning("UIButtonBuildProxy cần gắn trên một Button!");
    }

    void OnClickTrigger()
    {
        if (targetSpot == null)
        {
            Debug.LogWarning("⚠️ Chưa gán BuildSpot cho UIButtonBuildProxy!");
            return;
        }

        if (UIManager.Instance == null)
        {
            Debug.LogError("❌ Không tìm thấy UIManager.Instance!");
            return;
        }

        Debug.Log($"🔹 Đã giả lập click lên BuildSpot: {targetSpot.name}");
        UIManager.Instance.ShowBuildPanel(targetSpot);
    }
}
