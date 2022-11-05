using UnityEngine;

public class BubbleShooterBoard : AbstractBoard<BubblePiece, BubbleShooterLevelData>
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

    private Vector2Int AttachBubbleToBoard(BubblePiece bubblePiece)
    {
        bubblePiece.FixBubble();
        Vector3Int pos = gridComponent.WorldToCell(bubblePiece.transform.position);
        bubblePiece.transform.position = gridComponent.GetCellCenterWorld(pos);
        
        Vector2Int piecePosition = alignmentStrategy.LocalToGridPosition(bubblePiece.transform.localPosition);
        RegisterPiece(piecePosition, bubblePiece);

        return piecePosition;
    }

    protected override void SetupBoardComponents()
    {
        bubbleShooter.SetupComponent(this);
        bubbleStack.SetupComponent(this);
    }

    protected override void UpdateBoardComponents()
    {
        bubbleShooter.UpdateComponent();
        bubbleStack.UpdateComponent();
    }

    protected override void OnPieceCreated(BubblePiece piece) => piece.FixBubble();
    
    public override void OnPiecePositioned(BubblePiece piece)
    {
        Vector2Int pieceCoordinate = AttachBubbleToBoard(piece);
        ProcessMatches(pieceCoordinate);
        RegisterMovement();
        UpdateBoardComponents();
    }
}
