using UnityEngine;

public class enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxHealth = 50f;
    private float currentHealth;

    public Transform[] waypoints; // gán trong Inspector
    private int waypointIndex = 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;

        Transform targetPoint = waypoints[waypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                Destroy(gameObject); // tới cuối đường thì biến mất
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP left: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
