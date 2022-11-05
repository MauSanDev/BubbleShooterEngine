using UnityEngine;

public class BubbleShooter : AbstractBoardComponent<BubbleBehavior, BubbleShooterLevelData>
{
    [SerializeField] private float shotSpeed;
    private int currentMove = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!HasRemainingMoves)
            {
                Debug.LogError("You are out of moves!");
                return;
            }
            
            ShotBubble();
        }
    }

    private bool HasRemainingMoves => currentMove < Board.LevelData.PlayerMoves;

    private string GetNextBubbleId()
    {
        if (currentMove < Board.LevelData.PiecesStack.Count)
        {
            return Board.LevelData.PiecesStack[currentMove];
        }

        return "red"; // TODO: Return a random bubble that is on the board.
    }

    private void ShotBubble()
    {
        string bubbleId = GetNextBubbleId();
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shooterPosition = transform.position;
        Vector3 direction = mousePos - shooterPosition;
        direction.Normalize();

        //Change this positions
        BubbleBehavior instance = Instantiate(Board.PieceDatabase.GetPieceById(bubbleId), shooterPosition, Quaternion.identity);
        instance.transform.SetParent(Board.transform);

        instance.OnBubblePlaced += Board.OnPiecePositioned;
        instance.ShotBubble(direction * shotSpeed);
        currentMove++;
    }
}
