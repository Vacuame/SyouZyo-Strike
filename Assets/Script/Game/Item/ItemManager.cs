using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private ItemInfoSetting itemInfoSetting;
    public ItemManager()
    {
        itemInfoSetting = Resources.Load<ItemInfoSetting>("ScriptObjectData/Item/ItemSetting");
    }

    public ItemInfo GetItemInfo(int id)
    {
        return itemInfoSetting.setting.Get(id);
    }

}
