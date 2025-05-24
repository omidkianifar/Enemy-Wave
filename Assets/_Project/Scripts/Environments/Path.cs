using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Path
{
    [SerializeField] private string id;
    [SerializeField] private List<Transform> points = new();

    public string Id => id;
    public IReadOnlyList<Transform> Points => points;
    public Vector3 StartPoint => points.Count > 0 ? points[0].position : Vector3.zero;
    public Vector3 EndPoint => points.Count > 1 ? points[points.Count - 1].position : Vector3.zero;
    public bool IsValid => points.Count >= 2 && points[0] != null && points[points.Count - 1] != null;
    public int PointCount => points.Count;

    public Vector3 GetPointPosition(int index)
    {
        if (index < 0 || index >= points.Count)
        {
            Debug.LogError($"Invalid path point index: {index}");
            return Vector3.zero;
        }

        return points[index].position;
    }

    public void SetPoints(List<Transform> newPoints)
    {
        points.Clear();
        points.AddRange(newPoints);
    }
} 