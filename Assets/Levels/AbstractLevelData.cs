using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public abstract class AbstractLevelData : ScriptableObject
{
    [Header("Board Info")]
    [SerializeField] public List<PieceCoordinate> coordinates;
    
    [Serializable]
    public class PieceCoordinate
    {
        [SerializeField] public Vector2Int coordinates;
        [SerializeField] public string pieceId;
    }
}
