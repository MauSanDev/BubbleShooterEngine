using UnityEngine;

public class AbstractBoardComponent<TPiece> : MonoBehaviour where TPiece : MonoBehaviour, IPiece
{
    protected AbstractBoard<TPiece> Board { get; private set; }

    public void Setup(AbstractBoard<TPiece> board) => Board = board;
}
