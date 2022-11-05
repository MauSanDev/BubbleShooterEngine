using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubbleStack : AbstractBoardComponent<BubblePiece, BubbleShooterBoard>
{
    [SerializeField] private List<SpriteRenderer> stackSprites = new List<SpriteRenderer>();
    [SerializeField] private TextMeshPro moveCounter = null;
    
    public override void UpdateComponent() //TODO: This should be cleaned
    {
        moveCounter.text = Board.RemainingMoves.ToString();
        
        for (int i = 0; i < stackSprites.Count; i++)
        {
            int stackPiece = Board.CurrentMove + i + 1;

            bool hasMovesRemaining = stackPiece < Board.LevelData.PlayerMoves;

            if (!hasMovesRemaining)
            {
                stackSprites[i].color = Color.gray;
                continue;
            }
            
            bool hasStackRemaining = Board.LevelData.PiecesStack.Count > stackPiece;

            string nextPiece = hasStackRemaining
                ? Board.LevelData.PiecesStack[stackPiece]
                : "red";
            stackSprites[i].color = Board.PieceDatabase.GetPieceById(nextPiece)
                .GetComponent<SpriteRenderer>().color;
        }
    }

}
