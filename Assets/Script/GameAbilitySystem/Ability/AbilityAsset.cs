using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ABS/Ability/Basic")]
public class AbilityAsset : ScriptableObject
{
    [Header("基本设置")]
    public string abilityName;

    public string[] assetTags;//能力本身的tag
    public string[] cancelAbilityTags;//如果我运行会取消哪些能力
    public string[] blockAbilityTags;//我会阻塞哪些能力（让他们不能启动）
    public string[] activationOwnedTag;//能力启动时把这些加给角色
    public string[] activationRequiredTags;//需要角色有这些Tag才能启用能力
    public string[] activationBlockedTags;//会被角色的这些Tag挡住，不能启用
}
