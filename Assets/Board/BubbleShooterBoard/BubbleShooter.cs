using UnityEngine;

public class BubbleShooter : AbstractBoardComponent<BubbleBehavior, BubbleShooterLevelData>
{
    [SerializeField] private float shotSpeed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!Board.HasRemainingMoves)
            {
                Debug.LogError("You are out of moves!");
                return;
            }
            
            ShotBubble();
        }
    }


    private string GetNextBubbleId()
    {
        if (Board.CurrentMove < Board.LevelData.PiecesStack.Count)
        {
            return Board.LevelData.PiecesStack[Board.CurrentMove];
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
    }

    public override void UpdateComponent() { }
}
