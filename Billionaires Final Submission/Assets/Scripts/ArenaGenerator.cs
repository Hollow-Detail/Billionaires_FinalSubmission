using UnityEngine;
using System.Collections.Generic;

public class ArenaGenerator : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public int maxSteps = 500;
    public float safeRadius = 2f;

    private int width, height;
    private TileType[,] grid;
    private HashSet<Vector2Int> spawnerFloorZones = new HashSet<Vector2Int>();

    enum TileType { Wall, Floor }

    void Start()
    {
        Vector2Int size = GetCameraVisibleTileBounds();
        width = size.x;
        height = size.y;

        Debug.Log($"Arena Size: {width}x{height}");

        GenerateArena();
        SpawnTiles();
    }

    Vector2Int GetCameraVisibleTileBounds()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No Camera found! Please add a Camera to the scene.");
            return Vector2Int.zero;
        }

        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        return new Vector2Int(Mathf.FloorToInt(camWidth), Mathf.FloorToInt(camHeight));
    }

    void GenerateArena()
    {
        grid = new TileType[width, height];

        Vector2Int center = new Vector2Int(width / 2, height / 2);
        grid[center.x, center.y] = TileType.Floor;

        // Random walk from center
        int steps = 0;
        Vector2Int current = center;

        while (steps < maxSteps)
        {
            Vector2Int dir = GetRandomDirection();
            Vector2Int next = current + dir;
            if (InBounds(next) && !IsBorder(next))
            {
                current = next;
                grid[current.x, current.y] = TileType.Floor;
                steps++;
            }
        }

        // Carve safe zones around spawners and store them
        CarveSpawnerSafeZones();

        // Ensure each spawner zone is connected to the central floor region
        EnsureSpawnerZonesConnected(center);

        // Apply outer walls
        ApplyWallBorders();
    }

    void CarveSpawnerSafeZones()
    {
        Vector3 offset = new Vector3(-width / 2f, -height / 2f, 0f);
        float carveRadius = safeRadius + 1f;

        BillionSpawner[] spawners = FindObjectsOfType<BillionSpawner>();

        foreach (BillionSpawner spawner in spawners)
        {
            Vector3 localPos = spawner.transform.position - offset;
            int cx = Mathf.RoundToInt(localPos.x);
            int cy = Mathf.RoundToInt(localPos.y);
            int radius = Mathf.CeilToInt(carveRadius);

            for (int x = cx - radius; x <= cx + radius; x++)
            {
                for (int y = cy - radius; y <= cy + radius; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (InBounds(pos) && !IsBorder(pos))
                    {
                        float dist = Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy));
                        if (dist <= carveRadius)
                        {
                            grid[x, y] = TileType.Floor;
                            spawnerFloorZones.Add(pos);
                        }
                    }
                }
            }
        }
    }

    void EnsureSpawnerZonesConnected(Vector2Int center)
    {
        HashSet<Vector2Int> reachable = FloodFill(center);

        foreach (Vector2Int zoneTile in spawnerFloorZones)
        {
            if (!reachable.Contains(zoneTile))
            {
                Vector2Int nearest = FindNearestReachable(zoneTile, reachable);
                CarvePath(zoneTile, nearest);
                reachable = FloodFill(center); // Update reachable after each connection
            }
        }
    }

    HashSet<Vector2Int> FloodFill(Vector2Int start)
    {
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            foreach (Vector2Int dir in FourDirections())
            {
                Vector2Int next = current + dir;
                if (InBounds(next) && grid[next.x, next.y] == TileType.Floor && !visited.Contains(next))
                {
                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }
        }
        return visited;
    }

    Vector2Int FindNearestReachable(Vector2Int target, HashSet<Vector2Int> reachable)
    {
        Vector2Int closest = target;
        float minDist = float.MaxValue;
        foreach (Vector2Int pos in reachable)
        {
            float dist = Vector2.Distance(pos, target);
            if (dist < minDist)
            {
                minDist = dist;
                closest = pos;
            }
        }
        return closest;
    }

    void CarvePath(Vector2Int from, Vector2Int to)
    {
        Vector2Int pos = from;
        while (pos != to)
        {
            grid[pos.x, pos.y] = TileType.Floor;

            if (pos.x != to.x)
                pos.x += (to.x > pos.x) ? 1 : -1;
            else if (pos.y != to.y)
                pos.y += (to.y > pos.y) ? 1 : -1;
        }
    }

    void ApplyWallBorders()
    {
        for (int x = 0; x < width; x++)
        {
            grid[x, 0] = TileType.Wall;
            grid[x, height - 1] = TileType.Wall;
        }

        for (int y = 0; y < height; y++)
        {
            grid[0, y] = TileType.Wall;
            grid[width - 1, y] = TileType.Wall;
        }
    }

    Vector2Int GetRandomDirection()
    {
        int r = Random.Range(0, 4);
        return r switch
        {
            0 => Vector2Int.up,
            1 => Vector2Int.right,
            2 => Vector2Int.down,
            _ => Vector2Int.left,
        };
    }

    bool InBounds(Vector2Int pos)
    {
        return pos.x > 0 && pos.y > 0 && pos.x < width - 1 && pos.y < height - 1;
    }

    bool IsBorder(Vector2Int pos)
    {
        return pos.x == 0 || pos.y == 0 || pos.x == width - 1 || pos.y == height - 1;
    }

    List<Vector2Int> FourDirections()
    {
        return new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };
    }

    void SpawnTiles()
    {
        Vector3 offset = new Vector3(-width / 2f, -height / 2f, 0f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = new Vector3(x, y, 0) + offset;
                GameObject prefab = (grid[x, y] == TileType.Floor) ? floorPrefab : wallPrefab;

                if (prefab != null)
                    Instantiate(prefab, worldPos, Quaternion.identity);
            }
        }
    }
}