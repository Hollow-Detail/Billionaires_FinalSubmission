using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BillionHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    [Range(0f, 1f)]
    public float minHealthScale = 0.05f;
    public float radius = 0.2f;
    public int segments = 64;

    private LineRenderer outerCircleRenderer;
    private LineRenderer innerCircleRenderer;

    void Start()
    {
        currentHealth = maxHealth;

        // Create outer circle
        GameObject outer = new GameObject("OuterCircle");
        outer.transform.SetParent(transform);
        outer.transform.localPosition = Vector3.zero;

        outerCircleRenderer = outer.AddComponent<LineRenderer>();
        SetupCircleRenderer(outerCircleRenderer, Color.white);
        DrawCircle(outerCircleRenderer, radius);

        // Create inner circle
        GameObject inner = new GameObject("InnerCircle");
        inner.transform.SetParent(transform);
        inner.transform.localPosition = Vector3.zero;

        innerCircleRenderer = inner.AddComponent<LineRenderer>();
        SetupCircleRenderer(innerCircleRenderer, Color.white);
        DrawCircle(innerCircleRenderer, radius); // will get scaled later
    }

    void Update()
    {
        float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);
        float visualScale = Mathf.Lerp(minHealthScale, 1f, healthPercent);
        DrawCircle(innerCircleRenderer, radius * visualScale);
    }

    public void TakeDamage(float amount, teamColor.ColorChoice agressor)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0f, currentHealth);

        if (currentHealth <= 0f)
        {
            ExperienceObserver.Instance?.ReportDestruction(agressor);
            Destroy(gameObject);

        }
    }

    void SetupCircleRenderer(LineRenderer lr, Color color)
    {
        lr.useWorldSpace = false;
        lr.loop = true;
        lr.widthMultiplier = 0.05f;
        lr.positionCount = segments;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = color;
    }

    void DrawCircle(LineRenderer lr, float radius)
    {
        float angleStep = 2 * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
            lr.SetPosition(i, pos);
        }
    }
}
