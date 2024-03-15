using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Control;

public class AbilityMgr
{
    private List<Ability> _abilities;
    private Character character;

    private Ability _focusAbility;
    private void SetFocusAbility(Ability ab)
    {
        if (_focusAbility != null)
            _focusAbility.OnFocusEnd();
        _focusAbility = ab;
        if (_focusAbility != null)
            _focusAbility.OnFocusStart();
    }
    public void TrySetFocusAbility(Ability ab)
    {
        if( _focusAbility == null)
        {
            SetFocusAbility(ab);
            return;
        }
    }

    public AbilityMgr(Character character) 
    {
        this.character = character;
        _abilities = new List<Ability>();
    }

    public void Update(PlayerActions input)
    {
        foreach (Ability ability in _abilities)
            ability.Update(); 

        if(_focusAbility != null)
            _focusAbility.OnFocusUpdate(input); 
    }

    public void AddAbility(Ability ability)
    {
        ability.OnAdd(character);
        _abilities.Add(ability);
    }

    public void RemoveAbility(Ability ability) 
    {
        if(_focusAbility == ability)
            SetFocusAbility(null);
        ability.OnRemove();
        _abilities.Remove(ability); 
    }

}
