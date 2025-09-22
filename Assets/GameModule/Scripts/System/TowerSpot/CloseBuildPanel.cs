using UnityEngine;
using UnityEngine.UI;

public class CloseBuildPanel : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            UIManager.Instance.HideBuildPanel();
        });
    }
}
