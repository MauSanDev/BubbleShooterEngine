using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchStrategy<TPiece> where TPiece : AbstractPiece
{
    List<TPiece> GetMatches(Vector2Int piecePosition, Func<TPiece, bool> matchCondition, IBoard<TPiece> board);
}