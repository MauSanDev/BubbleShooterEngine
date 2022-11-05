using System.Collections.Generic;
using UnityEngine;

public class BubbleStack : AbstractBoardComponent<BubblePiece, BubbleShooterLevelData>
{
    [SerializeField] private List<SpriteRenderer> stackSprites = new List<SpriteRenderer>();
    
    public override void UpdateComponent() //TODO: This should be cleaned
    {
        for (int i = 0; i < stackSprites.Count; i++)
        {
            int stackPiece = Board.CurrentMove + i + 1;

            bool hasMovesRemaining = stackPiece < Board.LevelData.PlayerMoves;

            if (!hasMovesRemaining)
            {
                stackSprites[i].color = Color.gray;
                return;
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
