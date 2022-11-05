public class BombSpecialBubble : AbstractSpecialBubblePiece
{
    public override IMatchStrategy<BubblePiece> GetMatchStrategy() => new BombMatchStrategy();
}
