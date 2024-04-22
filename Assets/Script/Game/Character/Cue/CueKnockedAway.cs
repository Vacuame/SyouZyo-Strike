
using UnityEngine;

[CreateAssetMenu(fileName = "KnockedAway", menuName = "ABS/GameplayEffect/Cue/KnockedAway")]
public class CueKnockedAway : GameplayCueInstant
{
    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueKnockedAwaySpec(this,parameters);
    }

    public class CueKnockedAwaySpec : GameplayCueInstantSpec
    {
        public CueKnockedAwaySpec(GameplayCueInstant cue, GameplayCueParameters parameters) : base(cue, parameters)
        {

        }

        public override void Trigger()
        {
            GameObject target = (GameObject)parameters.customArguments[0];
            Vector3 force = (Vector3)parameters.customArguments[1];
            Rigidbody rb = target.GetComponent<Rigidbody>();
            Debug.Log(rb);
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

}
