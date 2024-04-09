using System.Collections;
using UnityEngine;
using static InventoryStatic;

[System.Serializable]
public class ItemSave
{
    public int id;
    public Vector2Int pos;
    public Dir dir;
    public ExtraSave extra;
    public ItemSave(int id, Vector2Int pos, Dir dir, ExtraSave extra)
    {
        this.id = id;
        this.pos = pos;
        this.dir = dir;
        this.extra = extra;
    }
}