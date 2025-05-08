using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject laserPrefab;  // The laser prefab
    public Transform turretEnd;     // Position from which the laser is fired
    public float fireRate = 1f;     // Time interval between shots
    public float firingRange = 5f;  // Range at which the shooter will fire
    private float nextFireTime = 0f; // Time before the next shot can be fired

    private Transform target;       // The target to shoot at
    private ColorTag myColorTag;    // The color tag for identifying the NPC's team

    void Start()
    {
        myColorTag = GetComponent<ColorTag>();  // Get the ColorTag to determine the NPC's team color
        if (myColorTag == null)
        {
            Debug.LogError("ColorTag component not found on shooter.");
        }
    }

    void Update()
    {
        // Find the nearest opponent (if any)
        target = FindNearestOpponent();

        // If there is a target and it's time to shoot, fire a laser
        if (target != null && Time.time >= nextFireTime)
        {
            FireLaser();
            nextFireTime = Time.time + fireRate;  // Set the next fire time
        }
    }

    Transform FindNearestOpponent()
    {
        if (myColorTag == null) return null;

        teamColor.ColorChoice myTeam = myColorTag.colorTag;  // Get the NPC's team color
        float closestDistance = float.MaxValue;
        Transform nearest = null;

        // Look for the nearest opponent within range
        foreach (ColorTag other in FindObjectsOfType<ColorTag>())
        {
            if (other == myColorTag) continue;  // Skip self
            if (other.colorTag == myTeam) continue;  // Skip teammates

            if (other.CompareTag("Billion") || other.CompareTag("Base"))
            {
                float dist = Vector2.Distance(turretEnd.position, other.transform.position);
                if (dist < closestDistance && dist <= firingRange)
                {
                    closestDistance = dist;
                    nearest = other.transform;
                }
            }
        }

        return nearest;  // Return the nearest enemy transform
    }

    void FireLaser()
    {
        if (target == null) return;

        // Calculate the direction towards the target
        Vector2 direction = (target.position - turretEnd.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Instantiate the laser and initialize it with the correct position, rotation, and color
        GameObject laser = Instantiate(laserPrefab, turretEnd.position, Quaternion.Euler(0, 0, angle));
        BillionLaser laserScript = laser.GetComponent<BillionLaser>();

        if (laserScript != null)
        {
            laserScript.Initialize(turretEnd.position, Quaternion.Euler(0, 0, angle), myColorTag.colorTag);
        }
    }
}
