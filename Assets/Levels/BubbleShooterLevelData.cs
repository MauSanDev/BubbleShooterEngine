using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleShooterLevelData", menuName = "Bubble Shooter/Level Data")]
public class BubbleShooterLevelData : AbstractLevelData
{
    [Tooltip("Quantity of moves the player will have on this level.")]
    [SerializeField] private int playerMoves = 0;
    
    [Tooltip("Piece IDs of the player stack. If none or not enough, will get random bubble.")]
    [SerializeField] private List<string> piecesStack = new List<string>();

    public int PlayerMoves => playerMoves;
    public List<string> PiecesStack => piecesStack;
}
