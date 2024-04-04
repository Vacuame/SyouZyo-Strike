using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class InventoryStatic
{
    public enum Dir
    {
        Down,
        Left,
        Up,
        Right,
    }

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Down;
            case Dir.Up: return Dir.Left;
            case Dir.Right: return Dir.Down;
        }
    }
    public static float GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 270;
            case Dir.Up: return 180;
            case Dir.Right: return 90;
        }
    }
    public static Vector2 GetRotationOffset(float width, float height,Dir dir)
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
    public static Vector2 GetRotationOffset(ItemInfo info, Dir dir)
    {
        return GetRotationOffset(info.width, info.height,dir);
    }
    public static List<Vector2Int> GetGridPositionList(Vector2Int leftDownPos, Dir dir, float width, float height)
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
    public static List<Vector2Int> GetGridPositionList(Vector2Int leftDownPos, Dir dir, ItemInfo info)
    {
        return GetGridPositionList(leftDownPos, dir, info.width,info.height);
    }



}
