using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract Database for pieces needed for different Match3 boards.
/// Inherit from this class and set the IPiece behavior that should be retrieved by the Database
/// And create your ScriptableObject with all the references to your pieces.
/// Access them with the GetPieceById method.
/// </summary>
/// <typeparam name="TPiece">IPiece that will be handled by the Database.</typeparam>

public abstract class AbstractPieceDatabase<TPiece> : ScriptableObject, ISerializationCallbackReceiver where TPiece : IPiece
{
    [SerializeField] private List<PieceReference<TPiece>> pieceReferences;
    private Dictionary<string, TPiece> deserializedPieceReferences = new Dictionary<string, TPiece>();
    
    [Serializable]
    private class PieceReference<TPrefab> where TPrefab : TPiece
    {
        [SerializeField] public string id;
        [SerializeField] public TPrefab piecePrefab;
    }
    
    public TPiece GetPieceById(string pieceId)
    {
        if (!deserializedPieceReferences.ContainsKey(pieceId))
        {
            throw new Exception($"{GetType()} :: The bubble with ID {pieceId} wasn't found.");
        }

        return deserializedPieceReferences[pieceId];
    }


    #region ISerializationCallbackReceiver Methods
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        deserializedPieceReferences.Clear();
        foreach (PieceReference<TPiece> reference in pieceReferences)
        {
            deserializedPieceReferences.Add(reference.id, reference.piecePrefab);
        }
    }
    #endregion
}
