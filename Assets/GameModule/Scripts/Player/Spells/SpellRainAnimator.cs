using UnityEngine;
using System.Collections;

public class SpellRainAnimator : MonoBehaviour
{
    private Animator animator;
    private float loopTime;
    private string triggerName;

    public void Init(Animator anim, float time, string trigger)
    {
        animator = anim;
        loopTime = time;
        triggerName = trigger;
        StartCoroutine(PlayFade());
    }

    private IEnumerator PlayFade()
    {
        yield return new WaitForSeconds(loopTime);
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
