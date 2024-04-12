using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


[System.Serializable]
public class AnimPlay
{
    public string animName;
    public string layerName;

    public void PlayAnim(Animator animator)
    {
        if (layerName == "")
            animator.Play(animName);
        else
            animator.Play(animName, animator.GetLayerIndex(layerName));
    }
}
