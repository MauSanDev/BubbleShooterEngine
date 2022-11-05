using System.Collections.Generic;
using UnityEngine;

public class BombMatchStrategy : IMatchStrategy<BubblePiece>
{
    public HashSet<Vector2Int> GetMatchCandidates(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board)
    {
        return new HashSet<Vector2Int>(GetNeighbourCoordinates(piecePosition));
    }
    
    private Vector2Int[] GetNeighbourCoordinates(Vector2Int cellPosition) // TODO: reanalize because of the alignment strategy
    {
        int diff = cellPosition.y % 2 == 0 ? -1 : 1;
        Vector2Int[] neighbours =
        {
            cellPosition,
            new Vector2Int(cellPosition.x - 1, cellPosition.y), //left
            new Vector2Int(cellPosition.x + 1, cellPosition.y), //right
            new Vector2Int(cellPosition.x, cellPosition.y - 1), //top left
            new Vector2Int(cellPosition.x + diff , cellPosition.y - 1), //top right
            new Vector2Int(cellPosition.x, cellPosition.y + 1), //bottom left
            new Vector2Int(cellPosition.x + diff, cellPosition.y + 1)// bottom right
        };

        return neighbours;
    }
}
