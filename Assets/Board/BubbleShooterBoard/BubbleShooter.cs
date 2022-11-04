
using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    [SerializeField] private BubbleShooterBoard bubbleShooterBoard;
    [SerializeField] private BubbleBehavior bubblePrefab;
    [SerializeField] private float shotSpeed;

    // Update is called once per frame
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
        BubbleBehavior instance = Instantiate(bubblePrefab, shooterPosition, Quaternion.identity);
        instance.transform.SetParent(bubbleShooterBoard.transform);

        instance.OnBubblePlaced += bubbleShooterBoard.OnPiecePositioned;
        instance.ShotBubble(direction * shotSpeed);
    }
}
