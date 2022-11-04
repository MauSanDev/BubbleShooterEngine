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
        int y = Mathf.RoundToInt(localPosition.y * 1.33f);
        int x = (int) (y % 2 == 0 ? localPosition.x : localPosition.x - .5f);
        return new Vector2Int(x, y);
    }

}
