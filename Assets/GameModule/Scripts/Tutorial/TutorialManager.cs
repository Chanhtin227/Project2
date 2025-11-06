using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Overlay")]
    public GameObject darkOverlay;
    public Text tutorialText;
    public GameObject highlightObject;
    public Canvas canvas;

    [Header("Text typing effect")]
    public float typingSpeed = 0.03f;
    private Coroutine typingCoroutine;
    private string fullText = "";
    private bool isTyping = false;

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
    public Button settingButton;

    private HashSet<Button> lockedButtons = new HashSet<Button>();
    private int currentStep = 0;
    private List<System.Action> tutorialSteps = new List<System.Action>();
    
    private bool tutorialEnded = false;

    void Start()
    {
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
        tutorialSteps.Add(TutorialStep11);
        tutorialSteps.Add(TutorialStep12);

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

        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonClicked);

        if (settingButton != null)
            settingButton.onClick.AddListener(OnSettingClicked);

        StartTutorial();
    }

    void Update()
    {
        if (!tutorialEnded)
        {
            foreach (Button btn in lockedButtons)
            {
                if (btn != null)
                    btn.interactable = false;
            }
        }
    }

    public void LockButton(Button btn)
    {
        if (btn != null)
        {
            lockedButtons.Add(btn);
            btn.interactable = false;
        }
    }

    public void UnlockButton(Button btn)
    {
        if (btn != null)
        {
            lockedButtons.Remove(btn);
        }
    }

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
                //Kiểm tra xem spell có component SpellButton không
                SpellButton spellScript = btn.GetComponent<SpellButton>();
                
                //Chỉ unlock nếu spell đã được mở (isUnlocked = true)
                if (spellScript != null && spellScript.isUnlocked)
                {
                    btn.interactable = true;

                    CanvasGroup cg = btn.GetComponent<CanvasGroup>();
                    if (cg != null)
                    {
                        cg.blocksRaycasts = true;
                        cg.interactable = true;
                    }
                }
                else
                {
                    //Spell chưa unlock thì vẫn giữ khóa
                    btn.interactable = false;
                    
                    CanvasGroup cg = btn.GetComponent<CanvasGroup>();
                    if (cg != null)
                    {
                        cg.blocksRaycasts = false;
                        cg.interactable = false;
                    }
                }
            }
        }
    }

    void StartTutorial()
    {
        currentStep = 0;
        tutorialEnded = false;
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

    IEnumerator WaitAndNextStep(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NextStep();
    }

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
            highlightObject.transform.rotation = Quaternion.Euler(0, 0, -90f);
            Vector3 newPos = startButton.transform.position;
            newPos.y += 2f;
            highlightObject.transform.position = newPos;
            brightArea.transform.position = startButton.transform.position;
        }
        LockAllExcept(startButton, brightArea1.GetComponent<Button>(), brightArea2.GetComponent<Button>(), 
                      brightArea3.GetComponent<Button>(), brightArea4.GetComponent<Button>(), 
                      brightArea5.GetComponent<Button>(), brightArea6.GetComponent<Button>(), 
                      brightArea.GetComponent<Button>());
    }

    void TutorialStep2()
    {
        darkOverlay.SetActive(false);

        if (tutorialText != null)
            ShowTutorialText("Select a build spot to build your tower!");

        UnlockAll();
        LockAllExcept(startButton, FakeBuildSpot, FakeBuildSpot1, FakeBuildSpot2, FakeBuildSpot3, 
                      FakeBuildSpot4, brightArea1.GetComponent<Button>(), brightArea2.GetComponent<Button>(), 
                      brightArea3.GetComponent<Button>(), brightArea4.GetComponent<Button>(), 
                      brightArea5.GetComponent<Button>(), brightArea6.GetComponent<Button>(), 
                      brightArea.GetComponent<Button>());
        
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
        LockAllExcept(startButton, FakeBuildSpot, FakeBuildSpot1, FakeBuildSpot2, FakeBuildSpot3, 
                      FakeBuildSpot4, brightArea1.GetComponent<Button>(), brightArea2.GetComponent<Button>(), 
                      brightArea3.GetComponent<Button>(), brightArea4.GetComponent<Button>(), 
                      brightArea5.GetComponent<Button>(), brightArea6.GetComponent<Button>(), 
                      brightArea.GetComponent<Button>());

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
        StartCoroutine(WaitAndNextStep(5f));

    }
    void TutorialStep7()
    {
        ShowTutorialText("Now, click the start button to begin new wave!");
        highlightObject.transform.rotation = Quaternion.Euler(0, 0, -90f);
        highlightObject.SetActive(true);
        brightArea.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f);
        brightArea.SetActive(true);

        if (startButton != null)
        {
            Vector3 newPos = startButton.transform.position;
            newPos.y += 2f;
            highlightObject.transform.position = newPos;
            brightArea.transform.position = startButton.transform.position;
        }
    }

    void TutorialStep8()
    {
        Debug.Log("Run 7 end step 6");
        brightArea.SetActive(false);
        highlightObject.SetActive(false);
        StartCoroutine(WaitAndNextStep(5f));
    }

    void TutorialStep9()
    {
        Debug.Log("Run 8 end step 7");
        highlightObject.SetActive(true);
        
        UnlockAllSpellButtons();

        ShowTutorialText("Spells are also very important, you can unlock them by completing stages!");

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
        brightArea6.transform.localScale = new Vector3(0.02115656f, 0.02115656f, 0.009255994f);

        brightArea.transform.position = SpellButton.transform.position;
        brightArea1.transform.position = SpellButton1.transform.position;
        brightArea2.transform.position = SpellButton2.transform.position;
        brightArea3.transform.position = SpellButton3.transform.position;
        brightArea4.transform.position = SpellButton4.transform.position;
        brightArea5.transform.position = SpellButton5.transform.position;
        brightArea6.transform.position = SpellButton6.transform.position;
        brightArea7.transform.position = SpellButton7.transform.position;

        if (SpellButton3 != null)
        {
            float midX = (SpellButton3.transform.position.x + SpellButton4.transform.position.x) / 2f;
            Vector3 newPos = new Vector3(midX, SpellButton4.transform.position.y, SpellButton4.transform.position.z);
            newPos.y += 2f;
            highlightObject.transform.position = newPos;
        }
    }

    void TutorialStep10()
    {
        Debug.Log("Run 9 end step 8");

        ShowTutorialText("Each spell has special effect, click on enemies to use!");

        StartCoroutine(WaitAndNextStep(5f));
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

    void TutorialStep11()
    {
        Debug.Log("Run 10 end step 9");
        ShowTutorialText("You can upgrade or sell towers by right clicking on it!");
        StartCoroutine(WaitAndNextStep(5f));
    }
    
    void TutorialStep12()
    {
        Debug.Log("Run 11 end step 10");
        ShowTutorialText("This is the end of the tutorial, good luck and have fun my dear Commander!");
        StartCoroutine(WaitAndEndTutorial(5f));
    }

    IEnumerator WaitAndEndTutorial(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EndTutorial();
    }

    void OnStartButtonClicked()
    {
        Debug.Log("Start Button clicked!");
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
        Debug.Log("✅ Tutorial ended - Unlocking all!");
        
        tutorialEnded = true;
        
        darkOverlay.SetActive(false);
        highlightObject.SetActive(false);
        
        brightArea.SetActive(false);
        brightArea1.SetActive(false);
        brightArea2.SetActive(false);
        brightArea3.SetActive(false);
        brightArea4.SetActive(false);
        brightArea5.SetActive(false);
        brightArea6.SetActive(false);
        if (brightArea7 != null) brightArea7.SetActive(false);
        
        UnlockAll();
        UnlockAllSpellButtons();
        
        Time.timeScale = 1f;

        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(false);
        }
        
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            //Nếu wave chưa từng spawn, tự động bắt đầu wave đầu tiên
            if (waveManager.IsWaitingForNext())
            {
                Debug.Log("[TutorialManager] Tutorial ended — starting first wave automatically!");
                waveManager.StartNextWave();
            }
        }

        //Kiểm tra điều kiện thắng (nếu hết wave hoặc hết enemy)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckWinCondition();
        }

        
        Debug.Log("✅ Tutorial cleanup complete!");
    }

    void OnDestroy()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartButtonClicked);
    }

    public void LockAllExcept(params Button[] allowedButtons)
    {
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
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        fullText = newText;
        tutorialText.text = "";

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