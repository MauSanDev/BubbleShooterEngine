using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterDefaultMatchStrategy : IMatchStrategy<BubblePiece>
{
    private const int MIN_PIECES_TO_MATCH = 3;
    
    public List<Vector2Int> GetMatchCandidates(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board)
    {
        List<Vector2Int> matches = new List<Vector2Int>();
        matches.Add(piecePosition);
        SearchMatches(piecePosition, matchCondition, board, ref matches);

        if (matches.Count < MIN_PIECES_TO_MATCH)
        {
            matches.RemoveAll(x => !(board.GetPiece(x) is ISpecialPiece<BubblePiece>));
        }
        
        return matches;
    }
    
    private void SearchMatches(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board, ref List<Vector2Int> matches)
    {
        Vector2Int[] neighbourPositions = GetNeighbourCoordinates(piecePosition);

        foreach (Vector2Int neighbour in neighbourPositions)
        {
            if(neighbour.x < 0 || neighbour.y < 0) 
                continue;

            BubblePiece neighbourPiece = board.GetPiece(neighbour);
            if (neighbourPiece != null && !matches.Contains(neighbour) && matchCondition.IsMatch(neighbourPiece))
            {
                matches.Add(neighbour);
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
