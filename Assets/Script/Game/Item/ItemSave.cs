using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSave
{
    public List<ItemSaveData> items;
    public int width, height;
}

[System.Serializable]
public class ItemSaveData
{
    public int id;
    public Vector2Int pos;
}
