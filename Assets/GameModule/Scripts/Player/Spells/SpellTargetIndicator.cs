using UnityEngine;

public class SpellTargetIndicator : MonoBehaviour
{
    private SpellData data;

    public Transform singleCursor;
    private Enemy highlightedEnemy;

    public void Setup(SpellData spell)
    {
        data = spell;
        UpdateScale();
        if (data.singleTarget)
        {
            if (singleCursor != null) singleCursor.gameObject.SetActive(true);
        }
        else
        {
            if (singleCursor != null) singleCursor.gameObject.SetActive(false);
        }
    }

    private void UpdateScale()
    {
        if (data == null) return;
        if (data.singleTarget)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            // AOE: scale theo bán kính
            float diameter = data.radius * 2f;
            transform.localScale = new Vector3(diameter, diameter, 1f);
        }
    }

    private void Update()
    {
        if (Camera.main == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
        mousePos.z = 0f;
        transform.position = mousePos;

        if (data == null) return;

        if (data.singleTarget)
        {
            HighlightEnemyUnderCursor(mousePos);
            if (singleCursor != null)
            {
                singleCursor.position = mousePos;
            }
        }
        else
        {
            ClearHighlightedEnemy();
        }
    }

    private void HighlightEnemyUnderCursor(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Enemy"));
        if (hit != null)
        {
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null && e != highlightedEnemy)
            {
                ClearHighlightedEnemy();
                highlightedEnemy = e;
                e.GetComponent<OutlineController>()?.ShowOutline(true);
            }
        }
        else
        {
            ClearHighlightedEnemy();
        }
    }

    private void ClearHighlightedEnemy()
    {
        if (highlightedEnemy != null)
        {
            highlightedEnemy.GetComponent<OutlineController>()?.ShowOutline(false);
            highlightedEnemy = null;
        }
    }

    private void OnDisable()
    {
        ClearHighlightedEnemy();
    }

    private void OnDestroy()
    {
        ClearHighlightedEnemy();
    }
}
