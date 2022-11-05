public class ColorBubbleCondition : AbstractMatchCondition<BubblePiece>
{
    private BubblePiece.BubbleColors colorToMatch;

    public ColorBubbleCondition(BubblePiece.BubbleColors colorToMatch)
    {
        this.colorToMatch = colorToMatch;
    }

    public override bool IsMatch(BubblePiece piece) => piece.bubbleColor == colorToMatch;
}