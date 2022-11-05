/// <summary>
/// Define the conditions for a piece to be a Match candidate.
/// </summary>
/// <typeparam name="TPiece"></typeparam>
public abstract class AbstractMatchCondition<TPiece> : IMatchCondition<TPiece> where TPiece : AbstractPiece
{
    public abstract bool IsMatch(TPiece piece);
    
    public bool IsMatch(IPiece piece) => (this as IMatchCondition<TPiece>).IsMatch((TPiece)piece);
}

public interface IMatchCondition<TPiece> : IMatchCondition where TPiece : AbstractPiece
{
    bool IsMatch(TPiece piece);
}

public interface IMatchCondition
{
    bool IsMatch(IPiece piece);
}