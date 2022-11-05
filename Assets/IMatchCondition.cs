/// <summary>
/// Define the conditions for a piece to be a Match candidate.
/// </summary>
/// <typeparam name="TPiece"></typeparam>
public interface IMatchCondition<TPiece> where TPiece : AbstractPiece
{
    bool IsMatch(TPiece piece);
}