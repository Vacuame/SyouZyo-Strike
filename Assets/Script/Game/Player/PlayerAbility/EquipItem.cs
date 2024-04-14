using UnityEngine;

public class EquipItem : AbstractAbility
{
    public EquipItem(AbilityAsset setAsset, params object[] binds) : base(setAsset,binds)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new EquipItemSpec(this,owner);
    }

    public class EquipItemSpec : TimeLineAbilitySpec
    {
        //EquipItem equip;
        //EquipItemAsset equipAsset => equip.AbilityAsset;
        PlayerCharacter character;
        //Rig chestRig;

        EquipedItem equipedItem;
        ItemSave itemSave;
        //ItemInfo itemInfo;

        public EquipItemSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            //equip = ability as EquipItem;
            character = ability.binds[0] as PlayerCharacter;
        }

        /// <param name="args">
        /// ItemInfo ItemSave
        /// </param>
        public override void ActivateAbility(params object[] args)
        {
            ItemInfo newItemInfo = args[0] as ItemInfo;
            ItemSave newItemSave = args[1] as ItemSave;

            if (itemSave != null && itemSave != newItemSave)
                UnEquipItem();

            if (newItemSave != itemSave)
                EquipItem(newItemInfo, newItemSave);
            else
                UnEquipItem();

            EndSelf();
        }

        public override void EndAbility()
        {
            
        }

        private void EquipItem(ItemInfo info,ItemSave save)
        {
            itemSave = save;
            equipedItem = GameObject.Instantiate(((EquipedItemInfo)info).equipedItemPrefab,character.RightHandTransform);
            equipedItem.TakeOut(character,save.extra);

        }
        private void UnEquipItem()
        {
            equipedItem.PutIn();
            itemSave =null;
        }


        public override void InitTimeLine()
        {
            
        }
    }

}
