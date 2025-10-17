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
    [Header("Vung highlight")]
    public GameObject brightArea;
    public GameObject brightArea1;
    public GameObject brightArea2;
    public GameObject brightArea3;
    public GameObject brightArea4;
    public GameObject brightArea5;
    public GameObject brightArea6;
    [Header("Build Spot")]
    public GameObject BuildSpot;
    public GameObject BuildSpot1;
    public GameObject BuildSpot2;
    public GameObject BuildSpot3;
    public GameObject BuildSpot4;
    [Header("Build Spot Fake Button")]
    public Button FakeBuildSpot;
    public Button FakeBuildSpot1;
    public Button FakeBuildSpot2;
    public Button FakeBuildSpot3;
    public Button FakeBuildSpot4;
    [Header("Tower Building Button")]
    public Button TowerButton;
    public Button TowerButton1;
    public Button TowerButton2;
    public Button TowerButton3;
    public Button TowerButton4;
    public Button TowerButton5;
    public Button TowerButton6;
    public Button PlaceButton;





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
        tutorialSteps.Add(TutorialStep4);
        tutorialSteps.Add(TutorialStep5);

        if (FakeBuildSpot != null)
            FakeBuildSpot.onClick.AddListener(FakeBuildSpotClicked);
        if (FakeBuildSpot1 != null)
            FakeBuildSpot1.onClick.AddListener(FakeBuildSpotClicked);
        if (FakeBuildSpot2 != null)
            FakeBuildSpot2.onClick.AddListener(FakeBuildSpotClicked);
        if (FakeBuildSpot3 != null)
            FakeBuildSpot3.onClick.AddListener(FakeBuildSpotClicked);
        if (FakeBuildSpot4 != null)
            FakeBuildSpot4.onClick.AddListener(FakeBuildSpotClicked);

        //if (TowerButton != null)
        //    TowerButton.onClick.AddListener(TowerButtonClicked);
        //if (TowerButton1 != null)
        //    TowerButton1.onClick.AddListener(TowerButtonClicked);
        //if (TowerButton2 != null)
        //    TowerButton2.onClick.AddListener(TowerButtonClicked);
        //if (TowerButton3 != null)
        //    TowerButton3.onClick.AddListener(TowerButtonClicked);
        //if (TowerButton4 != null)
        //    TowerButton4.onClick.AddListener(TowerButtonClicked);
        //if (TowerButton5 != null)
        //    TowerButton5.onClick.AddListener(TowerButtonClicked);
        //if (TowerButton6 != null)
        //    TowerButton6.onClick.AddListener(TowerButtonClicked);



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
        Debug.Log("Run 1");

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
        Debug.Log("Run 2 end step 1");
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

        FakeBuildSpot.gameObject.SetActive(true);
        FakeBuildSpot1.gameObject.SetActive(true);
        FakeBuildSpot2.gameObject.SetActive(true);
        FakeBuildSpot3.gameObject.SetActive(true);
        FakeBuildSpot4.gameObject.SetActive(true);



        brightArea.transform.position = BuildSpot.transform.position;
        brightArea1.transform.position = BuildSpot1.transform.position;
        brightArea2.transform.position = BuildSpot2.transform.position;
        brightArea3.transform.position = BuildSpot3.transform.position;
        brightArea4.transform.position = BuildSpot4.transform.position;
    }

    void TutorialStep3()
    {
        Debug.Log("Run 3 end step 2");

        FakeBuildSpot.gameObject.SetActive(false);
        FakeBuildSpot1.gameObject.SetActive(false);
        FakeBuildSpot2.gameObject.SetActive(false);
        FakeBuildSpot3.gameObject.SetActive(false);
        FakeBuildSpot4.gameObject.SetActive(false);

        highlightObject.transform.rotation = Quaternion.Euler(0, 0, 90f);
        Vector3 newPos = TowerButton.transform.position;
        newPos.x -= 2f;
        highlightObject.transform.position = newPos;

        brightArea.transform.position = TowerButton.transform.position;
        brightArea1.transform.position = TowerButton1.transform.position;
        brightArea2.transform.position = TowerButton2.transform.position;
        brightArea3.transform.position = TowerButton3.transform.position;
        brightArea4.transform.position = TowerButton4.transform.position;
        brightArea5.transform.position = TowerButton5.transform.position;
        brightArea6.transform.position = TowerButton6.transform.position;


    }

    void TutorialStep4()
    {
        Debug.Log("Run 4 end step 3");
        brightArea.SetActive(false);
        brightArea1.SetActive(false);
        brightArea2.SetActive(false);
        brightArea3.SetActive(false);
        brightArea4.SetActive(false);
        brightArea5.SetActive(false);
        brightArea6.transform.position = PlaceButton.transform.position;


        highlightObject.transform.rotation = Quaternion.Euler(0, 0, 90f);
        Vector3 newPos = PlaceButton.transform.position;
        newPos.x -= 2f;
        highlightObject.transform.position = newPos;
    }

    void TutorialStep5()
    {
        Debug.Log("Run 5 end step 2");
        brightArea6.SetActive(false);
        highlightObject.SetActive(false);

    }

    //void BuildSpotClicked()
    //{
    //    NextStep();
    //}



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

    }

    // ✅ Thêm mới: được gọi từ HighlightButtonManager khi nút 2light, 2light1,... được bấm
    public void OnHighlightButtonPressed()
    {
        Debug.Log("OnHighlightButtonPressed dg chay");
        NextStep();
    }

    public void FakeBuildSpotClicked()
    {
        Debug.Log("buildspot fake dc click");
        NextStep();

    }
}
