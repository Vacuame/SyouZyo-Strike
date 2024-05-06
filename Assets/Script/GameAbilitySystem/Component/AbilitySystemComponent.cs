using UnityEngine;

public class AbilitySystemComponent : MonoBehaviour
{
    public AbilityContainer AbilityContainer { get; private set; }
    public GameplayTagAggregator GameplayTagAggregator { get; private set; }
    public AttributeSetContainer AttributeSetContainer { get; private set; }
    public GameplayEffectContainer GameplayEffectContainer { get; private set; }

    private bool _ready;
    public void Prepare()
    {
        if (_ready) return;
        AbilityContainer = new AbilityContainer(this);
        GameplayEffectContainer = new GameplayEffectContainer(this);
        AttributeSetContainer = new AttributeSetContainer(this);
        GameplayTagAggregator = new GameplayTagAggregator(this);
        _ready = true;
    }
    private void Awake()
    {
        Prepare();
    }

    private void OnEnable()
    {
        GameAbilitySystem.GetOrCreateInstance()?.Register(this);
    }

    private void OnDisable()
    {
        GameAbilitySystem.Instance?.UnRegister(this);
    }

    public void Tick()
    {
        AbilityContainer.Tick();
        GameplayEffectContainer.Tick();
    }
    private void OnAnimatorMove()
    {
        AbilityContainer.AnimatorMove();
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
    public bool HasAllTags(GameplayTagSet activationRequiredTags)=>
        GameplayTagAggregator.HasAllTags(activationRequiredTags);
    public bool HasAnyTags(GameplayTagSet activationBlockedTags)=>
        GameplayTagAggregator.HasAnyTags(activationBlockedTags);
    public bool HasTag(GameplayTag tag) =>
        GameplayTagAggregator.HasTag(tag);
    public void ApplyAbilityTags(AbilitySpec source)=>
        GameplayTagAggregator.ApplyAbilityTags(source);

    #endregion

    #region AttributeSetContainer
    public T AttrSet<T>() where T : AttributeSet
    {
        AttributeSetContainer.TryGetAttributeSet<T>(out var attrSet);
        return attrSet;
    }
    public AttributeBase GetAttributeBase(string setName, string shortName)
    {
        AttributeBase attrBase = AttributeSetContainer.GetAttributeBase(setName, shortName);
        return attrBase;
    }
    public float? GetAttrCurValue(string setName, string shortName)
    {
        var value = AttributeSetContainer.GetAttributeCurrentValue(setName, shortName);
        return value;
    }

    public float? GetAttrBaseValue(string setName, string shortName)
    {
        var value = AttributeSetContainer.GetAttributeBaseValue(setName, shortName);
        return value;
    }
    #endregion

    #region GameEffectContainer
    //TODO 整理一下
    public GameplayEffectSpec ApplyGameplayEffectTo(GameplayEffect gameplayEffect, AbilitySystemComponent target)
    {
        if (!gameplayEffect.HasRequiredTag(target))
            return null;
        return target.AddGameplayEffect(gameplayEffect.CreateSpec(this,target));
    }
    public GameplayEffectSpec ApplyGameplayEffectToSelf(GameplayEffect gameplayEffect) =>
        ApplyGameplayEffectTo(gameplayEffect, this);
    private GameplayEffectSpec AddGameplayEffect(GameplayEffectSpec spec)
    {
        var success = GameplayEffectContainer.AddGameplayEffectSpec(spec);
        return success ? spec : null;
    }

    public void ApplyModFromInstantGameplayEffect(GameplayEffectSpec spec)
    {
        foreach (var modifier in spec.GameplayEffect.Modifiers)
        {
            var attrBase = GetAttributeBase(modifier.AttributeSetName, modifier.AttributeShortName);
            float magnitude;

            if (modifier.MMC == null)
                magnitude = modifier.ModiferMagnitude;
            else
                magnitude = modifier.MMC.CalculateMagnitude(spec, modifier.ModiferMagnitude);

            attrBase.SetValueRelative(magnitude, modifier.Operation, modifier.ValueType);
        }
    }

    #endregion

    #endregion

}
