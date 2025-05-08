using UnityEngine;

public class LaserShot : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 10f;
    private Vector3 spawnPosition;
    private ColorTag shooterColorTag;

    void Start()
    {
        spawnPosition = transform.position;
    }

    public void SetShooter(ColorTag colorTag)
    {
        shooterColorTag = colorTag;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Vector3.Distance(spawnPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject);
            return;
        }

        ColorTag otherColorTag = other.GetComponent<ColorTag>();
        if (otherColorTag != null && shooterColorTag != null && otherColorTag.colorTag != shooterColorTag.colorTag)
        {
            BillionHealth targetHealth = other.GetComponent<BillionHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(10f, shooterColorTag.colorTag); 
            }

            Destroy(gameObject);
        }
    }
}
