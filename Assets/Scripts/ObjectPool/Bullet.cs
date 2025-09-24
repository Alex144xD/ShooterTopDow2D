using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool useRigidbodyVelocity = true; 

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 2f;
    private float lifeTimer;

    private BulletPool pool;
    private Rigidbody2D rb;

    public void SetPoolReference(BulletPool bulletPool)
    {
        pool = bulletPool;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        lifeTimer = lifetime;

        if (useRigidbodyVelocity && rb != null)
        {
            rb.velocity = transform.up * speed; 
        }
    }

    private void Update()
    {
       
        if (!useRigidbodyVelocity || rb == null)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self);
        }

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            pool.ReturnBullet(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var ai = other.GetComponent<EnemyAI>();
            if (ai != null) ai.TakeDamage(25);

            pool.ReturnBullet(this);
        }
    }
}