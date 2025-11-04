using UnityEngine;
using TMPro;

public class GoldPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private float moveUpSpeed = 50f;
    [SerializeField] private float lifetime = 1f;

    private float timer = 0f;

    public void Setup(int gold)
    {
        goldText.text = "+" + gold.ToString();
    }

    void Update()
    {
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
