using UnityEngine;

public class AutoDestroyAfterAnim : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"{name} không có Animator, AutoDestroyAfterAnim sẽ dùng duration mặc định");
            Destroy(gameObject, 1f);
            return;
        }

        // Lấy độ dài clip đầu tiên trong Animator
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            float animLength = clipInfo[0].clip.length;
            Destroy(gameObject, animLength);
            
        }
        else
        {
            Debug.LogWarning($"{name} không tìm thấy clip trong Animator!");
            Destroy(gameObject, 1f); // fallback
        }
    }
}
