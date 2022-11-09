public class ColorBubbleCondition : AbstractMatchCondition<BubblePiece>
{
    private string bubbleId;

    public ColorBubbleCondition(string bubbleId)
    {
        this.bubbleId = bubbleId;
    }

    public override bool IsMatch(BubblePiece piece)
    {
        return piece.AssignedID == bubbleId || piece is ISpecialPiece<BubblePiece>;
    }
}