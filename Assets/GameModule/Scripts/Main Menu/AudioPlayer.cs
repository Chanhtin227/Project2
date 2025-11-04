using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
    }

}
