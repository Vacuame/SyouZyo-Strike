using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryStatic;

public class ItemSave
{
    public List<TetrisInfo> items = new List<TetrisInfo>();
    public int bagWidth, bagHeight;
}

[System.Serializable]
public class TetrisInfo
{
    public int id;
    public Vector2Int pos;
    public Dir dir;
    public TetrisInfo(int id, Vector2Int pos,Dir dir)
    {
        this.id = id;
        this.pos = pos;
        this.dir = dir;
    }
}
