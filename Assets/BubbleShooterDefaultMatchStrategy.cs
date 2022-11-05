using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterDefaultMatchStrategy : IMatchStrategy<BubblePiece>
{
    private const int MIN_PIECES_TO_MATCH = 3;
    
    public HashSet<Vector2Int> GetMatchCandidates(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board)
    {
        HashSet<Vector2Int> matches = new HashSet<Vector2Int>();
        HashSet<Vector2Int> specials = new HashSet<Vector2Int>();

        matches.Add(piecePosition);
        SearchMatches(piecePosition, matchCondition, board, ref matches, ref specials);

        if (matches.Count >= MIN_PIECES_TO_MATCH)
        {
            specials.UnionWith(matches);
        }
        
        return specials;
    }
    
    private void SearchMatches(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board, ref HashSet<Vector2Int> matches, ref HashSet<Vector2Int> specials)
    {
        Vector2Int[] neighbourPositions = GetNeighbourCoordinates(piecePosition);

        foreach (Vector2Int neighbour in neighbourPositions)
        {
            if(neighbour.x < 0 || neighbour.y < 0 || !board.HasPieceOnPosition(neighbour)) 
                continue;

            BubblePiece neighbourPiece = board.GetPiece(neighbour);
            if (matchCondition.IsMatch(neighbourPiece))
            {
                if (neighbourPiece is ISpecialPiece<BubblePiece> && !specials.Contains(neighbour))
                {
                    specials.Add(neighbour);
                }
                else if(!matches.Contains(neighbour))
                {
                    matches.Add(neighbour);
                    SearchMatches(neighbour, matchCondition, board, ref matches, ref specials);
                }
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
