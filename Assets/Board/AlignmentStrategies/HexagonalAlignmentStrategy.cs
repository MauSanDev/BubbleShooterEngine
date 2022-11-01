using UnityEngine;

public class HexagonalAlignmentStrategy : IAlignmentStrategy
{
    public Vector3 GetPiecePosition(Vector2Int pieceCoordinates)
    {
        float x = pieceCoordinates.y % 2 == 0 ? pieceCoordinates.x : pieceCoordinates.x + .5f;
        float y = -(pieceCoordinates.y * .75f);
        return new Vector3(x, y, 1);
    }
}
