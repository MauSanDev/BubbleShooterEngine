using UnityEngine;

public class BubbleShooter : AbstractBoardComponent<BubbleBehavior>
{
    [SerializeField] private float shotSpeed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBubble();
        }
    }

    private void SpawnBubble()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shooterPosition = transform.position;
        Vector3 direction = mousePos - shooterPosition;
        direction.Normalize();

        //Change this positions
        BubbleBehavior instance = Instantiate(Board.PieceDatabase.GetPieceById("green"), shooterPosition, Quaternion.identity);
        instance.transform.SetParent(Board.transform);

        instance.OnBubblePlaced += Board.OnPiecePositioned;
        instance.ShotBubble(direction * shotSpeed);
    }
}
