using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPiece : MonoBehaviour, IPiece
{
    [SerializeField] private List<AbstractPieceOverlay> overlays = new List<AbstractPieceOverlay>();

    public void ProcessAndRemoveOverlays(IBoard board)
    {
        ProcessOverlays(board);
        RemoveOverlays();
    }
    
    public void ProcessOverlays(IBoard board)
    {
        foreach (AbstractPieceOverlay overlay in overlays)
        {
            overlay.ProcessOverlay(board);
        }
    }

    public void CreateOverlay(AbstractPieceOverlay overlayPrefab)
    {
        AbstractPieceOverlay overlayInstance = Instantiate(overlayPrefab, transform);
        overlays.Add(overlayInstance);
    }

    public void RemoveOverlays()
    {
        foreach (AbstractPieceOverlay overlay in overlays)
        {
            Destroy(overlay.gameObject);
        }
        
        overlays.Clear();
    }
    
    public string AssignedID { get; private set; }

    public void AssignPieceID(string newId) => AssignedID = newId;
    
    public abstract IMatchCondition GetMatchCondition();
}
