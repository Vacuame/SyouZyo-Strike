using BehaviorDesigner.Runtime.Tasks.Unity.UnityLight;
using MyUI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MeleeAsset;
/// <summary>
/// 近战连击
/// </summary>
public class Melee : AbstractAbility<MeleeAsset>
{
    /// <summary>
    /// binds: Character,EquipedKnife
    /// </summary>
    public Melee(AbilityAsset abilityAsset, params object[] binds) : base(abilityAsset, binds)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new MeleeSpec(this,owner);
    }

    public class MeleeSpec : TimeLineAbilitySpec
    {
        Melee melee => ability as Melee;
        MeleeAsset asset => melee.AbilityAsset;

        Character character;
        Animator anim => character.anim;

        //攻击判定

        EquipedKnife knife;
        BoxCollider atkRange => knife.atkRange;
        HashSet<GameObject> dmgedObject = new HashSet<GameObject>();

        //连击
        int meleeIndex;
        MeleeConfig curConfig;
        bool canNext;
        float nextClickTimer;

        public MeleeSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            character = melee.binds[0] as Character;
            knife = melee.binds[1] as EquipedKnife;
            
        }

        public override void ActivateAbility(params object[] args)
        {
            curConfig = asset.meleeConfigs[meleeIndex];
            dmgedObject.Clear();
            canNext = false;

            //如果是其他动作，则立刻播放，不要缓缓转换
            if (meleeIndex == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Melee"))
                anim.Play("MeleeInward");
            anim.SetInteger(asset.animParamName, meleeIndex+1);
            anim.SetFloat("meleeSpeed", curConfig.animSpeed);

            InitTimeLine();

            base.ActivateAbility(args);
        }

        protected override void AbilityTick()
        {
            nextClickTimer.TimerTick();

            if (character.controller.control.Player.Fire.WasPressedThisFrame())
                nextClickTimer = asset.nextMeleeEarlierClickTime;

            if (nextClickTimer>0 && canNext)
            {
                owner.GameplayTagAggregator.ApplyAbilityTags(this);
                meleeIndex = (meleeIndex + 1) % asset.meleeConfigs.Count;
                ActivateAbility();
            }  

            base.AbilityTick();

            if(atkRange.enabled)
                AtkCheck();
        }

        public override void EndAbility()
        {
            meleeIndex = 0;
            anim.SetInteger(asset.animParamName, 0);
        }

        public override void InitTimeLine()
        {
            timeLine = new AbilityTimeLine();
            float meleeSpeed = curConfig.animSpeed;
            timeLine.AddEvent(curConfig.atkStartTime/ meleeSpeed, AtkStart);
            timeLine.AddEvent(curConfig.atkEndTime/ meleeSpeed, AtkEnd);
            timeLine.AddEvent(curConfig.canNextTime / meleeSpeed, SetCanNext );
            timeLine.AddEvent(curConfig.meleeEndTime/ meleeSpeed, EndSelf);
            timeLine.AddEvent(curConfig.soundTime / meleeSpeed, ()=>
            SoundManager.GetOrCreateInstance()?.PlaySound(SoundPoolType.SFX, curConfig.meleeSound, knife.transform.position));
        }

        private void AtkStart()
        {
            atkRange.enabled = true;
        }
        private void AtkEnd()
        {
            atkRange.enabled = false;
        }
        private void AtkCheck()
        {
            var colList = ColliderExtend.Overlap(atkRange, asset.atkMask);
            foreach(var col in colList)
            {
                Transform colTrans = col.transform;
                GameObject colRootObj = TransformExtension.FindRootParent(colTrans).gameObject;
                if(!dmgedObject.Contains(colRootObj))
                {
                    Vector3 hitPoint = knife.transform.position;
                    Vector3 hitDir = (colRootObj.transform.position - character.transform.position).normalized;
                    EventManager.Instance.TriggerEvent(Consts.Event.Hit + col.gameObject.GetInstanceID(),
                        new HitInfo(HitType.Cut, knife.damage, character.gameObject,col.gameObject, hitPoint, hitDir));
                    dmgedObject.Add(colRootObj);
                }
            }
        }

        /// <summary>
        /// 如果一些动作可以在后摇执行，这里取消标签让他们可以执行
        /// </summary>
        private void SetCanNext()//提前取消标签
        {
            canNext = true;
            owner.GameplayTagAggregator.RestoreAbilityTags(this);
        }
    }

}
