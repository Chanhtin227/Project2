using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellButton : MonoBehaviour
{
    public SpellData spellData;
    public Button button;
    public Image iconImage;
    public Image cooldownOverlay;
    public GameObject lockIcon;
    public TextMeshProUGUI lockText;

    private float cooldownTimer = 0f;
    public bool isUnlocked = false;

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(OnClick);

        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 0f;
            cooldownOverlay.gameObject.SetActive(false);
        }

        // Kiểm tra trạng thái unlock
        CheckUnlockStatus();
    }

    private void Update()
    {
        // Kiểm tra unlock status mỗi frame
        if (SpellManager.Instance != null)
        {
            bool nowUnlocked = SpellManager.Instance.IsSpellUnlocked(spellData.type);
            if (nowUnlocked != isUnlocked)
            {
                isUnlocked = nowUnlocked;
                UpdateLockVisual();
            }
        }

        // Nếu spell bị khóa thì không xử lý cooldown
        if (!isUnlocked) return;

        // Cooldown riêng
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            float ratio = Mathf.Clamp01(cooldownTimer / spellData.cooldown);
            cooldownOverlay.fillAmount = ratio;
            cooldownOverlay.gameObject.SetActive(true);
            button.interactable = false;
            SetIconAlpha(0.5f);
        }
        else
        {
            // Cooldown chung
            if (SpellManager.Instance != null && SpellManager.Instance.globalCooldown > 0)
            {
                if (SpellManager.Instance.globalCooldownTimer > 0)
                {
                    button.interactable = false;
                    SetIconAlpha(0.5f);
                    return;
                }
            }

            cooldownOverlay.fillAmount = 0f;
            cooldownOverlay.gameObject.SetActive(false);
            button.interactable = true;
            SetIconAlpha(1f);
        }
    }

    private void CheckUnlockStatus()
    {
        if (SpellManager.Instance != null)
        {
            isUnlocked = SpellManager.Instance.IsSpellUnlocked(spellData.type);
            UpdateLockVisual();
        }
    }

    private void UpdateLockVisual()
    {
        if (isUnlocked)
        {
            // Mở khóa
            button.interactable = true;
            SetIconAlpha(1f);
            if (lockIcon != null) lockIcon.SetActive(false);
            if (lockText != null) lockText.gameObject.SetActive(false);
        }
        else
        {
            // Bị khóa
            button.interactable = false;
            SetIconAlpha(0.3f);
            if (lockIcon != null) lockIcon.SetActive(true);
            if (lockText != null) lockText.gameObject.SetActive(true);
        }
    }

    private void OnClick()
    {
        if (SpellManager.Instance != null)
            SpellManager.Instance.CastSpell(spellData, this);
    }

    public void StartCooldown()
    {
        cooldownTimer = spellData.cooldown;
    }

    private void SetIconAlpha(float alpha)
    {
        if (iconImage != null)
        {
            Color c = iconImage.color;
            c.a = alpha;
            iconImage.color = c;
        }
    }
}