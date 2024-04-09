using UnityEngine;

public class Attack : AbstractAbility<Attack_SO>
{
    public Attack(AbilityAsset setAsset) : base(setAsset)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new AttackSpec(this, owner);
    }

    public class AttackSpec : TimeLineAbilitySpec
    {
        Attack attack;
        Attack_SO asset => attack.AbilityAsset;

        Character me;

        Collider collider;

        public AttackSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            attack = ability as Attack;
            InitTimeLine();
        }

        public override void ActivateAbility(params object[] args)
        {
            me = args[0] as Character;
            collider = args[1] as Collider;
            collider.enabled = true;

            base.ActivateAbility(args);
        }
        public override void EndAbility()
        {
            collider.enabled = false;
        }
        public override void InitTimeLine()
        {
            timeLine.AddEvent(0, () => asset.animPara.PlayAnim(me.anim));
            timeLine.AddEvent(asset.makeDmgTime, MakeDamage);
            timeLine.AddEvent(asset.endTime, EndSelf);
        }

        public void MakeDamage()
        {
            Collider[] list = collider.Overlap(asset.targetMask);
            foreach (var a in list)
            {
                EventManager.Instance.TriggerEvent("Hit" + a.gameObject.GetInstanceID(), new HitInfo(asset.dmg));
            }
        }

    }


}
