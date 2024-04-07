using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryStatic;

public class ItemSave
{
    public List<TetrisInfo> items = new List<TetrisInfo>();
    public int bagWidth, bagHeight;


    //动态规划
    public void TryFindItemPlace(ItemInfo newItem)
    {
        //生成二维数组Grid
        bool[,] grid = new bool[bagWidth, bagHeight];
        foreach (var item in items) 
        {
            Vector2Int itemPos = item.pos;
            ItemInfo info = ItemManager.Instance.GetItemInfo(item.id);
            for(int i=0;i<info.width;i++)
                for(int j=0;j<info.height;j++)
                    grid[i+itemPos.x,j+itemPos.y] = true;
        }

        List<int> numToBeCheckedPerRow = new List<int>();
        int rowNum = newItem.height;
        for(int i=0;i < rowNum;i++)
            numToBeCheckedPerRow.Add(newItem.width);
        Vector2Int newPos = Vector2Int.zero;

    }
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
