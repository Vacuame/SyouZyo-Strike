using UnityEngine;

[CreateAssetMenu(fileName = "CueSetCollision", menuName = "ABS/GameplayEffect/Cue/Enemy/CueSetCollision")]
public class CueSetCollision : GameplayCueInstant
{
    public bool isTriger;
    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueSetCollisionSpec(this, parameters);
    }
    public class CueSetCollisionSpec : GameplayCueInstantSpec
    {
        CueSetCollision cueSetCollision;
        public CueSetCollisionSpec(GameplayCueInstant cue, GameplayCueParameters parameters) : base(cue, parameters)
        {
            cueSetCollision = cue as CueSetCollision;
        }

        public override void Trigger()
        {
            //ÉèÖÃÐÐÎªÊ÷×´Ì¬ LoseBanlance true
            if (Owner.TryGetComponent(out Character character))
            {
                character.cc.enabled = !cueSetCollision.isTriger;
            }
        }
    }
}
