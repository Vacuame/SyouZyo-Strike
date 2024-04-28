using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    [Header("基本信息")]
    public int id;
    public string itemName;
    public int maxStackNum = 1;
    public ItemType type;

    [Header("背包物品设定")]
    public int width;
    public int height;
    public Sprite icon;

    [Header("合成配方")]
    public List<Pair<int, int>> compositeList = new List<Pair<int, int>>();

    public enum ItemType
    {
        Item,Useable,Gun,Knife
    }
}
