using UnityEngine;

public class HexagonalAlignmentStrategy : IAlignmentStrategy
{
    public Vector3 GridToLocalPosition(Vector2Int gridPosition)
    {
        float x = gridPosition.y % 2 == 0 ? gridPosition.x : gridPosition.x + .5f;
        float y = -(gridPosition.y * .75f);
        return new Vector3(x, y, 1);
    }
    
    public Vector2Int LocalToGridPosition(Vector3 localPosition)
    {
        int y = Mathf.Abs(Mathf.RoundToInt(localPosition.y * 1.33f));
        int x = Mathf.RoundToInt(y % 2 == 0 ? localPosition.x : localPosition.x - .5f);
        return new Vector2Int(x, y);
    }

    public Vector2Int[] GetNeighbourCoordinates(Vector2Int cellPosition)
    {
        int diff = cellPosition.y % 2 == 0 ? -1 : 1;
        Vector2Int[] neighbours =
        {
            new Vector2Int(cellPosition.x - 1, cellPosition.y), //left
            new Vector2Int(cellPosition.x + 1, cellPosition.y), //right
            new Vector2Int(cellPosition.x, cellPosition.y - 1), //top left
            new Vector2Int(cellPosition.x + diff , cellPosition.y - 1), //top right
            new Vector2Int(cellPosition.x, cellPosition.y + 1), //bottom left
            new Vector2Int(cellPosition.x + diff, cellPosition.y + 1)// bottom right
        };

        return neighbours;
    }

}
