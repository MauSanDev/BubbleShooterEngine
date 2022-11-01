using UnityEngine;

[RequireComponent(typeof(Grid))]
public class BoardBehavior : MonoBehaviour //This could also be generic depending on the match 3 strategy
{
    [Header("Databases")] 
    [SerializeField] private BubbleDatabase bubbleDatabase; //Could be an IPieceDatabase with generics.

    [Header("Components")] 
    [SerializeField] private Grid gridComponent;
    [SerializeField] private LevelData levelData;

    private void AlignAllChildren()
    {
        foreach (Transform child in transform)
        {
            Vector3Int pos = gridComponent.WorldToCell(child.position);
            child.position = gridComponent.GetCellCenterWorld(pos);
        }
    }

    private void Start()
    {
        PopulateBoard(levelData);
    }

    private void PopulateBoard(LevelData levelData)
    {
        foreach (var coordinate in levelData.coordinates)
        {
            BubbleBehavior bubblePrefab = bubbleDatabase.GetBubbleById(coordinate.bubbleId);

            if (bubblePrefab == null)
            {
                return;
            }
            
            BubbleBehavior bubbleInstance = Instantiate(bubblePrefab, gridComponent.transform);
            bubbleInstance.FixBubble();

            //TODO: Create a class named AlignmentStrategy depending on the grid layout
            float x = coordinate.coordinates.y % 2 == 0 ? coordinate.coordinates.x : coordinate.coordinates.x + .5f;
            float y = -(coordinate.coordinates.y * .75f);
            Vector3 position = new Vector3(x, y, 1);

            bubbleInstance.transform.localPosition = position;
        }
    }

    public void FixBubble(BubbleBehavior bubbleBehavior)
    {
        bubbleBehavior.FixBubble();
        Vector3Int pos = gridComponent.WorldToCell(bubbleBehavior.transform.position);
        bubbleBehavior.transform.position = gridComponent.GetCellCenterWorld(pos);
    }

}
