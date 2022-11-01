using UnityEngine;

public class BubbleShooterBoard : AbstractBoard<BubbleBehavior> //This could also be generic depending on the match 3 strategy
{
    [SerializeField] private LevelData levelData; //This should be removed and settled by the populate

    private void Start()
    {
        PopulateBoard(levelData);
    }

    public void FixBubble(BubbleBehavior bubbleBehavior)
    {
        bubbleBehavior.FixBubble();
        Vector3Int pos = gridComponent.WorldToCell(bubbleBehavior.transform.position);
        bubbleBehavior.transform.position = gridComponent.GetCellCenterWorld(pos);
    }

    protected override void OnPieceCreated(BubbleBehavior piece)
    {
        piece.FixBubble();
    }
}
