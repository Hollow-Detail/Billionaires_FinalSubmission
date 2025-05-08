using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius = 3f;      // The radius of the explosion
    public float explosionForce = 5f;       // The force applied to push objects away from the explosion
    public float damage = 20f;              // Damage dealt by the explosion
    public float explosionDuration = 1f;    // Time before the explosion disappears

    private teamColor.ColorChoice explosionColorTag;  // Color tag of the explosion

    void Start()
    {
        // Destroy the explosion after the set duration
        Destroy(gameObject, explosionDuration);

        // Apply damage and force to nearby objects
        ApplyExplosionEffects();
    }

    void ApplyExplosionEffects()
    {
        // Find all nearby objects within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var collider in colliders)
        {
            // Check if the collider belongs to an opposing base or billionaire
            var colorTag = collider.GetComponent<ColorTag>();
            if (colorTag != null && colorTag.colorTag != explosionColorTag)
            {
                // Deal damage to opposing billions or bases
                BillionHealth health = collider.GetComponent<BillionHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage, explosionColorTag);  // Make sure you have a TakeDamage method in BillionHealth
                }

                // Apply pushback force to billions
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = (collider.transform.position - transform.position).normalized;
                    rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    // Optional: To visually show the explosion radius (for debugging purposes)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
