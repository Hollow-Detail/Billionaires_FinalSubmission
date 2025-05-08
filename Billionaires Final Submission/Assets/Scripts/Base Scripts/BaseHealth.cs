using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public float barRadius = 1.2f;
    public float barThickness = 0.2f;
    public int segments = 100;

    public Color healthColor = Color.green;

    public float startAngle = 0f; // in degrees
    public bool clockwise = true;

    private LineRenderer ring;
    private GameObject ringObject;

    void Start()
    {
        currentHealth = maxHealth;
        CreateRing();
        UpdateRing();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
        UpdateRing();
        if (currentHealth <= 0f)
        {
            Destroy(ringObject);
            Destroy(gameObject);
        }
    }

    void CreateRing()
    {
        ringObject = new GameObject("HealthRing");
        ringObject.transform.SetParent(transform);
        ringObject.transform.localPosition = Vector3.zero;

        ring = ringObject.AddComponent<LineRenderer>();
        ring.useWorldSpace = false;
        ring.loop = true;
        ring.startWidth = barThickness;
        ring.endWidth = barThickness;
        ring.material = new Material(Shader.Find("Sprites/Default"));
        ring.startColor = healthColor;
        ring.endColor = healthColor;
    }

    void UpdateRing()
    {
        float healthPercent = currentHealth / maxHealth;
        int pointsToDraw = Mathf.CeilToInt(segments * healthPercent);
        Vector3[] positions = new Vector3[pointsToDraw];

        float angleStep = 360f / segments;
        for (int i = 0; i < pointsToDraw; i++)
        {
            float angleDeg = startAngle + (clockwise ? -i : i) * angleStep;
            float angleRad = angleDeg * Mathf.Deg2Rad;
            float x = Mathf.Cos(angleRad) * barRadius;
            float y = Mathf.Sin(angleRad) * barRadius;
            positions[i] = new Vector3(x, y, 0);
        }

        ring.positionCount = positions.Length;
        ring.SetPositions(positions);
    }
}
