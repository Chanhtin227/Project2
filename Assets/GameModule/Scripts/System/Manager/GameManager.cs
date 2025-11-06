using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    public PoolManager poolManager;
    public GoldManager goldManager;
    public AudioManager audioManager;
    public TowerRangeManager towerRangeManager;

    [Header("Player Base Health")]
    public int baseHealth = 1000;
    public int maxBaseHealth = 1000;

    [Header("Win/Lose Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private const string UNLOCKED_LEVEL_KEY = "UnlockedLevel";

    private bool gameEnded = false;

    public int aliveCount = 0;

    public void RegisterEnemy()
    {
        aliveCount++;
        Debug.Log($"[GameManager] Enemy spawned. AliveCount = {aliveCount}");
    }

    public void UnregisterEnemy()
    {
        aliveCount = Mathf.Max(0, aliveCount - 1);
        Debug.Log($"[GameManager] Enemy died or reached base. AliveCount = {aliveCount}");
        CheckWinCondition();
    }

    public void LoseBaseHealth(int amount)
    {
        if (gameEnded) return;

        baseHealth -= amount;
        baseHealth = Mathf.Max(baseHealth, 0);
        Debug.Log($"💔 Căn cứ bị tấn công! Mất {amount} máu. Còn lại: {baseHealth}");

        if (baseHealth <= 0)
        {
            baseHealth = 0;
            ShowLosePanel();
        }
    }

    public void CheckWinCondition()
    {
        if (gameEnded) return;

        Debug.Log($"[GameManager] Checking win condition... AliveCount = {aliveCount}");

        // Kiểm tra wave còn spawn không
        WaveManager[] waveManagers = FindObjectsOfType<WaveManager>();
        foreach (var wm in waveManagers)
        {
            if (wm.IsSpawning())
            {
                Debug.Log($"[GameManager] {wm.name} đang spawn, chưa thắng.");
                return;
            }
        }

        // Nếu không còn quái và đã hết wave => thắng
        if (aliveCount <= 0)
        {
            ShowWinPanel();
        }
    }

    private void ShowWinPanel()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("🎉 Victory!");

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        int unlocked = PlayerPrefs.GetInt(UNLOCKED_LEVEL_KEY, 1);
        if (nextIndex > unlocked)
        {
            PlayerPrefs.SetInt(UNLOCKED_LEVEL_KEY, nextIndex);
            PlayerPrefs.Save();
            Debug.Log($"🔓 Đã mở khóa Level {nextIndex}");
        }

        if (winPanel != null)
            winPanel.SetActive(true);
        else
            Debug.LogError("Win Panel chưa được gán!");

        Time.timeScale = 0f;
    }

    private void ShowLosePanel()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("💀 Game Over!");

        if (losePanel != null)
            losePanel.SetActive(true);
        else
            Debug.LogError("Lose Panel chưa được gán!");

        Time.timeScale = 0f;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (poolManager == null) poolManager = GetComponentInChildren<PoolManager>();
            if (goldManager == null) goldManager = GetComponentInChildren<GoldManager>();
            if (audioManager == null) audioManager = GetComponentInChildren<AudioManager>();
            if (towerRangeManager == null) towerRangeManager = GetComponentInChildren<TowerRangeManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameEnded = false;
        aliveCount = 0;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        gameEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
