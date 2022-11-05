using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleShooterLevelData", menuName = "Bubble Shooter/Level Data")]
public class BubbleShooterLevelData : AbstractLevelData
{
    
    [Tooltip("Piece IDs of the player stack. If none or not enough, will get random bubble.")]
    [SerializeField] private List<string> piecesStack = new List<string>();

    public List<string> PiecesStack => piecesStack;
}
