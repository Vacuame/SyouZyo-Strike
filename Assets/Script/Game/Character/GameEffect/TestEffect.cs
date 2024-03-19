using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : GameplayEffect<TestEffect_SO>
{
    public TestEffect(GameplayEffectAsset ass) : base(ass)
    {
    }

    public override GameplayEffectSpec CreateSpec(AbilitySystemComponent owner,AbilitySystemComponent source)
    {
        return new TestEffectSpec(this,owner,source);
    }


    public class TestEffectSpec : GameplayEffectSpec
    {
        private float a;
        TestEffect test;
        public TestEffectSpec(GameplayEffect gameplayEffect, AbilitySystemComponent source, AbilitySystemComponent owner) : base(gameplayEffect, source, owner)
        {
            test = gameplayEffect as TestEffect;
            a = test.EffectAsset.data;
        }

        public override void TriggerOnExecute()
        {
            Debug.Log(a++);
            base.TriggerOnExecute();
        }

    }

}
