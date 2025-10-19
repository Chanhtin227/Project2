using UnityEngine;
using UnityEngine.UI;

public class HighlightButtonManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button lightButton;
    public Button lightButton1;
    public Button lightButton2;
    public Button lightButton3;
    public Button lightButton4;
    public Button lightButton5;
    public Button lightButton6;



    [Header("Tutorial Manager")]
    public TutorialManager tutorialManager;  // 👈 thêm dòng này

    void Start()
    {
        if (lightButton != null)
            lightButton.onClick.AddListener(OnHighlightAreaClicked);
        if (lightButton1 != null)
            lightButton1.onClick.AddListener(OnHighlightAreaClicked);
        if (lightButton2 != null)
            lightButton2.onClick.AddListener(OnHighlightAreaClicked);
        if (lightButton3 != null)
            lightButton3.onClick.AddListener(OnHighlightAreaClicked);
        if (lightButton4 != null)
            lightButton4.onClick.AddListener(OnHighlightAreaClicked);
        if (lightButton5 != null)
            lightButton5.onClick.AddListener(OnHighlightAreaClicked);
        if (lightButton6 != null)
            lightButton6.onClick.AddListener(OnHighlightAreaClicked);

    }

    void OnHighlightAreaClicked()
    {
        Debug.Log("click vao o 2lai");

        // 👇 Gọi TutorialManager khi có nút bấm
        if (tutorialManager != null)
        {
            tutorialManager.OnHighlightButtonPressed();
        }
        else
        {
            Debug.LogWarning("Chưa gắn TutorialManager trong Ins");
        }
    }
}
