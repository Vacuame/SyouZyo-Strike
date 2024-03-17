using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystemComponent : MonoBehaviour
{
    public AbilityContainer AbilityContainer;

    private bool _ready;
    private void Prepare()
    {
        if (_ready) return;
        AbilityContainer = new AbilityContainer(this);
        //GameplayEffectContainer = new GameplayEffectContainer(this);
        //AttributeSetContainer = new AttributeSetContainer(this);
        //GameplayTagAggregator = new GameplayTagAggregator(this);
        _ready = true;
    }
    private void Awake()
    {
        Prepare();
    }

    public void Tick()
    {
        AbilityContainer.Tick();
        //GameplayEffectContainer.Tick();
    }


    #region 调用成员的函数
    public bool TryActivateAbility(string abilityName, params object[] args) => 
        AbilityContainer.TryActivateAbility(abilityName, args);
    public void TryEndAbility(string abilityName)=>
        AbilityContainer.EndAbility(abilityName);
    public void GrandAbility(AbstractAbility abstractAbility)=>
        AbilityContainer.GrantAbility(abstractAbility);
    public void RemoveAbility(string abilityName)=>
        AbilityContainer.RemoveAbility(abilityName);

    internal bool HasAllTags(GameplayTagSet activationRequiredTags)
    {
        throw new NotImplementedException();
    }

    internal bool HasAnyTags(GameplayTagSet activationBlockedTags)
    {
        throw new NotImplementedException();
    }
    #endregion
}
