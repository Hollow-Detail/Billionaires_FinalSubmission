using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float maxDistance = 15f;
    public float damage = 2f;

    private Vector2 spawnPosition;
    private teamColor.ColorChoice myColor;

    void Start()
    {
        spawnPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (Vector2.Distance(spawnPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector2 position, Quaternion rotation, teamColor.ColorChoice color)
    {
        transform.position = position;
        transform.rotation = rotation;
        myColor = color;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Billion"))
        {
            ColorTag colorTag = other.GetComponent<ColorTag>();
            BillionHealth health = other.GetComponent<BillionHealth>();

            if (colorTag != null && health != null && colorTag.colorTag != myColor)
            {
                health.TakeDamage(damage, myColor);
                Destroy(gameObject);
            }
        }
    }
}
