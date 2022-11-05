using System;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterBoard : AbstractBoard<BubblePiece, BubbleShooterLevelData> //This could also be generic depending on the match 3 strategy
{
    [SerializeField] private BubbleShooterLevelData abstractLevelData; //This should be removed and settled by the populate
    private void Start()
    {
        InitBoard(abstractLevelData);
    }

    private void FixBubble(BubblePiece bubblePiece)
    {
        bubblePiece.FixBubble();
        Vector3Int pos = gridComponent.WorldToCell(bubblePiece.transform.position);
        bubblePiece.transform.position = gridComponent.GetCellCenterWorld(pos);
    }

    private void ProcessMatches(BubblePiece piece, Vector2Int piecePosition)
    {
        List<BubblePiece> matches = new List<BubblePiece>();
        SearchMatches(piecePosition, piece.IsMatch, ref matches);

        if (matches.Count < 3) 
            return;
        
        foreach (BubblePiece bubble in matches)
        {
            MatchPiece(alignmentStrategy.LocalToGridPosition(bubble.transform.localPosition));
        }
    }

    protected override void SearchMatches(Vector2Int piecePosition, Func<BubblePiece, bool> matchCondition, ref List<BubblePiece> matches)
    {
        Vector2Int[] neighbourPositions = alignmentStrategy.GetNeighbourCoordinates(piecePosition);

        foreach (Vector2Int neighbour in neighbourPositions)
        {
            if(neighbour.x < 0 || neighbour.y < 0) 
                continue;

            BubblePiece neighbourPiece = GetPiece(neighbour);
            if (neighbourPiece != null && !matches.Contains(neighbourPiece) && matchCondition.Invoke(neighbourPiece))
            {
                matches.Add(neighbourPiece);
                SearchMatches(neighbour, matchCondition, ref matches);
            }
        }
    }

    protected override void OnPieceCreated(BubblePiece piece) => piece.FixBubble();
    
    public override void OnPiecePositioned(BubblePiece piece)
    {
        FixBubble(piece);
        Vector2Int piecePosition = alignmentStrategy.LocalToGridPosition(piece.transform.localPosition);
        RegisterPiece(piecePosition, piece);

        ProcessMatches(piece, piecePosition);
        UpdateComponents();
        RegisterMovement();
    }
}
