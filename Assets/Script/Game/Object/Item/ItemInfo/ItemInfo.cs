using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    [Header("������Ϣ")]
    public int id;
    public string itemName;
    public int maxStackNum = 1;
    public ItemType type;

    [Header("������Ʒ�趨")]
    public int width;
    public int height;
    public Sprite icon;

    [Header("�ϳ��䷽")]
    public List<Pair<int, int>> compositeList = new List<Pair<int, int>>();

    public enum ItemType
    {
        Item,Useable,Gun,Knife
    }
}
