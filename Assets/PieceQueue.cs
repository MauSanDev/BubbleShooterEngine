using System;
using System.Collections.Generic;
using UnityEngine;

public struct StackedPieceData
{
    public static StackedPieceData CreateStatic(string pieceId)
    {
        return new StackedPieceData()
        {
            pieceId = pieceId,
            modifiable = false
        };
    }

    public static StackedPieceData CreateModifiable(string pieceId = null)
    {
        return new StackedPieceData()
        {
            pieceId = null,
            modifiable = true
        };
    }
            
    public string pieceId;
    public bool modifiable;

    public void AssignId(string newCoso) => pieceId = newCoso;
}

public class PieceQueue
{
    private StackedPieceData[] calculatedQueue;
    
    public void Setup(List<string> data, int availableMoves)
    {
        calculatedQueue = new StackedPieceData[availableMoves];
        
        for (int i = 0; i < availableMoves; i++)
        {
            StackedPieceData pieceData = i < data.Count
                ? StackedPieceData.CreateStatic(data[i])
                : StackedPieceData.CreateModifiable();
            
            calculatedQueue[i] = pieceData;
        }
    }

    public void Recalculate(int currentMove, ref Dictionary<string, int> pieceCount, int piecesToRecalculate = 3)
    {
        currentMove++;
        List<string> availablePieces = new List<string>();
        foreach (var VARIABLE in pieceCount.Keys)
        {
            if (pieceCount[VARIABLE] > 0)
            {
                availablePieces.Add(VARIABLE);
            }
        }

        int toModify = Mathf.Min(currentMove + piecesToRecalculate, calculatedQueue.Length);
            
        for (int i = currentMove; i < toModify; i++)
        {
            bool isModifiable = calculatedQueue[i].modifiable;
            bool canBeReasigned = calculatedQueue[i].pieceId == null || !availablePieces.Contains(calculatedQueue[i].pieceId);
            
            if (isModifiable && canBeReasigned)
            {
                string toAssign = availablePieces[UnityEngine.Random.Range(0, availablePieces.Count)];
                calculatedQueue[i].AssignId(toAssign);
            }
        }
    }

    public string GetPieceForMovement(int movement)
    {
        if (movement >= calculatedQueue.Length || movement < 0)
        {
            throw new Exception($"{GetType()} :: The stack doesn't contains an id for the movement {movement}");
        }
        return calculatedQueue[movement].pieceId;
    }
}