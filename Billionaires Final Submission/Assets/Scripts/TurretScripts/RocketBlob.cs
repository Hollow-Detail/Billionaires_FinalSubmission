using UnityEngine;

public class RocketBlob : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 10f;
    public GameObject explosionPrefab;
    public float maxDistance = 5f;

    private Vector2 spawnPosition;
    private Transform target;
    public teamColor.ColorChoice myColorTag;

    void Start()
    {
        spawnPosition = transform.position;
        
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Destroy the laser if it travels beyond the max distance
        if (Vector2.Distance(spawnPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    // Initialize the rocket blob with position, rotation, and color info
    public void Initialize(Vector2 position, Quaternion rotation, teamColor.ColorChoice color, Transform target)
    {
        transform.position = position;
        transform.rotation = rotation;
        myColorTag = color;
        this.target = target;
    }
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("I collided with " + collision.gameObject.name);
        GameObject other = collision.gameObject;

        if (explosionPrefab != null)
        {

            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            ColorTag explosionColorTag = explosion.GetComponent<ColorTag>();
            Destroy(gameObject);

        }
    }
}
