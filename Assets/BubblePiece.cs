using System;
using TMPro;
using UnityEngine;

public class BubblePiece : AbstractPiece
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private TextMeshPro label;
    public event Action<BubblePiece> OnBubblePlaced;

    private BubbleState currentState = BubbleState.OnHand;
    private enum BubbleState
    {
        OnHand,
        Thrown,
        Fixed
    }

    private void SetLabel() => label.text = AssignedID;

    public override IMatchCondition GetMatchCondition() => new ColorBubbleCondition(AssignedID);

    public void FixBubble()
    {
        SetLabel();
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
