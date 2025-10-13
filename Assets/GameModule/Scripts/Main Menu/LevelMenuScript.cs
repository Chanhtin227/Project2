using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndex; // ví dụ: 1, 2, 3, ...

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Debug.Log("Load level " + levelIndex);
        SceneManager.LoadScene("Level" + levelIndex);
    }
}
//dat ten cac scene theo Level[number] bo []