using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float delayBeforeDisappear = 0.5f;
    public float fadeDuration = 1f;
    public float respawnDelay = 3f;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private bool playerOnPlatform = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !playerOnPlatform)
        {
            playerOnPlatform = true;
            StartCoroutine(DisappearPlatform());
        }
    }

    System.Collections.IEnumerator DisappearPlatform()
    {
        yield return new WaitForSeconds(delayBeforeDisappear);

        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        col.enabled = true;

        playerOnPlatform = false;
    }
}
