using UnityEngine;
using System.Collections.Generic;

public class FlagHandler2D : MonoBehaviour
{
    [Header("Flag Prefabs (Assign in Inspector)")]
    [SerializeField] private GameObject greenFlagPrefab;
    [SerializeField] private GameObject yellowFlagPrefab;
    [SerializeField] private GameObject redFlagPrefab;
    [SerializeField] private GameObject blueFlagPrefab;

    public enum TeamColor { Green, Yellow, Red, Blue }

    private Dictionary<TeamColor, List<GameObject>> teamFlags = new Dictionary<TeamColor, List<GameObject>>();
    private Dictionary<TeamColor, GameObject> teamPrefabs = new Dictionary<TeamColor, GameObject>();
    private Dictionary<GameObject, Vector3> dragStartPositions = new Dictionary<GameObject, Vector3>();

    private LineRenderer dragLine;
    private GameObject draggingFlag = null;

    void Start()
    {
        // Initialize flag lists for all teams
        foreach (TeamColor color in System.Enum.GetValues(typeof(TeamColor)))
            teamFlags[color] = new List<GameObject>();

        // Set prefab references
        teamPrefabs[TeamColor.Green] = greenFlagPrefab;
        teamPrefabs[TeamColor.Yellow] = yellowFlagPrefab;
        teamPrefabs[TeamColor.Red] = redFlagPrefab;
        teamPrefabs[TeamColor.Blue] = blueFlagPrefab;

        // Setup line renderer for drag visuals
        GameObject dragLineObj = new GameObject("DragLine");
        dragLine = dragLineObj.AddComponent<LineRenderer>();
        dragLine.startWidth = 0.05f;
        dragLine.endWidth = 0.05f;
        dragLine.material = new Material(Shader.Find("Sprites/Default"));
        dragLine.positionCount = 0;
        dragLine.sortingOrder = 10;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            HandleFlagPlacement(TeamColor.Green, 0);

        if (Input.GetKey(KeyCode.Alpha2))
            HandleFlagPlacement(TeamColor.Yellow, 0);

        if (Input.GetKey(KeyCode.Alpha3))
            HandleFlagPlacement(TeamColor.Red, 0);

        if (Input.GetKey(KeyCode.Alpha4))
            HandleFlagPlacement(TeamColor.Blue, 0);

        HandleDragLogic();
    }

    void HandleFlagPlacement(TeamColor color, int mouseButton)
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            GameObject closest = GetClosestFlag(color, mouseWorldPos);

            if (closest != null && IsMouseOver(closest, mouseWorldPos))
            {
                draggingFlag = closest;
                dragStartPositions[draggingFlag] = closest.transform.position;
                dragLine.positionCount = 2;
                dragLine.SetPosition(0, dragStartPositions[draggingFlag]);
                dragLine.SetPosition(1, mouseWorldPos);
                return;
            }

            if (teamFlags[color].Count < 2)
            {
                GameObject prefab = teamPrefabs[color];
                if (prefab == null)
                {
                    Debug.LogWarning($"Missing prefab for {color} team.");
                    return;
                }

                GameObject newFlag = Instantiate(prefab, mouseWorldPos, Quaternion.identity);
                teamFlags[color].Add(newFlag);
            }
            else
            {
                GameObject nearestFlag = GetClosestFlag(color, mouseWorldPos);
                if (nearestFlag != null)
                    nearestFlag.transform.position = mouseWorldPos;
            }
        }

        if (Input.GetMouseButton(mouseButton) && draggingFlag != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            dragLine.SetPosition(1, mouseWorldPos);
        }

        if (Input.GetMouseButtonUp(mouseButton) && draggingFlag != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            draggingFlag.transform.position = mouseWorldPos;
            draggingFlag = null;
            dragLine.positionCount = 0;
        }
    }

    GameObject GetClosestFlag(TeamColor color, Vector3 position)
    {
        GameObject closest = null;
        float closestDist = float.MaxValue;

        foreach (var flag in teamFlags[color])
        {
            float dist = Vector3.Distance(position, flag.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = flag;
            }
        }

        return closest;
    }

    bool IsMouseOver(GameObject obj, Vector3 mousePos)
    {
        float radius = 0.5f; // Adjust based on flag size
        return Vector3.Distance(mousePos, obj.transform.position) < radius;
    }

    void HandleDragLogic()
    {
        if (draggingFlag != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            dragLine.SetPosition(1, mouseWorldPos);
        }
    }
}
