using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Dead }

    [Header("Refs")]
    [SerializeField] private EnemyContactDamage contactDamage;
    private Rigidbody2D rb;
    private Transform player;
    private Camera cam;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float patrolChangeDirTime = 2f;
    [SerializeField] private float stopRadius = 0.6f;

    [Header("Ranges")]
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float attackRange = 1.2f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int hp;
    private bool isDead;

    [Header("Death Handling")]
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelay = 0.05f; 

    
    [SerializeField] private State state = State.Patrol;
    private float dirTimer;
    private Vector2 patrolDir = Vector2.right;

 
    private Vector2 minBounds, maxBounds;

    private float sqrDetection => detectionRange * detectionRange;
    private float sqrAttack => attackRange * attackRange;
    private float sqrStop => stopRadius * stopRadius;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        cam = Camera.main;
        if (!contactDamage)
            contactDamage = GetComponentInChildren<EnemyContactDamage>(true);
    }

    void OnEnable()
    {
       
        hp = maxHealth;
        isDead = false;
        SetContactDamage(false);
        EnableAllColliders(true);
        rb.velocity = Vector2.zero;
        SetPatrolDir(Random.insideUnitCircle.normalized);
        dirTimer = 0f;
        UpdateCameraBounds();
        state = State.Patrol;
    }

    void Start()
    {
        state = State.Patrol;
    }

    void Update()
    {
        if (isDead) return;

        UpdateCameraBounds();

        if (player == null)
        {
            if (state != State.Patrol) ChangeState(State.Patrol);
            return;
        }

        Vector2 toPlayer = (Vector2)(player.position - transform.position);
        float sqrDist = toPlayer.sqrMagnitude;

        switch (state)
        {
            case State.Patrol:
                if (sqrDist <= sqrDetection) ChangeState(State.Chase);

                dirTimer += Time.deltaTime;
                if (dirTimer >= patrolChangeDirTime)
                {
                    SetPatrolDir(Random.insideUnitCircle.normalized);
                    dirTimer = 0f;
                }
                break;

            case State.Chase:
                if (sqrDist <= sqrAttack) ChangeState(State.Attack);
                else if (sqrDist > sqrDetection * 1.1f) ChangeState(State.Patrol);
                else if (!InBounds((Vector2)rb.position + toPlayer.normalized * moveSpeed * Time.deltaTime))
                    ChangeState(State.Patrol);
                break;

            case State.Attack:
                if (sqrDist > sqrAttack * 1.3f) ChangeState(State.Chase);
                break;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        switch (state)
        {
            case State.Patrol:
                {
                    Vector2 next = rb.position + patrolDir * moveSpeed * Time.fixedDeltaTime;
                    if (!InBounds(next))
                    {
                        SetPatrolDir(Random.insideUnitCircle.normalized);
                        return;
                    }
                    rb.MovePosition(next);
                    break;
                }

            case State.Chase:
                {
            
                    LookAtPlayerPhysics();

                    if (!player) return;

                    Vector2 dir = ((Vector2)player.position - rb.position).normalized;
                    float sqrDist = ((Vector2)player.position - rb.position).sqrMagnitude;
                    if (sqrDist <= sqrStop) return;

                    Vector2 next = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
                    if (!InBounds(next)) return;
                    rb.MovePosition(next);
                    break;
                }

            case State.Attack:
                {
              
                    LookAtPlayerPhysics();
                    
                    break;
                }
        }
    }


    void ChangeState(State next)
    {
        if (state == next) return;

       
        if (state == State.Dead) return;

        switch (state)
        {
            case State.Attack:
                SetContactDamage(false);
                break;
        }


        state = next;


        switch (state)
        {
            case State.Patrol:
                dirTimer = 0f;
                SetPatrolDir(Random.insideUnitCircle.normalized);
                break;

            case State.Chase:

                break;

            case State.Attack:
                SetContactDamage(true);
                break;

            case State.Dead:
                SetContactDamage(false);
                rb.velocity = Vector2.zero;
                break;
        }
    }

    void SetPatrolDir(Vector2 dir)
    {
        patrolDir = dir.sqrMagnitude < 0.0001f ? Vector2.right : dir.normalized;
    }

    void SetContactDamage(bool enabled)
    {
        if (!contactDamage) return;
        contactDamage.enabled = enabled;
    }

    void LookAtPlayerPhysics()
    {
        if (!player) return;
        Vector2 to = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle); 
    }

    void UpdateCameraBounds()
    {
        if (!cam) return;
        Vector3 bl = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 tr = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        minBounds = new Vector2(bl.x, bl.y);
        maxBounds = new Vector2(tr.x, tr.y);
    }

    bool InBounds(Vector2 p)
    {
        return p.x >= minBounds.x && p.x <= maxBounds.x &&
               p.y >= minBounds.y && p.y <= maxBounds.y;
    }


    public void TakeDamage(int amount)
    {
        if (isDead) return;
        hp -= Mathf.Max(0, amount);
        if (hp <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;

        // 1) Score
        ScoreManager.Instance?.AddPoint();

        // 2) apagar daño/contacto y colisiones
        SetContactDamage(false);
        EnableAllColliders(false);

        // 3) estado Dead (primero estado, luego marcar bandera)
        ChangeState(State.Dead);
        isDead = true;

        // 4) detener físicamente
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

       
        if (destroyOnDeath)
        {
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            gameObject.SetActive(false); 
        }
    }

    void EnableAllColliders(bool enable)
    {
        var cols = GetComponentsInChildren<Collider2D>(true);
        for (int i = 0; i < cols.Length; i++) cols[i].enabled = enable;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.cyan; Gizmos.DrawWireSphere(transform.position, stopRadius);
    }
}