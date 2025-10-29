using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public bool isMusicOn = true;
    public bool isSfxOn = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateVolumes();
    }

    // Phát nhạc nền
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (!isMusicOn || clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    // Dừng nhạc
    public void StopMusic() => musicSource.Stop();

    // Phát hiệu ứng âm thanh
    public void PlaySfx(AudioClip clip)
    {
        if (!isSfxOn || clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // Cập nhật âm lượng khi slider thay đổi
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = musicVolume;
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = sfxVolume;
    }

    // Tắt/bật nhạc hoặc hiệu ứng
    public void ToggleMusic(bool state)
    {
        isMusicOn = state;
        if (!state) musicSource.Pause();
        else musicSource.UnPause();
    }

    public void ToggleSfx(bool state)
    {
        isSfxOn = state;
    }

    private void UpdateVolumes()
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }
}

/*void Start()
{
    AudioManager.Instance.PlayMusic(AudioManager.Instance.gameplayMusic); dùng phát sound cho gp scene
}
void Start()
{
    AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic); dùng phát sound cho menu scene
}*/

