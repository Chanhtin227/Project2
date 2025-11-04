using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class TutorialManager : MonoBehaviour
{
    [Header("UI Overlay")]
    public GameObject darkOverlay;           // ảnh đen phủ màn hình
    public Text tutorialText;
    public GameObject highlightObject;       // khung sáng quanh nút cần chỉ
    public Canvas canvas;                    // canvas chính của UI

    [Header("Text typing effect")]
    public float typingSpeed = 0.03f;     // thời gian giữa mỗi ký tự
    private Coroutine typingCoroutine;    // lưu coroutine hiện tại
    private string fullText = "";         // lưu toàn bộ nội dung text cần hiển thị
    private bool isTyping = false;        // đang gõ chữ hay không



    [Header("Vung highlight")]
    public GameObject brightArea;
    public GameObject brightArea1;
    public GameObject brightArea2;
    public GameObject brightArea3;
    public GameObject brightArea4;
    public GameObject brightArea5;
    public GameObject brightArea6;
    public GameObject brightArea7;
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
    public Button PlaceButton;
    [Header("Spell Button")]
    public Button SpellButton;
    public Button SpellButton1;
    public Button SpellButton2;
    public Button SpellButton3;
    public Button SpellButton4;
    public Button SpellButton5;
    public Button SpellButton6;
    public Button SpellButton7;

    [Header("Button References")]
    public Button startButton;
    public Button settingButton;// có thể để trống, không crash

    private HashSet<Button> lockedButtons = new HashSet<Button>();

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
        tutorialSteps.Add(TutorialStep6);
        tutorialSteps.Add(TutorialStep7);
        tutorialSteps.Add(TutorialStep8);
        tutorialSteps.Add(TutorialStep9);
        tutorialSteps.Add(TutorialStep10);


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

    void Update()
    {
        // Force khóa button mỗi frame
        foreach (Button btn in lockedButtons)
        {
            if (btn != null)
                btn.interactable = false;
        }
    }

    // Gọi khi bạn muốn tạm khóa 1 nút (bất kể SpellButton.cs đang làm gì)
    public void LockButton(Button btn)
    {
        if (btn != null)
        {
            lockedButtons.Add(btn);
            btn.interactable = false;
        }
    }

    // Gọi khi bạn muốn mở khóa lại
    public void UnlockButton(Button btn)
    {
        if (btn != null)
        {
            lockedButtons.Remove(btn);
            // Khi unlock xong, SpellButton sẽ tự quản lý lại interactable theo cooldown
        }
    }

    // Nếu muốn mở toàn bộ
    public void UnlockAll()
    {
        foreach (Button btn in lockedButtons)
        {
            if (btn != null)
                btn.interactable = true;
        }
        lockedButtons.Clear();
    }


    public void LockAllSpellButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);
        foreach (Button btn in allButtons)
        {
            if (btn.CompareTag("SpellButton"))
            {
                btn.interactable = false;

                CanvasGroup cg = btn.GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = btn.gameObject.AddComponent<CanvasGroup>();

                cg.blocksRaycasts = false;
                cg.interactable = false;
            }
        }
    }

    public void UnlockAllSpellButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);
        foreach (Button btn in allButtons)
        {
            if (btn.CompareTag("SpellButton"))
            {
                btn.interactable = true;

                CanvasGroup cg = btn.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.blocksRaycasts = true;
                    cg.interactable = true;
                }
            }
        }
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
    
    IEnumerator WaitAndNextStep(float waitTime)  //StartCoroutine(WaitAndNextStep(3f)); // vi du code cho doi 3s roi tu next step
    {
        yield return new WaitForSeconds(waitTime);
        NextStep();
    }

    // 🧭 Bước 1: chỉ nút Start
    void TutorialStep1()
    {
        brightArea.transform.localScale = new Vector3(0.0225f, 0.0225f, 0.009255994f);
        brightArea1.transform.localScale = new Vector3(0.0225f, 0.0225f, 0.009255994f);
        brightArea2.transform.localScale = new Vector3(0.0225f, 0.0225f, 0.009255994f);
        brightArea3.transform.localScale = new Vector3(0.0225f, 0.0225f, 0.009255994f);
        brightArea4.transform.localScale = new Vector3(0.0225f, 0.0225f, 0.009255994f);
        brightArea5.transform.localScale = new Vector3(0.0225f, 0.0225f, 0.009255994f);
        brightArea6.transform.localScale = new Vector3(0.0225f, 0.0225f, 0.009255994f);


        Debug.Log("Run 1");

        darkOverlay.SetActive(true);
        if (tutorialText != null)
            ShowTutorialText("To start the game, click the lower right button!");


        highlightObject.SetActive(true);
        if (startButton != null)
        {
            // Đưa highlight đến đúng vị trí nút Start
            highlightObject.transform.rotation = Quaternion.Euler(0, 0, -90f);
            Vector3 newPos = startButton.transform.position;
            newPos.y += 2f; // tăng cao thêm 2 pixel
            highlightObject.transform.position = newPos;
            brightArea.transform.position = startButton.transform.position;
        }
       LockAllExcept(startButton,brightArea1.GetComponent<Button>(), brightArea2.GetComponent<Button>(), brightArea3.GetComponent<Button>(), brightArea4.GetComponent<Button>(), brightArea5.GetComponent<Button>(), brightArea6.GetComponent<Button>(), brightArea.GetComponent<Button>());

    }

    // 🧭 Bước 2: ví dụ — tắt overlay
    void TutorialStep2()
    {
        darkOverlay.SetActive(false);

        if (tutorialText != null)
            ShowTutorialText("Select a build spot to build your tower!");
 
         UnlockAll();
        LockAllExcept(startButton, FakeBuildSpot, FakeBuildSpot1, FakeBuildSpot2, FakeBuildSpot3, FakeBuildSpot4, brightArea1.GetComponent<Button>(), brightArea2.GetComponent<Button>(), brightArea3.GetComponent<Button>(), brightArea4.GetComponent<Button>(), brightArea5.GetComponent<Button>(), brightArea6.GetComponent<Button>(), brightArea.GetComponent<Button>());
        Debug.Log("Run 2 end step 1");
        highlightObject.transform.rotation = Quaternion.Euler(0, 0, -135f);
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

        if (tutorialText != null)
            ShowTutorialText("Select a tower to build, each tower has its own strength!");

        UnlockAll();
        LockAllExcept(startButton,FakeBuildSpot, FakeBuildSpot1, FakeBuildSpot2, FakeBuildSpot3, FakeBuildSpot4, brightArea1.GetComponent<Button>(), brightArea2.GetComponent<Button>(), brightArea3.GetComponent<Button>(), brightArea4.GetComponent<Button>(), brightArea5.GetComponent<Button>(), brightArea6.GetComponent<Button>(), brightArea.GetComponent<Button>());

        FakeBuildSpot.gameObject.SetActive(false);
        FakeBuildSpot1.gameObject.SetActive(false);
        FakeBuildSpot2.gameObject.SetActive(false);
        FakeBuildSpot3.gameObject.SetActive(false);
        FakeBuildSpot4.gameObject.SetActive(false);

        highlightObject.transform.rotation = Quaternion.Euler(0, 0, 0f);
        Vector3 newPos = TowerButton.transform.position;
        newPos.x -= 2f;
        highlightObject.transform.position = newPos;

        brightArea.transform.position = TowerButton.transform.position;
        brightArea1.transform.position = TowerButton1.transform.position;
        brightArea2.transform.position = TowerButton2.transform.position;
        brightArea3.transform.position = TowerButton3.transform.position;
        brightArea4.transform.position = TowerButton4.transform.position;
        brightArea5.transform.position = TowerButton5.transform.position;

        //brightArea.transform.localScale = new Vector3(0.02115656f / 4f * 3f, 0.02115656f / 4f * 3f, 0.009255994f);//step6 tra ve mac dinh
        //brightArea1.transform.localScale = new Vector3(0.02115656f / 4f * 3f, 0.02115656f / 4f * 3f, 0.009255994f);
        //brightArea2.transform.localScale = new Vector3(0.02115656f / 4f * 3f, 0.02115656f / 4f * 3f, 0.009255994f);
        //brightArea3.transform.localScale = new Vector3(0.02115656f / 4f * 3f, 0.02115656f / 4f * 3f, 0.009255994f);
        //brightArea4.transform.localScale = new Vector3(0.02115656f / 4f * 3f, 0.02115656f / 4f * 3f, 0.009255994f);
        //brightArea5.transform.localScale = new Vector3(0.02115656f / 4f * 3f, 0.02115656f / 4f * 3f, 0.009255994f);
        //brightArea6.transform.localScale = new Vector3(0.02115656f / 4f * 3f, 0.02115656f / 4f * 3f, 0.009255994f);
    }

    void TutorialStep4()
    {
        Debug.Log("Run 4 end step 3");

        if (tutorialText != null)
            ShowTutorialText("Click Place button!");

        brightArea.SetActive(false);
        brightArea1.SetActive(false);
        brightArea2.SetActive(false);
        brightArea3.SetActive(false);
        brightArea4.SetActive(false);
        brightArea5.SetActive(false);
        brightArea6.transform.position = PlaceButton.transform.position;


        highlightObject.transform.rotation = Quaternion.Euler(0, 0, 0f);
        Vector3 newPos = PlaceButton.transform.position;
        newPos.x -= 2f;
        highlightObject.transform.position = newPos;
    }

    void TutorialStep5()
    {
        UnlockAll();

        LockAllSpellButtons();

        Debug.Log("Run 5 end step 4");

        ShowTutorialText("You can place more towers by repeating the process!");

        brightArea6.SetActive(false);
        highlightObject.SetActive(false);
        StartCoroutine(WaitAndNextStep(3.6f));

    }

    void TutorialStep6()
    {
        Debug.Log("Run 6 end step 5");

        ShowTutorialText("Remember, you have limited build spot and money, so do it wisely!");
        //FakeBuildSpot.gameObject.SetActive(true);
        //FakeBuildSpot1.gameObject.SetActive(true);
        //FakeBuildSpot2.gameObject.SetActive(true);
        //FakeBuildSpot3.gameObject.SetActive(true);
        //FakeBuildSpot4.gameObject.SetActive(true);

        //LockAllExcept(startButton, brightArea1.GetComponent<Button>(), brightArea2.GetComponent<Button>(), brightArea3.GetComponent<Button>(), brightArea4.GetComponent<Button>(), brightArea5.GetComponent<Button>(), brightArea6.GetComponent<Button>(), brightArea.GetComponent<Button>());

        highlightObject.transform.rotation = Quaternion.Euler(0, 0, -90f);
        highlightObject.SetActive(true);
        brightArea.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f);
        brightArea.SetActive(true);

        if (startButton != null)
        {
            // vi tri den tren start
            Vector3 newPos = startButton.transform.position;
            newPos.y += 2f; // tăng cao thêm 2 pixel
            highlightObject.transform.position = newPos;
            brightArea.transform.position = startButton.transform.position;

        }
    }

    void TutorialStep7()
    {
        Debug.Log("Run 7 end step 6");
        brightArea.SetActive(false);
        highlightObject.SetActive(false);
        StartCoroutine(WaitAndNextStep(5f));
    }

    void TutorialStep8()
    {
        Debug.Log("Run 8 end step 7");
        highlightObject.SetActive(true);
        UnlockAllSpellButtons();

        ShowTutorialText("Spells are also very important, you can unlock them by completeing stages!");

        SpellButton.interactable = true;
        SpellButton1.interactable = true;
        SpellButton2.interactable = true;
        SpellButton3.interactable = true;
        SpellButton4.interactable = true;
        SpellButton5.interactable = true;
        SpellButton6.interactable = true;
        SpellButton7.interactable = true;


        brightArea.SetActive(true);
        brightArea1.SetActive(true);
        brightArea2.SetActive(true);
        brightArea3.SetActive(true);
        brightArea4.SetActive(true);
        brightArea5.SetActive(true);
        brightArea6.SetActive(true);
        brightArea7.SetActive(true);

        brightArea1.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f);
        brightArea2.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f);
        brightArea3.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f);
        brightArea4.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f);
        brightArea5.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f);
        brightArea6.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f); //bro im too lazy to write a function ts

        brightArea.transform.position = SpellButton.transform.position;
        brightArea1.transform.position = SpellButton1.transform.position;
        brightArea2.transform.position = SpellButton2.transform.position;
        brightArea3.transform.position = SpellButton3.transform.position;
        brightArea4.transform.position = SpellButton4.transform.position;
        brightArea5.transform.position = SpellButton5.transform.position;
        brightArea6.transform.position = SpellButton6.transform.position;
        brightArea7.transform.position = SpellButton7.transform.position;                       //coder sieu cap vip pro siu gon gang XD


        if (SpellButton3 != null)
        {
            // vi tri den tren start
            float midX = (SpellButton3.transform.position.x + SpellButton4.transform.position.x) / 2f; //giua spell4 va spell3
            Vector3 newPos = new Vector3(midX, SpellButton4.transform.position.y, SpellButton4.transform.position.z);
            newPos.y += 2f;
            highlightObject.transform.position = newPos;
     
        }
    }

    void TutorialStep9()
    {
        Debug.Log("Run 9 end step 8");

        ShowTutorialText("Each spell has special effect, click on enemies to use!");

        StartCoroutine(WaitAndNextStep(5f));
        //o day la 1 doan chu keu click vao enemy bat ki (spell)
        highlightObject.SetActive(false);

        brightArea.SetActive(false);
        brightArea1.SetActive(false);
        brightArea2.SetActive(false);
        brightArea3.SetActive(false);
        brightArea4.SetActive(false);
        brightArea5.SetActive(false);
        brightArea6.SetActive(false);
        brightArea7.SetActive(false);
    }

    void TutorialStep10()
    {
        Debug.Log("Run 10 end step 9");

        ShowTutorialText("You can upgrade or sell towers by right clicking on it!");

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

    }


    void OnDestroy()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartButtonClicked);

    }

    public void LockAllExcept(params Button[] allowedButtons)//vi du LockAllExcept(StartButton);,LockAllExcept(SpellButton1, SpellButton2, SpellButton3);

    {
        // Tìm tất cả Button trong scene, không cần sắp xếp
        Button[] allButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        lockedButtons.Clear();

        foreach (Button btn in allButtons)
        {
            if (System.Array.IndexOf(allowedButtons, btn) == -1)
                LockButton(btn);
        }
    }

    public void ShowTutorialText(string newText)
    {
        // Nếu đang gõ dở → dừng luôn và bắt đầu text mới
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        fullText = newText;
        tutorialText.text = "";     // Xoá text cũ trước khi gõ mới

        // Nếu text rỗng → ẩn luôn Text object cho gọn
        if (string.IsNullOrEmpty(newText))
        {
            tutorialText.gameObject.SetActive(false);
            return;
        }
        else
        {
            tutorialText.gameObject.SetActive(true);
        }

        typingCoroutine = StartCoroutine(TypeTextEffect(newText));
    }

    private IEnumerator TypeTextEffect(string textToType)
    {
        isTyping = true;
        tutorialText.text = "";

        foreach (char c in textToType)
        {
            tutorialText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
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

