using UnityEngine;

public abstract class AbstractPiece : MonoBehaviour, IPiece
{
    public abstract IMatchCondition GetMatchCondition();
}
