using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLevelData : ScriptableObject
{
    [Header("Board Info")]
    [SerializeField] public List<PieceCoordinate> coordinates;
    
    
    [Tooltip("Quantity of moves the player will have on this level.")]
    [SerializeField] private int playerMoves = 0;
    
    public int PlayerMoves => playerMoves;

    
    [Serializable]
    public class PieceCoordinate
    {
        [SerializeField] public Vector2Int coordinates;
        [SerializeField] public string pieceId;
    }
}
