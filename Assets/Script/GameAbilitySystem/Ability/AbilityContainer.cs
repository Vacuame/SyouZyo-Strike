using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class AbilityContainer
{
    private readonly AbilitySystemComponent _owner;
    private readonly Dictionary<string, AbilitySpec> _abilities = new Dictionary<string, AbilitySpec>();
    public Dictionary<string, AbilitySpec> AbilitySpecs { get { return _abilities; } }

    public AbilityContainer(AbilitySystemComponent owner)
    {
        this._owner = owner;
    }
    public bool TryActivateAbility(string abilityName, params object[] args)
    {
        if (!_abilities.ContainsKey(abilityName)) return false;
        if (!_abilities[abilityName].TryActivateAbility(args)) return false;

        //取消Tag含有该能力的CancleTag的能力
        var cancleTags = _abilities[abilityName].Ability.Tag.CancelAbilitiesWithTags;
        foreach (var kv in _abilities)
        {
            var abilityTag = kv.Value.Ability.Tag;
            if (abilityTag.AssetTag.HasAnyTags(cancleTags))
            {
                _abilities[kv.Key].TryCancelAbility();
            }
        }

        return true;
    }
    public void EndAbility(string abilityName)
    {
        if (!_abilities.ContainsKey(abilityName)) return;
        _abilities[abilityName].TryEndAbility();
    }
    public void GrantAbility(AbstractAbility ability)
    {
        if (_abilities.ContainsKey(ability.Name)) return;
        var abilitySpec = ability.CreateSpec(_owner);
        _abilities.Add(ability.Name, abilitySpec);
    }
    public void RemoveAbility(string abilityName)
    {
        if (!_abilities.ContainsKey(abilityName)) return;

        EndAbility(abilityName);
        _abilities.Remove(abilityName);
    }
    public void Tick()
    {
        foreach(var a in _abilities)
            a.Value.Tick();
    }
}
