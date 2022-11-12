using UnityEngine;

public class LogThrowOverlay : AbstractPieceOverlay
{
    public override void ProcessOverlay(IBoard board)
    {
        Debug.Log("This is a log overlay");
    }
}
