using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FightAsset", menuName = "ABS/Ability/Fight")]
public class RandomFightAsset : AbilityAsset
{
    public List<FightConfig> fightActions;
    public string animSpeedParamName;
    public LayerMask atkMask;
    public float knockAwayForce;

    [System.Serializable]
    public struct FightConfig
    {
        [Header("攻击信息")]
        public AttackConfig atkConfig;

        [Header("动画信息")]
        public AnimPlayConfig animPara;

        public int animStartFrame;
        public int animEndFrame;
        public float animSpeed;
        public float animLenth => FrameToTime(animEndFrame);
        public float FrameToTime(int frame)
        {
            return (float)(frame - animStartFrame) / 30 / animSpeed;
        }
    }
    [System.Serializable]
    public struct AttackConfig
    {
        public float damage;
        public int atkStartFrame;
        public int atkEndFrame;
        public List<int> colliderIndex;
    }

}
