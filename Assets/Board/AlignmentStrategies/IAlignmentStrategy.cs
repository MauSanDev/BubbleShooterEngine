using UnityEngine;

public interface IAlignmentStrategy
{
    Vector3 GetPiecePosition(Vector2Int piece);
}
