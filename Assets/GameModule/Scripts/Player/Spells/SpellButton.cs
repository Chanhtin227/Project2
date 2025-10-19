using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public SpellData spellData;
    public Button button;
    public Image iconImage;        // icon chính
    public Image cooldownOverlay;  // ảnh overlay fill radial

    private float cooldownTimer = 0f;

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(OnClick);

        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 0f;
            cooldownOverlay.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            float ratio = Mathf.Clamp01(cooldownTimer / spellData.cooldown);

            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = ratio;
                cooldownOverlay.gameObject.SetActive(true);
            }

            button.interactable = false;
            SetIconAlpha(0.5f);
        }
        else
        {
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = 0f;
                cooldownOverlay.gameObject.SetActive(false);
            }

            button.interactable = true;
            SetIconAlpha(1f);
        }
    }

    private void OnClick()
    {
        if (SpellManager.Instance != null)
        {
            SpellManager.Instance.CastSpell(spellData, this);
        }
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
