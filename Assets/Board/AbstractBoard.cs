using System.Collections.Generic;
using UnityEngine;

public interface IBoard<TPiece> where TPiece : AbstractPiece
{
    TPiece GetPiece(Vector2Int coordinate);
}

[RequireComponent(typeof(Grid))]
public abstract class AbstractBoard<TPiece, TLevelData> : MonoBehaviour, IBoard<TPiece> where TPiece : AbstractPiece where TLevelData : AbstractLevelData
{
    [Header("Databases")] 
    [SerializeField] protected AbstractPieceDatabase<TPiece> pieceDatabase;

    [Header("References")] 
    [SerializeField] protected Grid gridComponent = null;

    protected IAlignmentStrategy alignmentStrategy = null;

    private Dictionary<Vector2Int, TPiece> pieceInstances = new Dictionary<Vector2Int, TPiece>();

    protected abstract IMatchStrategy<TPiece> DefaultMatchStrategy { get; }

    public TLevelData LevelData { get; private set; }

    public AbstractPieceDatabase<TPiece> PieceDatabase => pieceDatabase;
    public int CurrentMove { get; private set; } = 0;
    public bool HasRemainingMoves => CurrentMove < LevelData.PlayerMoves;

    protected void RegisterMovement(int toAdd = 1) => CurrentMove += toAdd;


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

    protected abstract void SetupComponents();

    protected abstract void UpdateComponents();

    public void InitBoard(TLevelData levelData)
    {
        LevelData = levelData;
        SetupComponents();
        PopulateBoard(levelData);
    }
    
    protected void PopulateBoard(TLevelData levelData)
    {
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

    public TPiece GetPiece(Vector2Int coordinate)
    {
        if (!pieceInstances.ContainsKey(coordinate))
        {
            return null;
        }

        return pieceInstances[coordinate];
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

    public abstract void OnPiecePositioned(BubblePiece piece);
}