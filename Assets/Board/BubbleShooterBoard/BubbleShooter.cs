using UnityEngine;

public class BubbleShooter : AbstractBoardComponent<BubblePiece, BubbleShooterBoard>
{
    [SerializeField] private float shotSpeed;
    [SerializeField] private LineRenderer lineRenderer = null;
    private BubblePiece bubbleToShot = null;
    private float lineLength = 3f;

    public bool CanShot { get; set; } = true;

    private Vector3 GetDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction = new Vector3(direction.x, direction.y, 1);
        direction.Normalize();

        return direction;
    }

    void Update()
    {
        Vector3 direction = GetDirection();
        lineRenderer.SetPosition(1, direction * lineLength);
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!Board.HasRemainingMoves)
            {
                Debug.LogError("You are out of moves!");
                return;
            }
            
            ShotBubble(direction);
        }
    }

    private void ShotBubble(Vector3 direction)
    {
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
