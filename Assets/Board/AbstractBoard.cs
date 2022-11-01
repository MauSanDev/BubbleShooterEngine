using UnityEngine;

[RequireComponent(typeof(Grid))]
public abstract class AbstractBoard<TPiece> : MonoBehaviour where TPiece : MonoBehaviour, IPiece
{
    [Header("Databases")] 
    [SerializeField] protected AbstractPieceDatabase<TPiece> pieceDatabase; //Could be an IPieceDatabase with generics.

    [Header("Components")] 
    [SerializeField] protected Grid gridComponent = null;
    
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
            TPiece bubblePrefab = pieceDatabase.GetPieceById(coordinate.bubbleId);

            if (bubblePrefab == null)
            {
                Debug.LogError($"{GetType()} :: The piece with id {coordinate.bubbleId} wasn't found. - Position: {coordinate.coordinates}");
                return;
            }
            
            TPiece pieceInstance = Instantiate(bubblePrefab, gridComponent.transform);

            //TODO: Create a class named AlignmentStrategy depending on the grid layout
            float x = coordinate.coordinates.y % 2 == 0 ? coordinate.coordinates.x : coordinate.coordinates.x + .5f;
            float y = -(coordinate.coordinates.y * .75f);
            Vector3 position = new Vector3(x, y, 1);

            pieceInstance.transform.localPosition = position;
            
            OnPieceCreated(pieceInstance);
        }
    }

    /// <summary>
    /// Add a behavior to the piece when it is spawned.
    /// </summary>
    /// <param name="piece"></param>
    protected abstract void OnPieceCreated(TPiece piece);
}
