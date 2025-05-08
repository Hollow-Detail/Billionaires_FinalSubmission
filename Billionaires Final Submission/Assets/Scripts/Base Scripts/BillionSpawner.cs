using UnityEngine;
using System.Collections;

public class BillionSpawner : MonoBehaviour
{
    public int numberToSpawn = 100;
    public float spawnRadius = 1f;
    public float minDistanceBetween = 0.15f;
    public int maxAttemptsPerBillion = 50;
    public float spawnInterval = 0.5f;

    public GameObject billionPrefab;
    public GameObject specialBillionPrefab;
    public int specialBillionCount = 5;

    private int remainingToSpawn;
    private int remainingSpecialSpawns = 0;
    private Collider2D baseCollider;

    void Start()
    {
        baseCollider = GetComponent<Collider2D>();
        if (baseCollider == null)
        {
            Debug.LogWarning("No Collider2D found on the GameObject. Overlap checks may fail.");
        }

        remainingToSpawn = numberToSpawn;
        StartCoroutine(SpawnBillionsCoroutine());
    }

    IEnumerator SpawnBillionsCoroutine()
    {
        while (remainingToSpawn > 0)
        {
            bool placed = false;

            for (int attempt = 0; attempt < maxAttemptsPerBillion; attempt++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector2 spawnPos = (Vector2)transform.position + randomOffset;

                if (baseCollider != null && baseCollider.OverlapPoint(spawnPos))
                    continue;

                Collider2D hit = Physics2D.OverlapCircle(spawnPos, minDistanceBetween);
                if (hit != null && hit.CompareTag("Wall"))
                    continue;

                GameObject prefabToSpawn = remainingSpecialSpawns > 0 ? specialBillionPrefab : billionPrefab;
                GameObject spawned = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

                if (remainingSpecialSpawns > 0)
                    remainingSpecialSpawns--;

                placed = true;
                break;
            }

            if (!placed)
            {
                Debug.LogWarning($"Could not place billion after {maxAttemptsPerBillion} attempts.");
            }

            remainingToSpawn--;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void TriggerSpecialBillionPhase()
    {
        remainingSpecialSpawns += specialBillionCount;
        Debug.Log($"Special billion phase triggered: next {specialBillionCount} spawns will be special.");
    }
}
