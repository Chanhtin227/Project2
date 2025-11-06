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

    private const string UNLOCKED_LEVEL_KEY = "UnlockedLevel";
    private bool gameEnded = false;

    public int aliveCount = 0;

    private void Awake()
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

    private void Start()
    {
        gameEnded = false;
        aliveCount = 0;
    }

    // ----------------- ENEMY LOGIC -----------------
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
            HandleGameLose();
        }
    }

    // ----------------- GAME CONDITIONS -----------------
    public void CheckWinCondition()
    {
        if (gameEnded) return;

        Debug.Log($"[GameManager] Checking win condition... AliveCount = {aliveCount}");
        WaveManager[] waveManagers = FindObjectsOfType<WaveManager>();

        foreach (var wm in waveManagers)
        {
            if (wm.IsSpawning() || wm.IsWaitingForNext())
            {
                Debug.Log($"[GameManager] {wm.name} còn wave chưa hoàn thành, chưa thắng.");
                return;
            }
        }

        if (aliveCount <= 0)
        {
            Debug.Log("[GameManager] Tất cả wave hoàn thành và không còn quái!");
            HandleGameWin();
        }
    }

    private void HandleGameWin()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("Victory!");
        UnlockNextLevel();

        Time.timeScale = 0f;
        UIEvents.OnWin?.Invoke();
    }

    private void HandleGameLose()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        UIEvents.OnLose?.Invoke();
    }

   private void UnlockNextLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (!sceneName.StartsWith("Level")) return;

        // Lấy số Level hiện tại
        int currentLevelNumber = 1;
        int.TryParse(sceneName.Replace("Level", ""), out currentLevelNumber);

        int nextLevel = currentLevelNumber + 1;

        int unlocked = PlayerPrefs.GetInt(UNLOCKED_LEVEL_KEY, 1);
        if (nextLevel > unlocked)
        {
            PlayerPrefs.SetInt(UNLOCKED_LEVEL_KEY, nextLevel);
            PlayerPrefs.Save();
            Debug.Log($"Đã mở khóa Level {nextLevel}");
        }
    }

    // ----------------- SCENE CONTROLS -----------------
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
