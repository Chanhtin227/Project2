using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && Keyboard.current.rKey.isPressed)
        {
            PlayerPrefs.DeleteKey("UnlockedLevel");
            PlayerPrefs.Save();
            Debug.Log("🧹 Dữ liệu mở khóa đã được reset (Level 1).");
        }
    }

    // ------------------- PAUSE -------------------
    public void TogglePause()
    {
        // Nếu panel chưa gán, cảnh báo tránh lỗi null
        if (pausePanel == null)
        {
            Debug.LogError("⚠️ PausePanel chưa được gán trong Inspector!");
            return;
        }

        // Đảo trạng thái pause
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        Debug.Log(isPaused ? "⏸ Game Paused" : "▶ Game Resumed");
    }

    public void ContinueGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene"); // ⚠️ nhớ đổi theo tên scene menu của bạn
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ------------------- WIN / LOSE -------------------
    public void OnNextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            ReturnToMenu();
    }

    public void OnRetry()
    {
        GameManager.Instance.RestartLevel();
    }

    public void OnBackToMenu()
    {
        ReturnToMenu();
    }
}
