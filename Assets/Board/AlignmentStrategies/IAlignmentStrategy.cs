using UnityEngine;

public interface IAlignmentStrategy
{
    Vector3 GridToLocalPosition(Vector2Int gridPosition);
    Vector2Int LocalToGridPosition(Vector3 localPosition);
}
