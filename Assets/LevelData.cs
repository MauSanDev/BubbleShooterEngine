using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Board Info")]
    [SerializeField] public List<BubbleCoordinate> coordinates;
    
    [Serializable]
    public class BubbleCoordinate
    {
        [SerializeField] public Vector2Int coordinates;
        [SerializeField] public string bubbleId;
    }
}
