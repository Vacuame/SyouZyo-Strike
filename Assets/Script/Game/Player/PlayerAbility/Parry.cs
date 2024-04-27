using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : AbstractAbility<ParryAsset>
{
    public Parry(AbilityAsset abilityAsset, params object[] binds) : base(abilityAsset, binds)
    {
    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new ParrySpec(this,owner);
    }

    public class ParrySpec : TimeLineAbilitySpec
    {
        Parry parry;
        ParryAsset asset => parry.AbilityAsset;
        Collider col;
        Character character;
        Animator anim => character.anim;

        private bool perfectTiming;
        public ParrySpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            parry = ability as Parry;

            character = parry.binds[0] as Character;

            //Åö×²Ìå
            if (col == null)
                col = GameObject.Instantiate<Collider>(asset.parryColPrefab,owner.transform);
            EventManager.Instance.AddListener<HitInfo>("Hit"+col.gameObject.GetInstanceID(),OnParryHit);
            col.enabled = false;

            InitTimeLine();
        }

        public override void InitTimeLine()
        {
            timeLine.AddEvent(asset.perfectParryStTime, () => perfectTiming = true);
            timeLine.AddEvent(asset.perfectParryEdTime, () => perfectTiming = false);
        }

        public override void ActivateAbility(params object[] args)
        {
            col.enabled = true;

            anim.SetBool("parry",true);

            base.ActivateAbility(args);

        }
        public override void EndAbility()
        {
            col.enabled = false;
            anim.SetBool("parry", false);
        }

        private void OnParryHit(HitInfo info)
        {
            if(perfectTiming)
            {
                anim.Play("PerfectParry");
            }
            else
            {
                anim.Play("ParryImpact");
            }
        }

    }

}
