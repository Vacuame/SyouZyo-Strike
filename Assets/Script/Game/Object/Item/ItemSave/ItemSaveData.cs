using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static InventoryStatic;
using static UnityEditor.Progress;

public class ItemSaveData
{
    public List<ItemSave> items = new List<ItemSave>();
    public int bagWidth, bagHeight;

    public ItemSaveData(int width,int height,List<ItemSave> infos)
    {
        bagWidth = width;
        bagHeight = height;
        items = infos;
    }

    public bool TryGetNewItem(ItemInfo newItem)
    {
        //生成二维数组Grid
        bool[,] grid = new bool[bagWidth, bagHeight];
        
        foreach (var item in items)
        {
            Vector2Int pos = item.pos;
            ItemInfo info = ItemManager.Instance.GetItemInfo(item.id);
            int w = info.width; int h = info.height;
            if (item.dir == Dir.Left)
                Calc.Swap(ref w, ref h);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    grid[i + pos.x, j + pos.y] = true;
        }

        Vector2Int itemPos;
        if (TryFindItemPlace(grid, Dir.Down, newItem.width,newItem.height,out itemPos))
        {
            items.Add(ItemSaveFac.NewItemSave(newItem, itemPos, Dir.Down));
            return true;
        }
        if (TryFindItemPlace(grid, Dir.Left, newItem.height, newItem.width, out itemPos))
        {
            items.Add(ItemSaveFac.NewItemSave(newItem, itemPos, Dir.Left));
            return true;
        }
        return false;
    }

    private bool TryFindItemPlace(bool[,] grid,Dir dir,int itemWidth,int itemHeight,out Vector2Int resPos)
    {
        resPos = new Vector2Int();

        for (int y = bagHeight- itemHeight; y>=0;y--)
        {
            for (int x = 0; x <= bagWidth - itemWidth; x++)
            {
                if (CanItemPlaceIn(grid, itemWidth, itemHeight, x, y))
                {
                    resPos = new Vector2Int(x, y);
                    return true;
                }
            }
        }
        return false;
    }

    private bool CanItemPlaceIn(bool[,]grid, int itemWidth, int itemHeight, int x,int y)
    {
        for (int i = 0; i < itemWidth; i++)
            for(int j =0; j < itemHeight; j++)
                if (grid[i+x,j+y])
                    return false;
        return true;
    }

}

[System.Serializable]
public class ItemSave
{
    public int id;
    public Vector2Int pos;
    public Dir dir;
    public ItemSave(int id, Vector2Int pos,Dir dir)
    {
        this.id = id;
        this.pos = pos;
        this.dir = dir;
    }
}

public static class ItemSaveFac
{
    public static ItemSave NewItemSave(ItemInfo info,Vector2Int pos,Dir dir)
    {
        switch (info.type) 
        {
            case ItemInfo.ItemType.Item:
                return new ItemSave(info.id,pos,dir);
            case ItemInfo.ItemType.Useable:
                break;
            case ItemInfo.ItemType.Gun:
                GunItemInfo gunInfo = (GunItemInfo)info;
                return new GunItemSave(info.id, pos, dir, gunInfo.fullAmmo,false);
        }
        return null;
    }
}
