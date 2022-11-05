using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterBoard : AbstractBoard<BubblePiece, BubbleShooterLevelData> //This could also be generic depending on the match 3 strategy
{
    [SerializeField] private BubbleShooterLevelData abstractLevelData; //This should be removed and settled by the populate

    [Header("Game Components")]
    [SerializeField] private BubbleShooter bubbleShooter = null;
    [SerializeField] private BubbleStack bubbleStack = null;
    
    protected override IMatchStrategy<BubblePiece> DefaultMatchStrategy { get; } = new BubbleShooterDefaultMatchStrategy();

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
        ColorBubbleCondition matchCondition = piece.GetMatchCondition();
        List<BubblePiece> matches = DefaultMatchStrategy.GetMatchCandidates(piecePosition, matchCondition, this);

        if (matches.Count < 3) 
            return;
        
        foreach (BubblePiece bubble in matches)
        {
            MatchPiece(alignmentStrategy.LocalToGridPosition(bubble.transform.localPosition));
        }
    }

    protected override void SetupComponents()
    {
        bubbleShooter.SetupComponent(this);
        bubbleStack.SetupComponent(this);
    }

    protected override void UpdateComponents()
    {
        bubbleShooter.UpdateComponent();
        bubbleStack.UpdateComponent();
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
