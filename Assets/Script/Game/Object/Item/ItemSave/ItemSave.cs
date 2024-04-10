using System.Collections;
using UnityEngine;
using static InventoryStatic;

/// <summary>
/// 背包里的物品 id可以查到物品设定的info，
/// extra是物品会变动的数据部分（比如数量）
/// <para>
/// （这里加上Serializable会和Extra有相互引用的问题）
/// </para>
/// </summary>
public class ItemSave
{
    public int id;
    public Vector2Int pos;
    public Dir dir;
    public ExtraSave extra;
    private ItemSaveData container;
    public ItemSave(int id, Vector2Int pos, Dir dir, ExtraSave extra,ItemSaveData container)
    {
        this.id = id;
        this.pos = pos;
        this.dir = dir;
        this.extra = extra;
        this.extra.itemSave = this;
        this.container = container;
    }
    public void RemoveSelf()
    {
        container?.items.Remove(this);
    }
}