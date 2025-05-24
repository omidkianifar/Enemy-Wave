using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PathFinder : MonoBehaviour, IPathFinder
{
    [Header("Path Points")]
    [SerializeField] private List<Transform> pathPoints = new();

    public IReadOnlyList<Transform> PathPoints => pathPoints;
    public Vector3 StartPoint => pathPoints.Count > 0 ? pathPoints[0].position : Vector3.zero;
    public Vector3 EndPoint => pathPoints.Count > 1 ? pathPoints[pathPoints.Count - 1].position : Vector3.zero;
    public bool IsValid => pathPoints.Count >= 2 && pathPoints[0] != null && pathPoints[pathPoints.Count - 1] != null;
    public int PathLength => pathPoints.Count;

    [ContextMenu("Find All Child Points")]
    private void FindAllChildPoints()
    {
        UpdatePathPoints();
    }

    private void OnValidate()
    {
        UpdatePathPoints();
    }

    private void UpdatePathPoints()
    {
        pathPoints.Clear();
        var children = GetComponentsInChildren<Transform>()
            .Where(t => t != transform) // Exclude this object
            .OrderBy(t => t.GetSiblingIndex()) // Order by child index
            .ToList();

        // Rename points
        for (int i = 0; i < children.Count; i++)
        {
            if (i == 0)
            {
                children[i].name = "Start";
            }
            else if (i == children.Count - 1)
            {
                children[i].name = "End";
            }
            else
            {
                children[i].name = i.ToString();
            }
        }

        pathPoints.AddRange(children);

        // Validate path
        if (pathPoints.Count < 2)
        {
            Debug.LogWarning("PathFinder requires at least 2 points (start and end)!");
        }
        else
        {
            Debug.Log($"Found {pathPoints.Count} path points. First point: {pathPoints[0].name}, Last point: {pathPoints[pathPoints.Count - 1].name}");
        }
    }

    private void OnDrawGizmos()
    {
        if (pathPoints.Count < 2) return;

        // Draw path lines
        Gizmos.color = Color.yellow;
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            if (pathPoints[i] != null && pathPoints[i + 1] != null)
            {
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
            }
        }

        // Draw points with different colors
        for (int i = 0; i < pathPoints.Count; i++)
        {
            if (pathPoints[i] == null) continue;

            // Set color based on point position
            if (i == 0)
            {
                Gizmos.color = Color.blue; // Start point
            }
            else if (i == pathPoints.Count - 1)
            {
                Gizmos.color = Color.red; // End point
            }
            else
            {
                Gizmos.color = Color.green; // Middle points
            }

            // Draw circle for the point
            Gizmos.DrawWireSphere(pathPoints[i].position, 2f);
        }
    }

    public Vector3 GetPointPosition(int index)
    {
        if (index < 0 || index >= pathPoints.Count)
        {
            Debug.LogError($"Invalid path point index: {index}");
            return Vector3.zero;
        }

        return pathPoints[index].position;
    }

    public bool IsValidPath()
    {
        return pathPoints.Count >= 2 && pathPoints[0] != null && pathPoints[pathPoints.Count - 1] != null;
    }
} 