
using UnityEngine;

[CreateAssetMenu(fileName = "DisableEnemy", menuName = "ABS/GameplayEffect/Cue/Enemy/DisableEnemy")]
public class CueDisableEnemy : GameplayCueInstant
{
    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueDisableEnemySpec(this, parameters);
    }
    public class CueDisableEnemySpec : GameplayCueInstantSpec
    {
        public CueDisableEnemySpec(GameplayCueInstant cue, GameplayCueParameters parameters) : base(cue, parameters)
        {
        }

        public override void Trigger()
        {
            foreach (var a in Owner.AbilityContainer.AbilitySpecs.Keys)
                Owner.TryEndAbility(a);

            //ÉèÖÃÐÐÎªÊ÷×´Ì¬ LoseBanlance true
            if (Owner.TryGetComponent(out Enemy enemy))
            {
                enemy.nav.destination = enemy.transform.position;
                enemy.nav.isStopped = true;

                enemy.bt.SetVariableValue("LoseBanlance", true);
                enemy.bt.DisableBehavior();
                enemy.bt.EnableBehavior();
            }
        }
    }
}
