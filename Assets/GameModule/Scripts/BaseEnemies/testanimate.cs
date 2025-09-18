using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Transform checkpoint;
    private int index = 0;

    private Transform[] myPath;  // đường đi riêng của con này

    [SerializeField] private int pathId = 0; // chọn đường (0 = A, 1 = B, 2 = C)

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Đổi sang MultiPathCheckpointsManager
        myPath = MultiPathCheckpointsManager.main.GetPath(pathId);

        if (myPath != null && myPath.Length > 0)
        {
            checkpoint = myPath[index];
        }
    }

    void Update()
    {
        if (checkpoint == null) return;

        if (Vector2.Distance(transform.position, checkpoint.position) <= 0.2f)
        {
            index++;
            if (index >= myPath.Length)
            {
                Destroy(gameObject);
                return;
            }
            checkpoint = myPath[index];
        }
    }

    void FixedUpdate()
    {
        if (checkpoint == null) return;

        Vector2 direction = (checkpoint.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (rb.linearVelocity.x > 0.1f) spriteRenderer.flipX = false;
        else if (rb.linearVelocity.x < -0.1f) spriteRenderer.flipX = true;
    }
}
