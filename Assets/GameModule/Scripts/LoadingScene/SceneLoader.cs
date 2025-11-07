using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string targetScene; // nơi cần load sau khi loading xong

    public static void LoadScene(string sceneName)
    {
        targetScene = sceneName;
        SceneManager.LoadScene("LoadingScene"); // luôn qua màn loading
    }
}
