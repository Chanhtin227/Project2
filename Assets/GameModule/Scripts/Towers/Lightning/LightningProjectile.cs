using System.Collections;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    public LineRenderer linePrefab;   // Prefab chứa LineRenderer + material
    public float damage = 10f;
    public float duration = 0.1f;     // thời gian tồn tại tia sét
    public int segments = 6;          // số đoạn zigzag
    public float jaggedness = 0.2f;   // độ lệch zigzag
    public LayerMask enemyLayer;

    public void Initialize(Transform start, Transform target, float dmg, int chain, float radius, LayerMask enemyMask)
    {
        damage = dmg;
        enemyLayer = enemyMask;
        StartCoroutine(DoLightning(start.position, target.position));
        
        // gây damage cho enemy
        enemy enemy = target.GetComponent<enemy>();
        if (enemy != null)
            enemy.TakeDamage((int)damage);
    }

    private IEnumerator DoLightning(Vector3 start, Vector3 end)
    {
        LineRenderer line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

        DrawZigZag(line, start, end);

        yield return new WaitForSeconds(duration);

        Destroy(line.gameObject);
    }

    private void DrawZigZag(LineRenderer line, Vector3 start, Vector3 end)
    {
        line.positionCount = segments + 1;
        Vector3 dir = (end - start) / segments;

        for (int i = 0; i <= segments; i++)
        {
            Vector3 pos = start + dir * i;

            if (i > 0 && i < segments) // chỉ zigzag ở giữa
            {
                pos += (Vector3)Random.insideUnitCircle * jaggedness;
            }

            line.SetPosition(i, pos);
        }
    }
}
