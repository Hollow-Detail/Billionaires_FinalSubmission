using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject powerupPrefab;
    public float spawnInterval = 20f;

    private Camera mainCamera;
    private GameObject currentPowerup;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentPowerup == null)
                TrySpawnPowerup();
        }
    }

    void TrySpawnPowerup()
    {
        Vector2 arenaMin = GetArenaMin();
        Vector2 arenaMax = GetArenaMax();

        for (int attempt = 0; attempt < 50; attempt++)
        {
            Vector2 pos = new Vector2(
                Random.Range(arenaMin.x, arenaMax.x),
                Random.Range(arenaMin.y, arenaMax.y)
            );

            Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.5f);
            bool blocked = false;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Wall") || hit.CompareTag("Base"))
                {
                    blocked = true;
                    break;
                }
            }

            if (!blocked)
            {
                currentPowerup = Instantiate(powerupPrefab, pos, Quaternion.identity);
                break;
            }
        }
    }

    Vector2 GetArenaMin()
    {
        float width = mainCamera.orthographicSize * mainCamera.aspect;
        return new Vector2(
            mainCamera.transform.position.x - width,
            mainCamera.transform.position.y - mainCamera.orthographicSize
        );
    }

    Vector2 GetArenaMax()
    {
        float width = mainCamera.orthographicSize * mainCamera.aspect;
        return new Vector2(
            mainCamera.transform.position.x + width,
            mainCamera.transform.position.y + mainCamera.orthographicSize
        );
    }

    public void ClearPowerup(GameObject powerup)
    {
        if (currentPowerup == powerup)
        {
            Destroy(powerup);
            currentPowerup = null;
        }
    }
}
