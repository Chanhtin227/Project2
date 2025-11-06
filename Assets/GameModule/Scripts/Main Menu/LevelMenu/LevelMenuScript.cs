using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    public int levelIndex = 1;
    private Button button;

    private const string UNLOCKED_LEVEL_KEY = "UnlockedLevel";

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt(UNLOCKED_LEVEL_KEY, 1);
        button.interactable = levelIndex <= unlockedLevel;
    }

    void OnClick()
    {
        string sceneName = "Level" + levelIndex;

        if (IsSceneInBuild(sceneName))
            SceneLoader.LoadScene(sceneName);
        else
            Debug.LogError($"❌ Scene {sceneName} chưa có trong Build Settings!");
    }

    private bool IsSceneInBuild(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return true;
        }
        return false;
    }
}
