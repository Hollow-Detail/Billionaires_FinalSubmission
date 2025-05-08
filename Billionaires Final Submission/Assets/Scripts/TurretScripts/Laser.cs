using UnityEngine;

public class BillionLaser : MonoBehaviour
{
    public float speed = 5f;
    public float maxDistance = 10f;
    public float damage = 1f;

    private Vector2 spawnPosition;
    private teamColor.ColorChoice myColor;

    void Start()
    {
        spawnPosition = transform.position;
    }

    void Update()
    {
        // Move the laser forward
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Destroy the laser if it travels beyond the max distance
        if (Vector2.Distance(spawnPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    // Initialize the laser with position, rotation, and color info
    public void Initialize(Vector2 position, Quaternion rotation, teamColor.ColorChoice color)
    {
        transform.position = position;
        transform.rotation = rotation;
        myColor = color;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        // Destroy on collision with walls
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        // Apply damage if the laser hits a Billion (enemy NPC)
        if (other.CompareTag("Billion"))
        {
            ColorTag colorTag = other.GetComponent<ColorTag>();
            BillionHealth health = other.GetComponent<BillionHealth>();

            if (colorTag != null && health != null && colorTag.colorTag != myColor)
            {
                health.TakeDamage(damage, myColor);  // Apply damage to the enemy Billion
                Destroy(gameObject);  // Destroy the laser after it hits
            }
        }

        if (other.CompareTag("Base"))
        {
            ColorTag colorTag = other.GetComponent<ColorTag>();
            BaseHealth health = other.GetComponent<BaseHealth>();

            if(colorTag != null && health != null && colorTag.colorTag != myColor)
            {
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}

