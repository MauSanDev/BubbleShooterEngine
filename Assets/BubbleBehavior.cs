using System;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour, IPiece
{
    [SerializeField] private Rigidbody2D body;
    public event Action<BubbleBehavior> OnBubblePlaced;

    private BubbleState currentState = BubbleState.OnHand;

    private enum BubbleState
    {
        OnHand,
        Thrown,
        Fixed
    }


    public void FixBubble()
    {
        currentState = BubbleState.Fixed;
        body.bodyType = RigidbodyType2D.Static;
    }

    public void ShotBubble(Vector3 shotDirection)
    {
        currentState = BubbleState.Thrown;
        body.bodyType = RigidbodyType2D.Dynamic;
        body.velocity = shotDirection;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentState == BubbleState.Thrown)
        {
            OnBubblePlaced?.Invoke(this);
        }
    }
}
