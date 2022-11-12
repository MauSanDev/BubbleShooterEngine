using UnityEngine;
using System.Collections.Generic;

public class PieceHandler<TPiece> where TPiece : AbstractPiece
{
    private Dictionary<Vector2Int, TPiece> pieceInstances = new Dictionary<Vector2Int, TPiece>();
    private SpawnRateHandler<string> pieceCounter = new SpawnRateHandler<string>();

    public TPiece GetPiece(Vector2Int coordinate)
    {
        if (!pieceInstances.ContainsKey(coordinate))
        {
            return null;
        }

        return pieceInstances[coordinate];
    }
    
    public void RegisterPiece(Vector2Int coordinate, TPiece piece)
    {
        pieceCounter.Stack(piece.AssignedID);
        pieceInstances.Add(coordinate, piece);
    }
    

    public void UnregisterPiece(Vector2Int coordinate)
    {
        string pieceId = pieceInstances[coordinate].AssignedID;
        pieceCounter.Pop(pieceId);
        
        pieceInstances.Remove(coordinate);
    }

    public bool HasPieceOnPosition(Vector2Int coordinate) => pieceInstances.ContainsKey(coordinate);

    public SpawnRateHandler<string> GetPieceSpawnHandler() => pieceCounter;
}