using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    private bool isPaused = false;

    private void OnEnable()
    {
        UIEvents.OnWin += ShowWinPanel;
        UIEvents.OnLose += ShowLosePanel;
    }

    private void OnDisable()
    {
        UIEvents.OnWin -= ShowWinPanel;
        UIEvents.OnLose -= ShowLosePanel;
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            PlayerPrefs.DeleteKey("UnlockedLevel");
            PlayerPrefs.Save();
            Debug.Log("Dữ liệu mở khóa đã được reset (Level 1).");
        }
    }

    // ---------------- PAUSE ----------------
    public void TogglePause()
    {
        if (pausePanel == null) return;

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }

    public void ContinueGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    // ---------------- WIN / LOSE ----------------
    private void ShowWinPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Debug.Log("[UI] Hiển thị Win Panel");
        }
    }

    private void ShowLosePanel()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Debug.Log("[UI] Hiển thị Lose Panel");
        }
    }

    // ---------------- BUTTONS ----------------
    public void OnNextLevel()
    {
        Time.timeScale = 1f;

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            ReturnToMenu();
            return;
        }

        //Lấy tên scene kế tiếp
        string nextScene = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextIndex));

        //Gọi SceneLoader thay vì LoadScene trực tiếp
        SceneLoader.LoadScene(nextScene);
    }


    public void OnRetry() => GameManager.Instance.RestartLevel();

    public void OnBackToMenu() => ReturnToMenu();

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
