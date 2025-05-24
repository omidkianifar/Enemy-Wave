using UnityEngine;

public interface IPathFinder
{
    Vector3 GetPointPosition(int index);
    int PathLength { get; }
    bool IsValidPath();
    bool IsValid { get; }
    Vector3 StartPoint { get; }
    Vector3 EndPoint { get; }
} 