using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemyNormalAttack : AbstractAbility<EnemyNormalAttackAsset>
{
    public EnemyNormalAttack(AbilityAsset setAsset,params object[] binds) : base(setAsset,binds)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new AttackSpec(this, owner);
    }

    public class AttackSpec : TimeLineAbilitySpec
    {
        EnemyNormalAttack attack;
        EnemyNormalAttackAsset asset => attack.AbilityAsset;

        Enemy me;

        Collider collider;

        public AttackSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            attack = ability as EnemyNormalAttack;
            me = attack.binds[0] as Enemy;
            collider = attack.binds[1] as Collider;
            InitTimeLine();
        }

        public override void ActivateAbility(params object[] args)
        {
            collider.enabled = true;

            SoundManager.GetOrCreateInstance()?.PlaySound(SoundPoolType.SFX, asset.atkSound, me.transform.position);

            base.ActivateAbility(args);
        }
        public override void EndAbility()
        {
            collider.enabled = false;
        }
        public override void InitTimeLine()
        {
            timeLine.AddEvent(0, () => asset.animPara.PlayAnim(me.anim));
            foreach(var config in asset.atkConfigs)
            {
                float makeDmgTime = (float)(config.makeDmgFrame - asset.animStartFrame) / 30;
                timeLine.AddEvent(makeDmgTime, ()=> MakeDamage(config.dmg));
            }
            timeLine.AddEvent(asset.animLenth, EndSelf);
        }

        public void MakeDamage(float dmg)
        {
            Collider[] list = collider.Overlap(asset.targetMask);
            GameObject dmgObj = null;
            float closestDistance = 999f;

            foreach (var a in list)
            {
                float distance = Vector2.Distance(
                    new Vector2(owner.transform.position.x, owner.transform.position.z),
                    new Vector2(a.transform.position.x, a.transform.position.z));
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    dmgObj = a.gameObject;
                }
            }

            if(dmgObj != null)
                EventManager.Instance.TriggerEvent("Hit" + dmgObj.gameObject.GetInstanceID(), 
                    new HitInfo(HitType.Cut, dmg, owner.gameObject, dmgObj, 
                    pos: dmgObj.transform.position,
                    hitDire: (dmgObj.transform.position - owner.transform.position).normalized));
        }

    }


}
