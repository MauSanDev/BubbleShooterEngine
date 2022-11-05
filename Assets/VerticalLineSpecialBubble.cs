public class VerticalLineSpecialBubble : AbstractSpecialBubblePiece
{
    public override IMatchStrategy<BubblePiece> GetMatchStrategy() => new VerticalLineMatchStrategy();
}
