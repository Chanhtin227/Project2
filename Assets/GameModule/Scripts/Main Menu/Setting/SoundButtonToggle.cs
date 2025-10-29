using UnityEngine;
using UnityEngine.UI;

public class SoundButtonToggle : MonoBehaviour
{
    public Sprite buttonSound;      
    public Sprite buttonSoundOff;   

    private Image buttonImage;
    private bool isSoundOn = true; 

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = buttonSound; // ban đầu là bật
    }

    public void ToggleSoundButton()
    {
        if (isSoundOn)
        {
            buttonImage.sprite = buttonSoundOff; // đổi sang nút off
            isSoundOn = false;
            if (isSoundOn)
            {
                buttonImage.sprite = buttonSoundOff;
                isSoundOn = false;
                AudioManager.Instance.ToggleSfx(false);
            }
            else
            {
                buttonImage.sprite = buttonSound;
                isSoundOn = true;
                AudioManager.Instance.ToggleSfx(true);
            }
        }
        else
        {
            buttonImage.sprite = buttonSound; // đổi sang nút on
            isSoundOn = true;
            // ở đây bạn có thể thêm code bật nhạc
        }
    }
}
