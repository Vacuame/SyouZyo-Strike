using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryStatic;

public class ItemSaveData
{
    #region Items
    private List<ItemSave> items = new List<ItemSave>();
    public void AddItem(ItemSave save)
    {
        items.Add(save);
    }
    public void RemoveItem(ItemSave save)
    {
        items.Remove(save);
    }
    public List<ItemSave> GetItems()
    {
        return items;
    }
    #endregion

    public int bagWidth, bagHeight;

    public ItemSaveData(int width,int height,List<ItemSave> infos)
    {
        bagWidth = width;
        bagHeight = height;
        items = infos;
    }

    public bool TryGetNewItem(ItemInfo itemInfo,ExtraSave extra)
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
        if (TryFindItemPlace(grid, Dir.Down, itemInfo.width,itemInfo.height,out itemPos))
        {
            AddItem(new ItemSave(itemInfo.id, itemPos, Dir.Down, extra));
            return true;
        }
        if (TryFindItemPlace(grid, Dir.Left, itemInfo.height, itemInfo.width, out itemPos))
        {
            AddItem(new ItemSave(itemInfo.id, itemPos, Dir.Left, extra));
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


