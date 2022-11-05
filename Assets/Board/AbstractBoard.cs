using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public abstract class AbstractBoard<TPiece, TLevelData> : MonoBehaviour, IBoard<TPiece> where TPiece : MonoBehaviour, IPiece where TLevelData : AbstractLevelData
{
    [Header("Databases")] 
    [SerializeField] protected AbstractPieceDatabase<TPiece> pieceDatabase;

    [Header("Components")] 
    [SerializeField] protected Grid gridComponent = null;
    [SerializeField] protected List<AbstractBoardComponent<TPiece, TLevelData>> boardComponents = new List<AbstractBoardComponent<TPiece, TLevelData>>();

    protected IAlignmentStrategy alignmentStrategy = null;

    private Dictionary<Vector2Int, TPiece> pieceInstances = new Dictionary<Vector2Int, TPiece>();
    
    public TLevelData LevelData { get; private set; }

    public AbstractPieceDatabase<TPiece> PieceDatabase => pieceDatabase;

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
    
    protected void PopulateBoard(TLevelData levelData)
    {
        LevelData = levelData;
        
        foreach (var VARIABLE in boardComponents)
        {
            VARIABLE.Setup(this);
        }
        
        foreach (var coordinate in levelData.coordinates)
        {
            TPiece piecePrefab = pieceDatabase.GetPieceById(coordinate.pieceId);

            if (piecePrefab == null)
            {
                Debug.LogError(
                    $"{GetType()} :: The piece with id {coordinate.pieceId} wasn't found. - Position: {coordinate.coordinates}");
                continue;
            }

            TPiece pieceInstance = Instantiate(piecePrefab, gridComponent.transform);

            pieceInstance.transform.localPosition = alignmentStrategy.GridToLocalPosition(coordinate.coordinates);
            RegisterPiece(coordinate.coordinates, pieceInstance);

            OnPieceCreated(pieceInstance);
        }
    }

    protected void RegisterPiece(Vector2Int coordinate, TPiece piece)
    {
        pieceInstances.Add(coordinate, piece);
    }

    protected TPiece GetPiece(Vector2Int coordinate)
    {
        if (!pieceInstances.ContainsKey(coordinate))
        {
            return null;
        }

        return pieceInstances[coordinate];
    }
    

    protected List<TPiece> GetNeighboursFromCoordinates(Vector2Int coordinate)
    {
        List<TPiece> toReturn = new List<TPiece>();
        foreach (Vector2Int neighbourCoordinate in alignmentStrategy.GetNeighbourCoordinates(coordinate))
        {
            TPiece neighbour = GetPiece(neighbourCoordinate);
            if (neighbour != null)
            {
                toReturn.Add(neighbour);
            }
        }

        return toReturn;
    }

    protected bool MatchPiece(Vector2Int coordinate)
    {
        if (!pieceInstances.ContainsKey(coordinate))
        {
            return false;
        }
        
        TPiece instance = pieceInstances[coordinate];
        pieceInstances.Remove(coordinate);
        Destroy(instance.gameObject);
        return true;
    }

    /// <summary>
    /// Add a behavior to the piece when it is spawned.
    /// </summary>
    /// <param name="piece"></param>
    protected abstract void OnPieceCreated(TPiece piece);

    public abstract void OnPiecePositioned(BubbleBehavior piece);

    protected abstract void SearchMatches(Vector2Int piecePosition, Func<BubbleBehavior, bool> matchCondition, ref List<BubbleBehavior> matches);
}

public interface IBoard<TPiece> where TPiece : MonoBehaviour, IPiece {}