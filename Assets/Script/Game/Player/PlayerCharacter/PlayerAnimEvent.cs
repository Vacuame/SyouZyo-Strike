using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    public List<SoundConfig> footSteps = new List<SoundConfig>();

    public void MakeFootStep(int level)
    {
        SoundMaker.Instance.MakeSound(transform.position, footSteps[level],new SoundInfo(SoundType.Sound));
    }
}
