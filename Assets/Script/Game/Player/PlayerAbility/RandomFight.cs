using MoleMole;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Melee;
using static RandomFightAsset;

/// <summary>
/// 随机选择一个攻击执行
/// </summary>
public class RandomFight : AbstractAbility<RandomFightAsset>
{
    /// <summary>
    /// binds: character checkCollider  atkColliders
    /// </summary>
    public RandomFight(AbilityAsset abilityAsset, params object[] binds) : base(abilityAsset, binds)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new RandomFightSpec(this, owner);
    }

    public class RandomFightSpec : TimeLineAbilitySpec
    {
        RandomFight randomFight;
        RandomFightAsset asset => randomFight.AbilityAsset;
        List<FightConfig> fightConfigs=>asset.fightActions;

        Character character;
        Animator anim => character.anim;
        HashSet<GameObject> dmgedObject = new HashSet<GameObject>();
        List<Collider> atkRanges;
        Collider checkRange;

        int curFightIndex;
        FightConfig curFightConfig => fightConfigs[curFightIndex];
        List<int> curAtkRangeIndex;

        public RandomFightSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            randomFight = ability as RandomFight;
            character = randomFight.binds[0] as Character;
            checkRange = randomFight.binds[1] as Collider;
            atkRanges = randomFight.binds[2] as List<Collider>;
        }

        public override void ActivateAbility(params object[] args)
        {
            curAtkRangeIndex = new List<int>();
            dmgedObject.Clear();
            curFightIndex = Random.Range(0, fightConfigs.Count);
            curFightConfig.animPara.PlayAnim(character.anim);
            anim.SetFloat(asset.animSpeedParamName, curFightConfig.animSpeed);

            InitTimeLine();
            base.ActivateAbility(args);
        }

        public override void InitTimeLine()
        {
            timeLine = new AbilityTimeLine();
            AttackConfig atk = curFightConfig.atkConfig;
            timeLine.AddEvent(curFightConfig.FrameToTime(atk.atkStartFrame), () => AtkStart(atk.colliderIndex));
            timeLine.AddEvent(curFightConfig.FrameToTime(atk.atkEndFrame), () => AtkEnd());
            timeLine.AddEvent(curFightConfig.animLenth, EndSelf);
        }

        protected override void AbilityTick()
        {
            base.AbilityTick();

            AtkCheck();
        }

        private void AtkCheck()
        {
            foreach(var i in curAtkRangeIndex)
            {
                var colList = ColliderExtend.Overlap(atkRanges[i], asset.atkMask);
                foreach (var col in colList)
                {
                    Transform colTrans = col.transform;
                    GameObject colObj = col.gameObject;
                    if (!dmgedObject.Contains(colObj))
                    {
                        Vector3 hitDir = (colTrans.position - character.transform.position).normalized;
                        EventManager.Instance.TriggerEvent(Consts.Event.Hit + col.gameObject.GetInstanceID(),
                            new HitInfo(HitType.Impulse, curFightConfig.atkConfig.damage,
                            character.gameObject, col.gameObject, hitDire: hitDir));

                        CueKnockedAway knock = Resources.Load<CueKnockedAway>("ScriptObjectData/Cue/KnockedAway");
                        knock.ApplyFrom(this, col.gameObject, hitDir * asset.knockAwayForce);

                        dmgedObject.Add(colObj);
                    }
                }
            }
        }

        private void AtkStart(List<int> colliderIndex)
        {
            curAtkRangeIndex = new List<int>(colliderIndex);
        }
        private void AtkEnd()
        {
            curAtkRangeIndex = new List<int>();
        }
    }
}
