using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public abstract class AbstractBoard<TPiece, TLevelData> : MonoBehaviour, IBoard<TPiece> where TPiece : AbstractPiece where TLevelData : AbstractLevelData
{
    [Header("Databases")] 
    [SerializeField] protected AbstractDatabase<TPiece> database;
    [SerializeField] protected OverlayDatabase overlayDatabase;

    [Header("References")] 
    [SerializeField] protected Grid gridComponent = null;
    
    protected IAlignmentStrategy alignmentStrategy = null;

    protected PieceHandler<TPiece> pieceHandler = new PieceHandler<TPiece>();
    private PiecePoolHandler<TPiece, TLevelData> piecePoolHandler = new PiecePoolHandler<TPiece, TLevelData>();

    protected abstract IMatchStrategy<TPiece> DefaultMatchStrategy { get; }
    public AbstractDatabase<TPiece> Database => database;
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
        piecePoolHandler.Initialize(this);
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
            TPiece pieceInstance = CreatePiece(coordinate.pieceId, gridComponent.transform.position);
            pieceInstance.transform.localPosition = alignmentStrategy.GridToLocalPosition(coordinate.coordinates);
            pieceHandler.RegisterPiece(coordinate.coordinates, pieceInstance);

            foreach (string overlayId in coordinate.overlayIds)
            {
                pieceInstance.CreateOverlay(overlayDatabase.GetElementById(overlayId));
            }
            
            OnPieceCreated(pieceInstance);
        }
    }


    public TPiece CreatePiece(string pieceId, Vector3 spawnPosition)
    {
        TPiece pieceInstance = piecePoolHandler.GetPiece(pieceId);
        pieceInstance.transform.position = spawnPosition;
        pieceInstance.AssignPieceID(pieceId);
        return pieceInstance;
    }
    protected void RegisterMovement(int toAdd = 1) => CurrentMove += toAdd;

    public TPiece GetPiece(Vector2Int coordinate) => pieceHandler.GetPiece(coordinate);

    public bool HasPieceOnPosition(Vector2Int coordinate) => pieceHandler.HasPieceOnPosition(coordinate);
    
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


        HashSet<Vector2Int> matchCandidates = matchStrategy.GetMatchCandidates(piecePosition, matchCondition, this);

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
        if (!pieceHandler.HasPieceOnPosition(coordinate))
        {
            return false;
        }
        
        TPiece matchedPiece = pieceHandler.GetPiece(coordinate);
        matchedPiece.ProcessAndRemoveOverlays(this);
        pieceHandler.UnregisterPiece(coordinate);
        piecePoolHandler.ReturnPiece(matchedPiece);
        
        return true;
    }
}

public interface IBoard
{
    
}


public interface IBoard<TPiece> : IBoard where TPiece : AbstractPiece
{
    TPiece GetPiece(Vector2Int coordinate);
    bool HasPieceOnPosition(Vector2Int coordinate);
}