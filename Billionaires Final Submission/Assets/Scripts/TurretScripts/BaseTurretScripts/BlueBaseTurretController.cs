using UnityEngine;

public class BlueBaseTurretController : MonoBehaviour
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
            Vector3 direction = target.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Correct for sprite facing right
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
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

            if (other.CompareTag("Billion"))
            {
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
