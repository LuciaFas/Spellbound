using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 7f;
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround = false;

    private Animator playerAnimation;

    private Vector3 respownPoint;
    public GameObject fallDetector;

    public LogicManager gameManager;
    public HealthBar healthBar;

    public GameObject orb;
    public Transform firePoint;

    private bool canAttack = true;
    private float attackTimer = 0f;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        respownPoint = transform.position;
        gameManager.UpdateScore();
    }

    void Update()
    {

        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (direction > 0f)
        {
            player.linearVelocity = new Vector2(direction * speed, player.linearVelocityY);
            transform.localScale = new Vector2(0.7148194f, 0.7148194f);
        }
        else if (direction < 0f)
        {
            player.linearVelocity = new Vector2(direction * speed, player.linearVelocityY);
            transform.localScale = new Vector2(-0.7148194f, 0.7148194f);
        }
        else
        {
            player.linearVelocity = new Vector2(0f, player.linearVelocityY);
        }


        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.linearVelocity = new Vector2(player.linearVelocityX, jumpSpeed);
        }

        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                canAttack = true;
            }
        }

        if (Input.GetMouseButtonDown(0) && isTouchingGround && canAttack)
        {
            playerAnimation.SetTrigger("Attack");
            canAttack = false;
            attackTimer = 2.5f;
            ShootOrb();
        }

        playerAnimation.SetFloat("Speed", Mathf.Abs(player.linearVelocityX));
        playerAnimation.SetBool("OnGround", isTouchingGround);

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "FallDetector": transform.position = respownPoint;
                TakeDamage(0.1f); break;
            case "CheckPoint": respownPoint = transform.position; break;
            case "NextLevel": SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                respownPoint = transform.position; break;
            case "PreviousLevel": SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                respownPoint = transform.position; break;

            case "Mushroom": player.linearVelocityY = 0f; player.AddForce(new Vector2(player.linearVelocityX, 12f), ForceMode2D.Impulse); break;

            case "GemTier1": Scoring.totalScore += 10; 
                collision.gameObject.SetActive(false);
                gameManager.UpdateScore(); break;
            case "GemTier2": Scoring.totalScore += 25;
                collision.gameObject.SetActive(false);
                gameManager.UpdateScore(); ; break;
            case "GemTier3": Scoring.totalScore += 50;
                collision.gameObject.SetActive(false);
                gameManager.UpdateScore(); break;
            case "GemTier4": Scoring.totalScore += 100;
                collision.gameObject.SetActive(false);
                gameManager.UpdateScore(); break;
            case "GemTier5": Scoring.totalScore += 250;
                collision.gameObject.SetActive(false);
                gameManager.UpdateScore(); break;

            case "LowPotion":
                collision.gameObject.SetActive(false);
                healthBar.Heal(0.1f); break;
            case "MediumPotion":
                collision.gameObject.SetActive(false);
                healthBar.Heal(0.2f); break;
            case "HighPotion":
                collision.gameObject.SetActive(false);
                healthBar.Heal(0.3f); break;

            case "AttackRadius":
                TakeDamage(0.2f);
                playerAnimation.SetTrigger("Damage"); break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            TakeDamage(0.002f);
            playerAnimation.SetTrigger("Damage");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = collision.transform; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
        }
    }
    void ShootOrb()
    {
        GameObject newProjectile = Instantiate(orb, firePoint.position, firePoint.rotation);

        ProjectileController projectileScript = newProjectile.GetComponent<ProjectileController>();

        Vector2 projectileDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        projectileScript.SetDirection(projectileDirection);
    }

    void TakeDamage(float damage)
    {
        healthBar.Damage(damage);

        if (Health.totalHealth <= 0f)
        {
            playerAnimation.SetTrigger("Die");
            gameManager.GameOver();
            player.linearVelocity = Vector2.zero;
            GetComponent<PlayerController>().enabled = false;
        }
    }

}
