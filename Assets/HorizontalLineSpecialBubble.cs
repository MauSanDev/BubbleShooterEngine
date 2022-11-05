public class HorizontalLineSpecialBubble : AbstractSpecialBubblePiece
{
    public override IMatchStrategy<BubblePiece> GetMatchStrategy() => new HorizontalLineMatchStrategy();
}
