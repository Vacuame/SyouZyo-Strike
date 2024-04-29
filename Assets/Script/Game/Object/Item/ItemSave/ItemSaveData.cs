using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static InventoryStatic;

public class ItemSaveData
{
    #region Items
    
    private List<ItemSave> items = new List<ItemSave>();
    public void AddItem(ItemSave save)
    {
        items.Add(save);
        save.container = this;
        itemDict.Add(save.id, save);
        if(onSpecificIDAdded.TryGetValue(save.id,out var action))
        {
            action.Invoke(save);
        }
        onItemAdded?.Invoke(save);
    }
    public void RemoveItem(ItemSave save)
    {
        items.Remove(save);
        itemDict.Remove(save.id, save);
        if (onSpecificIDRemoved.TryGetValue(save.id, out var action))
        {
            action.Invoke(save);
        }
        onItemRemoved?.Invoke(save);
    }
    public List<ItemSave> GetItems()
    {
        return items;
    }
    #endregion

    #region ItemDict
    private ListDictionary<int, ItemSave> itemDict = new ListDictionary<int, ItemSave>();
/*
    private Dictionary<int, List<ItemSave>> _itemDict = new Dictionary<int, List<ItemSave>>();
    private void AddItemToDict(ItemSave item)
    {
        if(_itemDict.ContainsKey(item.id))
        {
            _itemDict[item.id].Add(item);
        }
        else
        {
            _itemDict.Add(item.id, new List<ItemSave>() { item});
        }
    }
    private void RemoveItemToDict(ItemSave item)
    {
        if(_itemDict.TryGetValue(item.id,out var list))
        {
            list.Remove(item);
            if(list.Count<=0)
                _itemDict.Remove(item.id);
        }
    }*/
    public bool TryGetListOfItem(int id,out List<ItemSave>list)
    {
        return itemDict.TryGetList(id,out list);
        //return _itemDict.TryGetValue(id,out list);
    }
    #endregion

    public UnityAction<ItemSave> onItemRemoved;
    public UnityAction<ItemSave> onItemAdded;
    public Dictionary<int, UnityAction<ItemSave>> onSpecificIDAdded = new Dictionary<int, UnityAction<ItemSave>>();
    public Dictionary<int, UnityAction<ItemSave>> onSpecificIDRemoved=new Dictionary<int, UnityAction<ItemSave>>();

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


