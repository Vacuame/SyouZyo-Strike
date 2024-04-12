using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MeleeAsset;
/// <summary>
/// ��ս����
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

        EquipedKnife knife;

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

            canNext = false;
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
        }

        private void AtkStart()
        {
            //������ײ���
        }
        private void AtkEnd()
        {
            //�ر���ײ���
        }

        /// <summary>
        /// ���һЩ���������ں�ҡִ�У�����ȡ����ǩ�����ǿ���ִ��
        /// </summary>
        private void SetCanNext()//��ǰȡ����ǩ
        {
            canNext = true;
            owner.GameplayTagAggregator.RestoreAbilityTags(this);
        }
    }

}
