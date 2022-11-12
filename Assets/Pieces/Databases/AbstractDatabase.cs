using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract Database for pieces needed for different Match3 boards.
/// Inherit from this class and set the IPiece behavior that should be retrieved by the Database
/// And create your ScriptableObject with all the references to your pieces.
/// Access them with the GetPieceById method.
/// </summary>
/// <typeparam name="TElement">IPiece that will be handled by the Database.</typeparam>

public abstract class AbstractDatabase<TElement> : ScriptableObject, ISerializationCallbackReceiver where TElement : MonoBehaviour
{
    [SerializeField] private List<ElementReference<TElement>> elementReferences;
    private Dictionary<string, TElement> deserializedElementsReference = new Dictionary<string, TElement>();
    
    [Serializable]
    private class ElementReference<TPrefab> where TPrefab : TElement
    {
        [SerializeField] public string id;
        [SerializeField] public TPrefab piecePrefab;
    }
    
    public TElement GetElementById(string pieceId)
    {
        if (!deserializedElementsReference.ContainsKey(pieceId))
        {
            throw new Exception($"{GetType()} :: The bubble with ID {pieceId} wasn't found.");
        }

        return deserializedElementsReference[pieceId];
    }


    #region ISerializationCallbackReceiver Methods
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        deserializedElementsReference.Clear();
        foreach (ElementReference<TElement> reference in elementReferences)
        {
            deserializedElementsReference[reference.id] = reference.piecePrefab;
        }
    }
    #endregion
}
