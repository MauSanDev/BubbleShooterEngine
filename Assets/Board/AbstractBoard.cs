using UnityEngine;

[RequireComponent(typeof(Grid))]
public abstract class AbstractBoard<TPiece> : MonoBehaviour where TPiece : MonoBehaviour, IPiece
{
    [Header("Databases")] 
    [SerializeField] protected AbstractPieceDatabase<TPiece> pieceDatabase; //Could be an IPieceDatabase with generics.

    [Header("Components")] 
    [SerializeField] protected Grid gridComponent = null;

    private IAlignmentStrategy alignmentStrategy;

    private void Awake()
    {
        DefineAlignmentStrategy();
    }

    private void DefineAlignmentStrategy()
    {
        switch (gridComponent.cellLayout)
        {
            case GridLayout.CellLayout.Hexagon:
                alignmentStrategy = new HexagonalAlignmentStrategy();
                return;
        }
    }
    
    private void AlignAllChildren()
    {
        foreach (Transform child in transform)
        {
            Vector3Int pos = gridComponent.WorldToCell(child.position);
            child.position = gridComponent.GetCellCenterWorld(pos);
        }
    }
    
    protected void PopulateBoard(LevelData levelData)
    {
        foreach (var coordinate in levelData.coordinates)
        {
            TPiece bubblePrefab = pieceDatabase.GetPieceById(coordinate.pieceId);

            if (bubblePrefab == null)
            {
                Debug.LogError($"{GetType()} :: The piece with id {coordinate.pieceId} wasn't found. - Position: {coordinate.coordinates}");
                continue;
            }
            
            TPiece pieceInstance = Instantiate(bubblePrefab, gridComponent.transform);
            pieceInstance.transform.localPosition = alignmentStrategy.GetPiecePosition(coordinate.coordinates);
            
            OnPieceCreated(pieceInstance);
        }
    }

    /// <summary>
    /// Add a behavior to the piece when it is spawned.
    /// </summary>
    /// <param name="piece"></param>
    protected abstract void OnPieceCreated(TPiece piece);
}
