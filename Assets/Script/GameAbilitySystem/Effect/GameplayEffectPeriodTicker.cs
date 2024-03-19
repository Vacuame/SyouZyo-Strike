using System.Collections;
using UnityEngine;


public class GameplayEffectPeriodTicker
{
    private float _periodRemaining;
    private readonly GameplayEffectSpec _spec;
    public float Period => _spec.GameplayEffect.period;
    public GameplayEffectPeriodTicker(GameplayEffectSpec spec)
    {
        _spec = spec;
        _periodRemaining = Period;
    }

    public void Tick()
    {
        _spec.TriggerOnTick();

        if (_periodRemaining.TimerTick())
        {
            _periodRemaining = Period;
            _spec.TriggerOnExecute();
        }
    }

}
