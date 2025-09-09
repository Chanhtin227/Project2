using UnityEngine;
using UnityEngine.SceneManagement; // cần cho SceneManager

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string GameplayScene)
    {
        SceneManager.LoadScene(GameplayScene);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
