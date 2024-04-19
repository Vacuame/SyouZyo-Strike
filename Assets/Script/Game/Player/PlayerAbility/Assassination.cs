using MoleMole;
using UnityEngine;

public class Assassination : AbstractAbility<AssassinationAsset>
{
    /// <summary>
    /// binds：Character Collider
    /// </summary>
    public Assassination(AbilityAsset setAsset, params object[] binds) : base(setAsset, binds)
    {
    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new AssassinationSpec(this, owner);
    }

    public class AssassinationSpec : TimeLineAbilitySpec
    {
        Character character;
        Animator anim => character.anim;
        Collider atkRange;
        AssassinationAsset asset;

        GameObject target;

        public AssassinationSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            asset = (ability as Assassination).AbilityAsset;
            character = ability.binds[0] as Character;
            atkRange = ability.binds[1] as Collider;

            InitTimeLine();
        }
        protected override bool CheckOtherCondition()
        {
            return target!= null;
        }

        public override void ActivateAbility(params object[] args)
        {
            anim.Play("Assassination");
            HUDManager.GetHUD<PlayerHUD>()?.SetTip(null);
            base.ActivateAbility(args);
        }

        private bool lastCanActivate;
        protected override void SustainedTick()
        {
            if (IsActive) return;

            Collider[] cols = atkRange.Overlap(asset.assassinLayer);

            //其实应该每一个col都判定的，这里偷懒了
            if(cols.Length > 0 && CanAssassinate(cols[0].gameObject))
                target = cols[0].gameObject;
            else 
                target = null;

            bool canActivate = CanActivate();
            if (canActivate != lastCanActivate)
            {
                if (canActivate)
                    HUDManager.GetHUD<PlayerHUD>()?.SetTip("左键刺杀");
                else
                    HUDManager.GetHUD<PlayerHUD>()?.SetTip(null);
            }
            lastCanActivate = canActivate;
        }

        private bool CanAssassinate(GameObject target)
        {
            //背对
            float Angle = Vector3.Angle(character.transform.forward, target.transform.forward);
            if (Angle > asset.canAssassinateAngle)
                return false;

            //没有战斗
            if (target.GetComponent<Enemy>().bBattle)
                return false;

            return true;
        }

        public override void InitTimeLine()
        {
            timeLine.AddEvent(asset.doAtkTime, ()=> 
            EventManager.Instance.TriggerEvent(Consts.Event.Hit + target.GetInstanceID(),
            new HitInfo(HitType.Cut, asset.dmg, character.gameObject, target)));

            timeLine.AddEvent(asset.endTime, EndSelf);
        }
    }
}
