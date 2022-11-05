using UnityEngine;

public abstract class AbstractBoardComponent<TPiece, TLevelData> : MonoBehaviour where TPiece : AbstractPiece where TLevelData : AbstractLevelData
{
    protected AbstractBoard<TPiece, TLevelData> Board { get; private set; }

    public void Setup(AbstractBoard<TPiece, TLevelData> board)
    {
        Board = board;
        UpdateComponent();
    }

    public abstract void UpdateComponent();
}
