using System.Collections;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [Header("Visual")]
    public LineRenderer line;
    public float duration = 0.1f;
    public int segments = 6;
    public float jaggedness = 0.2f;

    [Header("Damage")]
    public float damage = 10f;
    public LayerMask enemyLayer;

    private Coroutine running;

    public void Initialize(Transform start, Transform target, float dmg, int chain, float radius, LayerMask enemyMask)
    {
        damage = dmg;
        enemyLayer = enemyMask;

        if (running != null) StopCoroutine(running);
        running = StartCoroutine(DoLightning(start.position, target.position, target));
    }

    private IEnumerator DoLightning(Vector3 start, Vector3 end, Transform target)
    {
        DrawZigZag(start, end);
        line.enabled = true;

        if (target != null)
        {
            enemy en = target.GetComponent<enemy>();
            if (en != null) en.TakeDamage((int)damage);
        }

        yield return new WaitForSeconds(duration);

        line.enabled = false;
        running = null;

        PoolManager.Instance.Return(gameObject, "Lightning");
    }

    private void DrawZigZag(Vector3 start, Vector3 end)
    {
        line.positionCount = segments + 1;
        Vector3 dir = (end - start) / segments;

        for (int i = 0; i <= segments; i++)
        {
            Vector3 pos = start + dir * i;
            if (i > 0 && i < segments)
                pos += (Vector3)Random.insideUnitCircle * jaggedness;
            line.SetPosition(i, pos);
        }
    }

    private void OnDisable()
    {
        if (running != null)
        {
            StopCoroutine(running);
            running = null;
        }
        if (line != null) line.enabled = false;
    }
}
