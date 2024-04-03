using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventorySet", menuName = "Item/Inventory")]
public class InventoryItem_SO : ScriptableObject
{
    public enum Dir
    {
        Down,
        Left,
        Up,
        Right,
    }

    public string itemName;
    public Transform prefab;
    public int width;
    public int height;

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }
    public Vector2 GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up: return new Vector2(width, height);
            case Dir.Left:
            case Dir.Right: return new Vector2(height, width);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int leftDownPos, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(leftDownPos + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        gridPositionList.Add(leftDownPos + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }
}
