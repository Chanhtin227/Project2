using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private float movespeed = 0.5f;
    private Rigidbody2D rb;
    private Transform checkpoint;
    private int index = 0;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        checkpoint = CheckpointsManager.main.checkpoints[index];
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, checkpoint.position) <= 0.2f)
        {
            index++;
            if (index >= CheckpointsManager.main.checkpoints.Length)
            {
                Destroy(gameObject);
                return;
            }
            checkpoint = CheckpointsManager.main.checkpoints[index];
        }
    }

    void FixedUpdate()
    {
        Vector2 direction = (checkpoint.position - transform.position).normalized;
        rb.linearVelocity = direction * movespeed;
        if(rb.linearVelocity.x > 0.1)
        {
            spriteRenderer.flipX = false;
        }
        else if(rb.linearVelocity.x < -0.1)
        {
            spriteRenderer.flipX = true;
        }
    }
}
