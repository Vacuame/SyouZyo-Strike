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

        //取消在CancleTag里的能力
        var cancleTags = _abilities[abilityName].ability.Tag.CancelAbilitiesWithTags;
        foreach (var kv in _abilities)
        {
            var abilityTag = kv.Value.ability.Tag;
            if (abilityTag.AssetTag.HasAnyTags(cancleTags))
            {
                _abilities[kv.Key].TryCancelAbility();
            }
        }

        return true;
    }

    public void GrantAbility(AbstractAbility ability)
    {
        if (_abilities.ContainsKey(ability.Name)) return;
        var abilitySpec = ability.CreateSpec(_owner);
        abilitySpec.OnGet();
        _abilities.Add(ability.Name, abilitySpec);
    }
    public void RemoveAbility(string abilityName)
    {
        if (!_abilities.ContainsKey(abilityName)) return;

        EndAbility(abilityName);
        _abilities[abilityName].OnRemoved();
        _abilities.Remove(abilityName);
    }
    public void EndAbility(string abilityName)
    {
        if (!_abilities.ContainsKey(abilityName)) return;
        _abilities[abilityName].TryEndAbility();
    }
    public void Tick()
    {
        foreach(var a in _abilities)
            a.Value.Tick();
    }

    public void AnimatorMove()
    {
        foreach (var a in _abilities)
            if(a.Value.IsActive)
                a.Value.AnimatorMove();
    }

}
