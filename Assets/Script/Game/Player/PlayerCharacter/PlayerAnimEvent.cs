using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    public List<SoundConfig> footSteps = new List<SoundConfig>();
    public List<AudioClip> footStepSounds;

    public void MakeFootStep(int level)
    {
        SoundMaker.Instance.MakeSound(transform.position, footSteps[level],new SoundInfo(SoundType.Sound));

        int footStepSoundIndex = Random.Range(0, footStepSounds.Count);
        AudioClip footStepSound = footStepSounds[footStepSoundIndex];
        SoundManager.GetOrCreateInstance()?.PlaySound
            (SoundPoolType.SFX, footStepSound, transform.position,volume:0.5f);
    }
}
