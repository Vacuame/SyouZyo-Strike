using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


[System.Serializable]
public class AnimPlayConfig
{
    public string animName;
    public string layerName;

    //感觉不好用，切割了
/*    public List<AnimParamConfig<float>> floatParam;
    public List<AnimParamConfig<bool>> boolParam;
    public List<AnimParamConfig<int>> intParam;
    public List<string> trigerParam;*/

    public AnimPlayConfig():this("")
    {

    }
    public AnimPlayConfig(string animName, string layerName)
    {
        this.animName = animName;
        this.layerName = layerName;
    }
    public AnimPlayConfig(string animName) : this(animName, "")
    {
        
    }

    public void PlayAnim(Animator animator)
    {
        if(animName!="")
        {
            if (layerName == "")
                animator.Play(animName);
            else
                animator.Play(animName, animator.GetLayerIndex(layerName));

/*
            foreach (var a in floatParam)
                animator.SetFloat(a.paramName, a.value);
            foreach(var a in intParam)
                animator.SetInteger(a.paramName, a.value);
            foreach (var a in boolParam)
                animator.SetBool(a.paramName, a.value);
            foreach (var a in trigerParam)
                animator.SetTrigger(a);*/
        }
        else
        {
            Debug.LogError("AnimPlayConfig 没有设置动画名字！");
        }
    }
/*
    [System.Serializable]
    public class AnimParamConfig<T>
    {
        public string paramName;
        public T value;
    }*/
}
