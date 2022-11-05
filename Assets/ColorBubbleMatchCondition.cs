public struct ColorBubbleCondition : IMatchCondition<BubblePiece>
{
    private BubblePiece.BubbleColors colorToMatch;

    public ColorBubbleCondition(BubblePiece.BubbleColors colorToMatch)
    {
        this.colorToMatch = colorToMatch;
    }

    public bool IsMatch(BubblePiece piece) => piece.bubbleColor == colorToMatch;
}