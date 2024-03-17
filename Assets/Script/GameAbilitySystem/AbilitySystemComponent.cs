using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystemComponent : MonoBehaviour
{
    public AbilityContainer AbilityContainer;
    public GameplayTagAggregator GameplayTagAggregator;
    

    private bool _ready;
    public void Prepare()
    {
        if (_ready) return;
        AbilityContainer = new AbilityContainer(this);
        //GameplayEffectContainer = new GameplayEffectContainer(this);
        //AttributeSetContainer = new AttributeSetContainer(this);
        GameplayTagAggregator = new GameplayTagAggregator(this);
        _ready = true;
    }

    public void Tick()//暂时让Player调用，以后集中管理
    {
        AbilityContainer.Tick();
        //GameplayEffectContainer.Tick();
    }


    #region 调用成员的函数

    #region AbilityContainer
    public bool TryActivateAbility(string abilityName, params object[] args) => 
        AbilityContainer.TryActivateAbility(abilityName, args);
    public void TryEndAbility(string abilityName)=>
        AbilityContainer.EndAbility(abilityName);
    public void GrandAbility(AbstractAbility abstractAbility)=>
        AbilityContainer.GrantAbility(abstractAbility);
    public void RemoveAbility(string abilityName)=>
        AbilityContainer.RemoveAbility(abilityName);
    #endregion

    #region GameplayTagAggregator
    internal bool HasAllTags(GameplayTagSet activationRequiredTags)=>
        GameplayTagAggregator.HasAllTags(activationRequiredTags);
    internal bool HasAnyTags(GameplayTagSet activationBlockedTags)=>
        GameplayTagAggregator.HasAnyTags(activationBlockedTags);
    internal void ApplyAbilityTags(AbilitySpec source)=>
        GameplayTagAggregator.ApplyAbilityTags(source);
    #endregion

    #endregion
}
