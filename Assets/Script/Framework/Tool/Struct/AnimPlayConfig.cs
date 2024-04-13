using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


[System.Serializable]
public class AnimPlayConfig
{
    public string animName;
    public string layerName;
    public void PlayAnim(Animator animator)
    {
        if(animName!="")
        {
            if (layerName == "")
                animator.Play(animName);
            else
                animator.Play(animName, animator.GetLayerIndex(layerName));
        }
        else
        {
            Debug.LogError("AnimPlayConfig 没有设置动画名字！");
        }
    }
}
