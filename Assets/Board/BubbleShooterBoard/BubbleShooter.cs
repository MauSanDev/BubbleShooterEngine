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
        
        string bubbleId = Board.GetNextBubbleId();
        
        bubbleToShot = Board.CreatePiece(bubbleId, transform.position);
        bubbleToShot.OnBubblePlaced += Board.OnPiecePositioned;
    }
}
