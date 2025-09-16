using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [Header("Visual")]
    public LineRenderer line;
    public float duration = 0.1f;
    public int segments = 6;
    public float jaggedness = 0.2f;

    [Header("Damage")]
    public LayerMask enemyLayer;

    private Coroutine running;

    public void Initialize(Transform start, Transform target, float damage, int chains, float radius, LayerMask enemyMask, HashSet<Transform> alreadyHit = null)
    {
        enemyLayer = enemyMask;

        if (running != null) StopCoroutine(running);
        running = StartCoroutine(DoLightning(start.position, target.position, target, damage, chains, radius, alreadyHit));
    }

    private IEnumerator DoLightning(Vector3 start, Vector3 end, Transform target, float damage, int chains, float radius, HashSet<Transform> alreadyHit)
    {
        DrawZigZag(start, end);
        line.enabled = true;

        if (alreadyHit == null) alreadyHit = new HashSet<Transform>();
        if (target != null && !alreadyHit.Contains(target))
        {
            enemy en = target.GetComponent<enemy>();
            if (en != null) en.TakeDamage((int)damage);
            alreadyHit.Add(target);
        }

        yield return new WaitForSeconds(duration);
        line.enabled = false;
        running = null;

        // Chain to next targets
        if (chains > 0 && target != null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, radius, enemyLayer);
            int chainsLeft = chains;
            foreach (var hit in hits)
            {
                if (chainsLeft <= 0) break;
                if (hit.transform == target || alreadyHit.Contains(hit.transform)) continue;

                // Spawn new lightning for each chain
                LightningProjectile zap = PoolManager.Instance.Get<LightningProjectile>("Lightning");
                if (zap != null)
                {
                    zap.Initialize(target, hit.transform, damage, chains - 1, radius, enemyLayer, alreadyHit);
                    chainsLeft--;
                }
            }
        }

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
