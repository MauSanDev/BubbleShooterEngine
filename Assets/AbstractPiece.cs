using UnityEngine;

public abstract class AbstractPiece : MonoBehaviour, IPiece
{
    public string AssignedID { get; private set; }

    public void AssignPieceID(string newId) => AssignedID = newId;
    
    public abstract IMatchCondition GetMatchCondition();
}
