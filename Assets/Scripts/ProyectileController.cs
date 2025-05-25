using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float projectileSpeed = 8f;
    public float lifetime = 3f;
    public LayerMask groundLayer;

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        transform.Translate(projectileSpeed * Time.deltaTime * direction, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            enemy.TakeDamage(1f);
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Player") && !collision.CompareTag("MovingPlatform") && (((1 << collision.gameObject.layer) & groundLayer) != 0))
        {
            Destroy(gameObject);
        }
    }
}