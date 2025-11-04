using UnityEngine;
using UnityEngine.UI;

public class AudioSettingControl : MonoBehaviour
{
    [Header("🎵 Music Controls")]
    public Slider musicSlider;
    public Button musicToggleButton;
    public Image musicButtonImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Button musicPlusButton;
    public Button musicMinusButton;

    [Header("🔊 SFX Controls")]
    public Slider sfxSlider;
    public Button sfxToggleButton;
    public Image sfxButtonImage;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;
    public Button sfxPlusButton;
    public Button sfxMinusButton;

    private float lastMusicVolume = 1f;
    private float lastSfxVolume = 1f;

    void Start()
    {
        // Khởi tạo giá trị slider
        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.value = AudioManager.Instance.musicVolume;
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.value = AudioManager.Instance.sfxVolume;
            sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        }

        // Gắn event cho toggle buttons
        if (musicToggleButton != null)
            musicToggleButton.onClick.AddListener(ToggleMusic);

        if (sfxToggleButton != null)
            sfxToggleButton.onClick.AddListener(ToggleSfx);

        // Gắn event cho nút +/-
        if (musicPlusButton != null) musicPlusButton.onClick.AddListener(() => ChangeMusicVolume(0.1f));
        if (musicMinusButton != null) musicMinusButton.onClick.AddListener(() => ChangeMusicVolume(-0.1f));
        if (sfxPlusButton != null) sfxPlusButton.onClick.AddListener(() => ChangeSfxVolume(0.1f));
        if (sfxMinusButton != null) sfxMinusButton.onClick.AddListener(() => ChangeSfxVolume(-0.1f));

        UpdateButtonIcons();
    }

    // ===================== MUSIC =====================
    private void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        AudioManager.Instance.ToggleMusic(value > 0);
        UpdateButtonIcons();
    }

    private void ToggleMusic()
    {
        bool isCurrentlyOn = AudioManager.Instance.isMusicOn;

        if (isCurrentlyOn)
        {
            lastMusicVolume = musicSlider.value;
            musicSlider.value = 0;
            AudioManager.Instance.ToggleMusic(false);
        }
        else
        {
            musicSlider.value = lastMusicVolume > 0 ? lastMusicVolume : 0.5f;
            AudioManager.Instance.ToggleMusic(true);
        }

        UpdateButtonIcons();
    }

    private void ChangeMusicVolume(float delta)
    {
        float newValue = Mathf.Clamp(musicSlider.value + delta, 0, 1);
        musicSlider.value = newValue;
        OnMusicSliderChanged(newValue);
    }

    // ===================== SFX =====================
    private void OnSfxSliderChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
        AudioManager.Instance.ToggleSfx(value > 0);
        UpdateButtonIcons();
    }

    private void ToggleSfx()
    {
        bool isCurrentlyOn = AudioManager.Instance.isSfxOn;

        if (isCurrentlyOn)
        {
            lastSfxVolume = sfxSlider.value;
            sfxSlider.value = 0;
            AudioManager.Instance.ToggleSfx(false);
        }
        else
        {
            sfxSlider.value = lastSfxVolume > 0 ? lastSfxVolume : 0.5f;
            AudioManager.Instance.ToggleSfx(true);
        }

        UpdateButtonIcons();
    }

    private void ChangeSfxVolume(float delta)
    {
        float newValue = Mathf.Clamp(sfxSlider.value + delta, 0, 1);
        sfxSlider.value = newValue;
        OnSfxSliderChanged(newValue);
    }

    // ===================== ICON UPDATE =====================
    private void UpdateButtonIcons()
    {
        if (musicButtonImage != null)
            musicButtonImage.sprite = (musicSlider.value <= 0.001f) ? musicOffSprite : musicOnSprite;

        if (sfxButtonImage != null)
            sfxButtonImage.sprite = (sfxSlider.value <= 0.001f) ? sfxOffSprite : sfxOnSprite;
    }
}
