using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterDefaultMatchStrategy : IMatchStrategy<BubblePiece>
{
    public List<BubblePiece> GetMatchCandidates(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board)
    {
        List<BubblePiece> matches = new List<BubblePiece>();
        SearchMatches(piecePosition, matchCondition, board, ref matches);

        return matches;
    }
    
    private void SearchMatches(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board, ref List<BubblePiece> matches)
    {
        Vector2Int[] neighbourPositions = GetNeighbourCoordinates(piecePosition);

        foreach (Vector2Int neighbour in neighbourPositions)
        {
            if(neighbour.x < 0 || neighbour.y < 0) 
                continue;

            BubblePiece neighbourPiece = board.GetPiece(neighbour);
            if (neighbourPiece != null && !matches.Contains(neighbourPiece) && matchCondition.IsMatch(neighbourPiece))
            {
                matches.Add(neighbourPiece);
                SearchMatches(neighbour, matchCondition, board, ref matches);
            }
        }
    }
    
    private Vector2Int[] GetNeighbourCoordinates(Vector2Int cellPosition) // TODO: reanalize because of the alignment strategy
    {
        int diff = cellPosition.y % 2 == 0 ? -1 : 1;
        Vector2Int[] neighbours =
        {
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
