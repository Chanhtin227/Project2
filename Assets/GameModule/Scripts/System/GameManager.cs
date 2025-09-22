using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    public PoolManager poolManager;
    public GoldManager goldManager;
    public AudioManager audioManager;
    // thêm manager khác sau này nếu cần

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (poolManager == null) poolManager = GetComponentInChildren<PoolManager>();
            if (goldManager == null) goldManager = GetComponentInChildren<GoldManager>();
            if (audioManager == null) audioManager = GetComponentInChildren<AudioManager>();
        }
        else
        {
            Destroy(gameObject); // tránh duplicate khi load scene mới
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
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
