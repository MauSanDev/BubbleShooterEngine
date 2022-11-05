using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define the search strategy to retrieve the match candidates.
/// </summary>
/// <typeparam name="TPiece"></typeparam>
public interface IMatchStrategy<TPiece> where TPiece : AbstractPiece
{
    List<TPiece> GetMatchCandidates(Vector2Int piecePosition, IMatchCondition<TPiece> matchCondition, IBoard<TPiece> board);
}