public abstract class AbstractSpecialBubblePiece : BubblePiece, ISpecialPiece<BubblePiece>
{
    public override IMatchCondition GetMatchCondition() => new SpecialBubbleMatchCondition();
    public abstract IMatchStrategy<BubblePiece> GetMatchStrategy();
}