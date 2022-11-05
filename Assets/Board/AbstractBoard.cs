using System.Collections.Generic;
using UnityEngine;

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
    public AbstractPieceDatabase<TPiece> PieceDatabase => pieceDatabase;
    public int RemainingMoves => LevelData.PlayerMoves - CurrentMove;
    public int CurrentMove { get; private set; } = 0;
    public bool HasRemainingMoves => RemainingMoves > 0;
    public TLevelData LevelData { get; private set; }

    
    /// <summary>
    /// Setup the needed components when the board is Init.
    /// </summary>
    protected abstract void SetupBoardComponents();
    
    /// <summary>
    /// Update all the Components when needed.
    /// </summary>
    protected abstract void UpdateBoardComponents();
    

    /// <summary>
    /// Add a behavior to the piece when it is spawned on the Board creation.
    /// </summary>
    /// <param name="piece"></param>
    protected abstract void OnPieceCreated(TPiece piece);

    /// <summary>
    /// Execute this callback when a new piece is positioned on the board.
    /// </summary>
    /// <param name="piece"></param>
    public abstract void OnPiecePositioned(BubblePiece piece);


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
    
    public void InitBoard(TLevelData levelData)
    {
        LevelData = levelData;
        SetupBoardComponents();
        PopulateBoard(levelData);
    }
    
    private void PopulateBoard(TLevelData levelData)
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
    
    protected void RegisterMovement(int toAdd = 1) => CurrentMove += toAdd;

    public TPiece GetPiece(Vector2Int coordinate)
    {
        if (!pieceInstances.ContainsKey(coordinate))
        {
            return null;
        }

        return pieceInstances[coordinate];
    }

    public bool HasPieceOnPosition(Vector2Int coordinate) => pieceInstances.ContainsKey(coordinate);
    
    /// <summary>
    /// Analyze the board for possible matches.
    /// </summary>
    protected void ProcessMatches(Vector2Int piecePosition)
    {
        TPiece pieceToProcess = GetPiece(piecePosition);
        IMatchCondition matchCondition = pieceToProcess.GetMatchCondition();
        
        IMatchStrategy<TPiece> matchStrategy = null;

        if (pieceToProcess is ISpecialPiece<TPiece> specialPiece)
        {
            matchStrategy = specialPiece.GetMatchStrategy();
        }
        else
        {
            matchStrategy = DefaultMatchStrategy;
        }


        List<Vector2Int> matchCandidates = matchStrategy.GetMatchCandidates(piecePosition, matchCondition, this);

        foreach (Vector2Int matchCandidate in matchCandidates)
        {
            if(!HasPieceOnPosition(matchCandidate))
                continue;
            
            TPiece matchPiece = GetPiece(matchCandidate);
            bool isTheSamePiece = piecePosition == matchCandidate;
            if (!isTheSamePiece && matchPiece is ISpecialPiece<TPiece>)
            {
                ProcessMatches(matchCandidate);
            }

            MatchPiece(matchCandidate);
        }
    }

    private bool MatchPiece(Vector2Int coordinate)
    {
        if (!pieceInstances.ContainsKey(coordinate))
        {
            return false;
        }
        
        TPiece instance = pieceInstances[coordinate];
        
        //TODO: Add a pool
        pieceInstances.Remove(coordinate);
        Destroy(instance.gameObject);
        return true;
    }
}


public interface IBoard<TPiece> where TPiece : AbstractPiece
{
    TPiece GetPiece(Vector2Int coordinate);
    bool HasPieceOnPosition(Vector2Int coordinate);
}