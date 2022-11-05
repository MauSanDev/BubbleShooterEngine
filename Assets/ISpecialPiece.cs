public interface ISpecialPiece<TPiece> where TPiece : AbstractPiece
{
    IMatchStrategy<TPiece> GetMatchStrategy();
}