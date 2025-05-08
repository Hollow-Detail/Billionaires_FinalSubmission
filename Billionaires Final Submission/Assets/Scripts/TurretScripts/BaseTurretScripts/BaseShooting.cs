using UnityEngine;

public class BaseShooting : MonoBehaviour
{
    public GameObject baseProjectilePrefab;
    public Transform firePoint; // Assign this to the end of the base turret barrel
    public float fireInterval = 2f;
    public float firingRange = 6f;

    private float fireTimer = 0f;
    private Transform target;
    private ColorTag myColorTag;

    void Start()
    {
        myColorTag = GetComponentInParent<ColorTag>();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        target = FindNearestOpponent();

        if (target != null && fireTimer >= fireInterval)
        {
            float dist = Vector2.Distance(transform.position, target.position);
            if (dist <= firingRange)
            {
                FireProjectile();
                fireTimer = 0f;
            }
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
            if (!other.CompareTag("Billion")) continue;

            float dist = Vector2.Distance(transform.position, other.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                nearest = other.transform;
            }
        }

        return nearest;
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(baseProjectilePrefab, firePoint.position, firePoint.rotation);
        BaseProjectile proj = projectile.GetComponent<BaseProjectile>();

        if (proj != null)
        {
            proj.Initialize(firePoint.position, firePoint.rotation, myColorTag.colorTag);
        }
    }
}