using UnityEngine;

public class BubbleShooter : AbstractBoardComponent<BubblePiece, BubbleShooterBoard>
{
    [SerializeField] private float shotSpeed;
    private BubblePiece bubbleToShot = null;

    public bool CanShot { get; set; } = true;

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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction.Normalize();
        
        bubbleToShot.ShotBubble(direction * shotSpeed);

        CanShot = false;
    }

    public override void UpdateComponent()
    { 
        if(!Board.HasRemainingMoves)
            return;

        CanShot = true;
        
        string bubbleId = GetNextBubbleId();
        
        //Change this positions
        bubbleToShot = Instantiate(Board.PieceDatabase.GetPieceById(bubbleId), transform.position, Quaternion.identity);
        bubbleToShot.transform.SetParent(Board.transform);

        bubbleToShot.OnBubblePlaced += Board.OnPiecePositioned;
    }
}
