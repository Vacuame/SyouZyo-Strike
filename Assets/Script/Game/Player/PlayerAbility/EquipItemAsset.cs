using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "EquipData", menuName = "ABS/Ability/Equip")]
public class EquipItemAsset : AbilityAsset
{
    [Header("动态绑定")]
    [HideInInspector] public PlayerCharacter character;
    [HideInInspector] public Rig chestRig;
    public void Bind(PlayerCharacter character, Rig chestRig)
    {
        this.chestRig = chestRig;
        this.character = character;
    }
}