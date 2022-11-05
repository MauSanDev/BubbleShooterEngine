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
    public HashSet<Vector2Int> GetMatchCandidates(Vector2Int piecePosition, IMatchCondition matchCondition, IBoard<BubblePiece> board)
    {
        HashSet<Vector2Int> matches = new HashSet<Vector2Int>();
        for (int i = 0; i < 10; i++) //TODO: Change the index to the grid X size
        {
            Vector2Int position = new Vector2Int(i, piecePosition.y);

            if (board.HasPieceOnPosition(position))
            {
                matches.Add(position);
            }
        }

        return matches;
    }
}
