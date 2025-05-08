using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(ColorTag))]
public class BillionMovement : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 4f;
    public float slowingDistance = 1.5f;

    private Rigidbody2D rb;
    private ColorTag colorTag;
    private Transform targetFlag;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colorTag = GetComponent<ColorTag>();
    }

    void FixedUpdate()
    {
        FindNearestMatchingFlag();
        MoveTowardTarget();
    }

    void FindNearestMatchingFlag()
    {
        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        float closestDistance = Mathf.Infinity;
        Transform closestFlag = null;

        foreach (GameObject flag in flags)
        {
            ColorTag flagColor = flag.GetComponent<ColorTag>();
            if (flagColor != null && flagColor.colorTag == colorTag.colorTag)
            {
                float distance = Vector3.Distance(transform.position, flag.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFlag = flag.transform;
                }
            }
        }

        targetFlag = closestFlag;
    }

    void MoveTowardTarget()
    {
        if (targetFlag == null)
            return;

        Vector2 direction = ((Vector2)targetFlag.position - rb.position);
        float distance = direction.magnitude;
        direction.Normalize();

        float speedFactor = 1f;

        if (distance < slowingDistance)
        {
            // Decelerate as we get close
            speedFactor = Mathf.Clamp01(distance / slowingDistance);
        }

        Vector2 desiredVelocity = direction * maxSpeed * speedFactor;
        Vector2 steering = desiredVelocity - rb.velocity;

        rb.AddForce(steering * acceleration);
    }

    // Optional: For debug visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, slowingDistance);
    }
}
