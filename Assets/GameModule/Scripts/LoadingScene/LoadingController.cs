using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    [Header("References")]
    public LoadingBar loadingBar;
    public LoadingDots loadingDots;
    public ImageScaler logoScaler;

    [Header("Timing Settings")]
    [SerializeField] private float fakeFillDuration = 0.8f; // thời gian chạy 95% → 100%

    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return null;

        string nextScene = SceneLoader.targetScene;
        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.LogWarning("⚠️ Không có scene đích! Quay lại Menu.");
            SceneManager.LoadScene("MenuScene");
            yield break;
        }

        Debug.Log($"[Loading] Bắt đầu tải scene {nextScene}");

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float progress = 0f;

        while (!op.isDone)
        {
            if (op.progress < 0.9f)
            {
                float target = Mathf.Clamp01(op.progress / 0.9f) * 0.95f; // tối đa 95%
                progress = Mathf.MoveTowards(progress, target, Time.unscaledDeltaTime * 0.5f);
                loadingBar?.UpdateBar(progress);
            }
            else
            {
                Debug.Log("[Loading] Đã load xong, fake chạy 95% → 100%");
                float timer = 0f;
                float startValue = progress;

                while (timer < fakeFillDuration)
                {
                    timer += Time.unscaledDeltaTime;
                    progress = Mathf.Lerp(startValue, 1f, timer / fakeFillDuration);
                    loadingBar?.UpdateBar(progress);
                    yield return null;
                }

                loadingBar?.UpdateBar(1f);
                Debug.Log("[Loading] Hoàn tất 100%, chuyển cảnh!");
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
