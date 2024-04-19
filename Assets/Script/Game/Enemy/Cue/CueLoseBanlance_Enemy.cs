using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// 处理LoseBanlance的实际逻辑
/// </summary>
[CreateAssetMenu(fileName = "NewData", menuName = "ABS/GameplayEffect/Cue/LoseBanlance")]
public class CueLoseBanlance_Enemy : GameplayCueDurational
{
    [Serializable]
    public struct PartBalanceSet
    {
        public float loseBanlanceDuration;
        public string animName;
    }

    [SerializeField] private List<Pair<string, PartBalanceSet>> partSets = new List<Pair<string, PartBalanceSet>>();

    [SerializeField] private PartBalanceSet defaultPartSet;

    public PartBalanceSet GetPartBanlanceSet(string name)
    {
        if (partSets.Any(a => a.key == name))
            return partSets.Find(a => a.key == name).value;
        else
            return defaultPartSet;
    }

    public override GameplayCueDurationalSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueLoseBanlanceSpec(this, parameters);
    }

    public class CueLoseBanlanceSpec : GameplayCueDurationalSpec
    {
        public CueLoseBanlanceSpec(GameplayCueDurational cue, GameplayCueParameters parameters) : base(cue, parameters)
        {
            
        }

        Enemy enemy;

        public override void OnAdd()
        {
            //结束所有行动
            foreach(var a in Owner.AbilityContainer.AbilitySpecs.Keys)
                Owner.TryEndAbility(a);

            //设置行为树状态 LoseBanlance true
            if(Owner.TryGetComponent(out enemy)) 
            {
                enemy.nav.destination = enemy.transform.position;
                enemy.nav.isStopped = true;

                enemy.bt.SetVariableValue("LoseBanlance", true);
                enemy.bt.DisableBehavior();
                enemy.bt.EnableBehavior();
            }
            
        }

        public override void OnRemove()
        {
            //Debug.Log("LoseBanlance OnRemove");
            if (enemy != null)
            {
                enemy.nav.isStopped = false;
                enemy.bt.SetVariableValue("LoseBanlance", false);
            }
                
        }

        public override void OnTick()
        {
            
        }
    }

}
