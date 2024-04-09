using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class AnimParameter
{
    public string animName;
    public string layerName;
    public List<Pair<string, float>> animParameters = new List<Pair<string, float>>();

    public void PlayAnim(Animator animator)
    {
        if (layerName == "")
            animator.Play(animName);
        else
            animator.Play(animName, animator.GetLayerIndex(layerName));

        foreach (var a in animParameters)
            animator.SetFloat(a.key, a.value);
    }
}
