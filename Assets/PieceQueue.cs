using System;
using System.Collections.Generic;
using UnityEngine;

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

    public void Recalculate(int currentMove, SpawnRateHandler<string> availablePieces, int piecesToRecalculate = 3)
    {
        currentMove++;

        int toModify = Mathf.Min(currentMove + piecesToRecalculate, calculatedQueue.Length);
            
        for (int i = currentMove; i < toModify; i++)
        {
            if (calculatedQueue[i].IsModifiable && !availablePieces.Contains(calculatedQueue[i].PieceID))
            {
                string toAssign = availablePieces.GetRandom();
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
        return calculatedQueue[movement].PieceID;
    }
    
    public struct StackedPieceData
    {
        private string pieceId;
        private bool modifiable;
        
        public string PieceID => pieceId;
        public bool IsModifiable => modifiable && !string.IsNullOrEmpty(pieceId);
        
        public static StackedPieceData CreateStatic(string pieceId)
        {
            return new StackedPieceData()
            {
                pieceId = pieceId,
                modifiable = false
            };
        }

        public static StackedPieceData CreateModifiable()
        {
            return new StackedPieceData()
            {
                pieceId = null,
                modifiable = true
            };
        }

        public void AssignId(string newCoso) => pieceId = newCoso;
    }
}