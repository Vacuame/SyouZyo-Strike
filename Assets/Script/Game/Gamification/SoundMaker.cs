using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SoundMaker:Singleton<SoundMaker>
{
    public void MakeSound(Vector3 pos,SoundConfig sound,SoundInfo info)
    {
        Collider[] cols = Physics.OverlapSphere(pos, sound.radious, sound.soundMask);
        foreach(var a in cols)
        {
            //不想用寻路了，有时候听不到，所以加上true||
            NavMeshPath path = new NavMeshPath();
            if(true||NavMesh.CalculatePath(a.transform.position, pos, NavMesh.AllAreas, path))
            {
                if(true || NavExtension.GetPathLength(pos,path,a.transform.position)<=sound.radious)
                    EventManager.Instance.TriggerEvent("Hear"+a.gameObject.GetInstanceID(), pos, info);
            }
        }
    }
}
[System.Serializable]
public struct SoundConfig
{
    public float radious;
    public LayerMask soundMask;
    public SoundConfig(float radious, LayerMask soundMask)
    {
        this.radious = radious;
        this.soundMask = soundMask;
    }
}

public enum SoundType { Sound, NotifyPlayer }

public class SoundInfo
{
    public SoundType type;
    public object[] paras;
    public SoundInfo(SoundType type,params object[] paras)
    {
        this.type = type;
        this.paras = paras;
    }
}