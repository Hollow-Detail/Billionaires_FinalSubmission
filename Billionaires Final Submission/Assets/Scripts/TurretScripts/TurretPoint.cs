using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private Transform parentBillion;
    private ColorTag myColorTag;
    

    void Start()
    {
        parentBillion = transform.parent;
        myColorTag = parentBillion.GetComponent<ColorTag>();
    }

    void Update()
    {
        Transform nearestOpponent = FindNearestOpponent();

        if (nearestOpponent != null)
        {
            Vector2 direction = (nearestOpponent.position - transform.position).normalized;

            // Rotate turret so its RIGHT side (tip) points at the opponent
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            // Default orientation when no opponents remain
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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

            if (other.CompareTag("Billion") || other.CompareTag("Base"))
            {
                float dist = Vector2.Distance(parentBillion.position, other.transform.position);
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