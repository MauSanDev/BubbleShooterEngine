using System.Collections.Generic;
using UnityEngine;

public class PiecePoolHandler<TPiece, TLevelData> where TPiece : AbstractPiece where TLevelData : AbstractLevelData
{
    private Dictionary<string, ObjectPool<TPiece>> piecePools = new Dictionary<string, ObjectPool<TPiece>>();
    private AbstractBoard<TPiece, TLevelData> board = null;
    
    public void Initialize(AbstractBoard<TPiece, TLevelData> board)
    {
        this.board = board;
    }
    
    public TPiece GetPiece(string pieceId)
    {
        TPiece piece = GetPiecePool(pieceId).GetElement();
        piece.gameObject.SetActive(true);
        return piece;
    }

    public void ReturnPiece(TPiece piece)
    {
        piece.gameObject.SetActive(false);
        GetPiecePool(piece.AssignedID).ReturnElement(piece);
    }

    private ObjectPool<TPiece> GetPiecePool(string pieceId)
    {
        if (piecePools.ContainsKey(pieceId))
        {
            return piecePools[pieceId];
        }

        TPiece piecePrefab = board.PieceDatabase.GetPieceById(pieceId);

        if (piecePrefab == null)
        {
            Debug.LogError(
                $"{GetType()} :: The piece with id {pieceId} wasn't found.");
            return null;
        }
        
        ObjectPool<TPiece> newPool = new ObjectPool<TPiece>(piecePrefab, board.transform, 100);
        piecePools.Add(pieceId, newPool);

        return newPool;
    }
}