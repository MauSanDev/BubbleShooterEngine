using UnityEngine;

public abstract class AbstractBoardComponent<TPiece, TBoard> : MonoBehaviour where TPiece : AbstractPiece where TBoard : IBoard<TPiece>
{
    protected TBoard Board { get; private set; }

    public void SetupComponent(TBoard board)
    {
        Board = board;
        UpdateComponent();
    }

    public abstract void UpdateComponent();
}
