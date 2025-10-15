using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Overlay")]
    public GameObject darkOverlay;           // ảnh đen phủ màn hình
    public Text tutorialText;                // text hiển thị nội dung hướng dẫn
    public GameObject highlightObject;       // khung sáng quanh nút cần chỉ
    public Canvas canvas;                    // canvas chính của UI
    public GameObject brightArea;
    public GameObject brightArea1;
    public GameObject brightArea2;
    public GameObject brightArea3;
    public GameObject brightArea4;
    public GameObject BuildSpot;
    public GameObject BuildSpot1;
    public GameObject BuildSpot2;
    public GameObject BuildSpot3;
    public GameObject BuildSpot4;

    [Header("Button References")]
    public Button startButton;
    public Button settingButton;             // có thể để trống, không crash

    private int currentStep = 0;
    private List<System.Action> tutorialSteps = new List<System.Action>();

    void Start()
    {
        // ✅ Khởi tạo các bước tutorial
        tutorialSteps.Add(TutorialStep1);
        tutorialSteps.Add(TutorialStep2);
        tutorialSteps.Add(TutorialStep3);

        /*BuildSpot.onClick.AddListener(BuildSpotClicked);
        BuildSpot1.onClick.AddListener(BuildSpotClicked);
        BuildSpot2.onClick.AddListener(BuildSpotClicked);
        BuildSpot3.onClick.AddListener(BuildSpotClicked);
        BuildSpot4.onClick.AddListener(BuildSpotClicked);*/

        // ✅ Gắn sự kiện khi bấm nút Start
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        // ✅ Gắn Setting nếu có
        if (settingButton != null)
        {
            settingButton.onClick.AddListener(OnSettingClicked);
        }

        StartTutorial();
    }

    void StartTutorial()
    {
        currentStep = 0;
        ShowStep(currentStep);
    }

    void ShowStep(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= tutorialSteps.Count)
        {
            Debug.Log("Tutorial finished!");
            EndTutorial();
            return;
        }

        tutorialSteps[stepIndex].Invoke();
    }

    // 🧭 Bước 1: chỉ nút Start
    void TutorialStep1()
    {
        Debug.Log("Running Step 1: Highlight Start Button");

        darkOverlay.SetActive(true);
        if (tutorialText != null)
            tutorialText.text = "Bấm nút Start để bắt đầu!";

        highlightObject.SetActive(true);
        if (startButton != null)
        {
            // Đưa highlight đến đúng vị trí nút Start
            Vector3 newPos = startButton.transform.position;
            newPos.y += 2f; // tăng cao thêm 50 pixel
            highlightObject.transform.position = newPos;
            brightArea.transform.position = startButton.transform.position;
        }
    }

    // 🧭 Bước 2: ví dụ — tắt overlay
    void TutorialStep2()
    {
        Debug.Log("Running Step 2: End Tutorial 1");
        highlightObject.transform.rotation = Quaternion.Euler(0, 0, -45f);
        Vector3 newPos = BuildSpot4.transform.position;
        newPos.y += 1f;
        newPos.x += 1f;
        highlightObject.transform.position = newPos;
        brightArea.SetActive(true);
        brightArea1.SetActive(true);
        brightArea2.SetActive(true);
        brightArea3.SetActive(true);
        brightArea4.SetActive(true);

        brightArea.transform.position = BuildSpot.transform.position;
        brightArea1.transform.position = BuildSpot1.transform.position;
        brightArea2.transform.position = BuildSpot2.transform.position;
        brightArea3.transform.position = BuildSpot3.transform.position;
        brightArea4.transform.position = BuildSpot4.transform.position;
    }

    void TutorialStep3()
    {
        Debug.Log("Run 3 end step 2");
        brightArea.SetActive(false);
        brightArea1.SetActive(false);
        brightArea2.SetActive(false);
        brightArea3.SetActive(false);
        brightArea4.SetActive(false);
        highlightObject.SetActive(false);
    }

    void BuildSpotClicked()
    {
        NextStep();
    }

    

    // 📌 Khi người chơi bấm nút Start
    void OnStartButtonClicked()
    {
        Debug.Log("Start Button clicked!");
     //   NextStep();
    }

    void OnSettingClicked()
    {
        Debug.Log("Setting Button clicked!");
    }

    public void NextStep()
    {
        currentStep++;
        ShowStep(currentStep);
    }

    void EndTutorial()
    {
        darkOverlay.SetActive(false);
        highlightObject.SetActive(false);
        //       SetUIInteractable(true);
    }

    // ⚡ Chặn / cho phép toàn bộ UI
    //   void SetUIInteractable(bool state)
    //   {
    //       Button[] allButtons = FindObjectsOfType<Button>(true); // true = kể cả button đang bị tắt
    //
    //       foreach (Button btn in allButtons)
    //       {
    //           btn.interactable = state;
    //       }
    //   }


    // ⚡ Bật/tắt tương tác 1 nút cụ thể
    //   void SetButtonInteractable(Button btn, bool state)
    //   {
    //       if (btn != null)
    //           btn.interactable = state;
    //   }

    void OnDestroy()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartButtonClicked);

        if (settingButton != null)
            settingButton.onClick.RemoveListener(OnSettingClicked);
    }

    // ✅ Thêm mới: được gọi từ HighlightButtonManager khi nút 2light, 2light1,... được bấm
    public void OnHighlightButtonPressed()
    {
        Debug.Log("OnHighlightButtonPressed() dg chay");
        NextStep();
    }
}
