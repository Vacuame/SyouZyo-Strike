using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAbilitySystem : SingletonMono_AutoInst<GameAbilitySystem>
{
    private List<AbilitySystemComponent> abilitySystems = new List<AbilitySystemComponent>();

    private void Update()
    {
        foreach (var component in abilitySystems)
            component.Tick();
    }

    public void Register(AbilitySystemComponent a)
    {
        if(!abilitySystems.Contains(a))
            abilitySystems.Add(a);
    }

    public void UnRegister(AbilitySystemComponent a)
    {
        abilitySystems.Remove(a);
    }

}
