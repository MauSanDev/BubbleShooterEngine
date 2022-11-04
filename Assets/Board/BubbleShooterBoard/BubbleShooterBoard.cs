using System;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterBoard : AbstractBoard<BubbleBehavior> //This could also be generic depending on the match 3 strategy
{
    [SerializeField] private AbstractLevelData abstractLevelData; //This should be removed and settled by the populate

    private void Start()
    {
        PopulateBoard(abstractLevelData);
    }

    private void FixBubble(BubbleBehavior bubbleBehavior)
    {
        bubbleBehavior.FixBubble();
        Vector3Int pos = gridComponent.WorldToCell(bubbleBehavior.transform.position);
        bubbleBehavior.transform.position = gridComponent.GetCellCenterWorld(pos);
    }

    private void ProcessMatches(BubbleBehavior piece, Vector2Int piecePosition)
    {
        List<BubbleBehavior> matches = new List<BubbleBehavior>();
        SearchMatches(piecePosition, piece.IsMatch, ref matches);

        if (matches.Count >= 3)
        {
            foreach (BubbleBehavior bubble in matches)
            {
                MatchPiece(alignmentStrategy.LocalToGridPosition(bubble.transform.localPosition));
            }
        }
    }

    protected override void SearchMatches(Vector2Int piecePosition, Func<BubbleBehavior, bool> matchCondition, ref List<BubbleBehavior> matches)
    {
        Vector2Int[] neighbourPositions = alignmentStrategy.GetNeighbourCoordinates(piecePosition);

        foreach (Vector2Int neighbour in neighbourPositions)
        {
            if(neighbour.x < 0 || neighbour.y < 0) continue;

            BubbleBehavior neighbourPiece = GetPiece(neighbour);
            if (neighbourPiece != null && !matches.Contains(neighbourPiece) && matchCondition.Invoke(neighbourPiece))
            {
                matches.Add(neighbourPiece);
                SearchMatches(neighbour, matchCondition, ref matches);
            }
        }
    }

    protected override void OnPieceCreated(BubbleBehavior piece)
    {
        piece.FixBubble();
    }
    
    public override void OnPiecePositioned(BubbleBehavior piece)
    {
        FixBubble(piece);
        Vector2Int piecePosition = alignmentStrategy.LocalToGridPosition(piece.transform.localPosition);
        RegisterPiece(piecePosition, piece);

        ProcessMatches(piece, piecePosition);
    }
}
