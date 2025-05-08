using UnityEngine;

public class BaseTurretController : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrees per second

    private Transform target;
    private ColorTag myColorTag;

    void Start()
    {
        myColorTag = GetComponentInParent<ColorTag>();
    }

    void Update()
    {
        target = FindNearestOpponent();

        if (target != null)
        {
            // Calculate direction and desired angle
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Build the desired rotation
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Smoothly rotate toward the target at rotationSpeed degrees per second
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    Transform FindNearestOpponent()
    {
        teamColor.ColorChoice myTeam = myColorTag.colorTag;
        float closestDistance = float.MaxValue;
        Transform nearest = null;

        foreach (ColorTag other in FindObjectsOfType<ColorTag>())
        {
            if (other == myColorTag) continue;
            if (other.colorTag == myTeam) continue;

            if (other.CompareTag("Billion")) { 
            float dist = Vector2.Distance(transform.position, other.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                nearest = other.transform;
            }
            }
        }

        return nearest;
    }
}
