using UnityEngine;

public class BubbleShooterBoard : AbstractBoard<BubbleBehavior> //This could also be generic depending on the match 3 strategy
{
    [SerializeField] private LevelData levelData; //This should be removed and settled by the populate

    private void Start()
    {
        PopulateBoard(levelData);
    }

    private void FixBubble(BubbleBehavior bubbleBehavior)
    {
        bubbleBehavior.FixBubble();
        Vector3Int pos = gridComponent.WorldToCell(bubbleBehavior.transform.position);
        bubbleBehavior.transform.position = gridComponent.GetCellCenterWorld(pos);
    }

    public void OnPiecePositioned(BubbleBehavior bubbleBehavior)
    {
        Vector2Int piecePosition = alignmentStrategy.LocalToGridPosition(bubbleBehavior.transform.localPosition);
        RegisterPiece(piecePosition, bubbleBehavior);
        
        FixBubble(bubbleBehavior);
    }
    
    protected override void OnPieceCreated(BubbleBehavior piece)
    {
        piece.FixBubble();
    }
}
