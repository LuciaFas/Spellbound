using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject pointA; 
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;
    public float speed;

    private bool isWaiting = false;
    private bool isStunned = false;
    private float stunTimer = 0f;
    private float waitTimer = 0f;

    public Transform player;
    public float lookRadius = 5f;

    public Transform attackPoint;
    public float attackRange = 0.8f;
    public LayerMask playerLayer;
    public float attackDamage = 0.2f;

    private float health = 3f;
    public LogicManager gameManager;


    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       currentPoint = pointB.transform;
       animator.SetBool("Moving", true);

       Flip();
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
            }
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= lookRadius)
        {
            ChasePlayer();
            return;
        }

        Patrol();
    }

    private void Patrol()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                Flip();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        animator.SetBool("Moving", true);

        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            animator.SetBool("Moving", false);
            isWaiting = true;
            waitTimer = 2f;
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            animator.SetBool("Moving", false);
            isWaiting = true;
            waitTimer = 2f;
            currentPoint = pointB.transform;
        }
    }

    private void ChasePlayer()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            animator.SetBool("Attack", true);
            rb.linearVelocity = Vector2.zero;
            return;
        }

        animator.SetBool("Attack", false);
        animator.SetBool("Moving", true);

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, 0);

        Vector3 s = transform.localScale;
        if (dir.x > 0f)
        {
            s.x = -Mathf.Abs(s.x);
        }
        else
        {
            s.x = Mathf.Abs(s.x);
        }
        transform.localScale = s;


    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            animator.SetTrigger("DeathTrigger");
            rb.linearVelocity = Vector2.zero;
            Scoring.totalScore += 50;
            gameManager.UpdateScore();
            Invoke(nameof(Die), 1f);
        }

        isStunned = true;
        animator.SetTrigger("StunedTrigger");
        stunTimer = 2f;
    }

    void Die()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
