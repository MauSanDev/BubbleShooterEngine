using UnityEngine;

public interface IBoardComponent<TPiece, TBoard> where TPiece : AbstractPiece where TBoard : IBoard<TPiece>
{
    void SetupComponent(TBoard board);
    void UpdateComponent();

}

public abstract class AbstractBoardComponent<TPiece, TBoard> : MonoBehaviour, IBoardComponent<TPiece, TBoard> where TPiece : AbstractPiece where TBoard : IBoard<TPiece>
{
    protected TBoard Board { get; private set; }

    public void SetupComponent(TBoard board)
    {
        Board = board;
        UpdateComponent();
    }

    public abstract void UpdateComponent();
}
