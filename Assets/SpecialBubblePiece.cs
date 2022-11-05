using System.Collections.Generic;
using UnityEngine;

public class SpecialBubblePiece : BubblePiece, ISpecialPiece<BubblePiece>
{
    public override IMatchCondition GetMatchCondition() => new SpecialBubbleMatchCondition();

    public IMatchStrategy<BubblePiece> GetMatchStrategy() => new LineSpecialBubbleMatchStrategy();
}

public class SpecialBubbleMatchCondition : AbstractMatchCondition<BubblePiece>
{
    public override bool IsMatch(BubblePiece piece) => true;
}

public interface ISpecialPiece<TPiece> where TPiece : AbstractPiece
{
    IMatchStrategy<TPiece> GetMatchStrategy();
}

public class LineSpecialBubbleMatchStrategy : IMatchStrategy<BubblePiece>
{
    public List<Vector2Int> GetMatchCandidates(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board)
    {
        List<Vector2Int> matches = new List<Vector2Int>();
        matches.Add(piecePosition);
        for (int i = 0; i < 10; i++)
        {
            Vector2Int position = new Vector2Int(i, piecePosition.y);
            if (board.IsPieceOnPosition(position))
            {
                matches.Add(position);
            }
        }

        return matches;
    }
}
