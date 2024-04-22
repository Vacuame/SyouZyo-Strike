
using UnityEngine;

[CreateAssetMenu(fileName = "EnableEnemy", menuName = "ABS/GameplayEffect/Cue/Enemy/EnableEnemy")]
public class CueEnableEnemy : GameplayCueInstant
{
    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueEnableEnemySpec(this, parameters);
    }
    public class CueEnableEnemySpec : GameplayCueInstantSpec
    {
        public CueEnableEnemySpec(GameplayCueInstant cue, GameplayCueParameters parameters) : base(cue, parameters)
        {
        }

        public override void Trigger()
        {
            if (Owner.TryGetComponent(out Enemy enemy))
            {
                enemy.nav.isStopped = false;
                enemy.bt.SetVariableValue("LoseBanlance", false);
            }
        }
    }
}
