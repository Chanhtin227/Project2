using UnityEngine;
using UnityEngine.UI;

public class MusicButtonToggle : MonoBehaviour
{
    public Sprite buttonMusic;      // sprite bật nhạc
    public Sprite buttonMusicOff;   // sprite tắt nhạc

    private Image buttonImage;
    private bool isMusicOn = true; // trạng thái nhạc

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = buttonMusic; // ban đầu là bật
    }

    public void ToggleMusicButton()
    {
        if (isMusicOn)
        {
            buttonImage.sprite = buttonMusicOff; // đổi sang nút off
            isMusicOn = false;
            // ở đây bạn có thể thêm code tắt nhạc
        }
        else
        {
            buttonImage.sprite = buttonMusic; // đổi sang nút on
            isMusicOn = true;
            // ở đây bạn có thể thêm code bật nhạc
        }
    }
}
