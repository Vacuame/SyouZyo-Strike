using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SoundMaker:Singleton<SoundMaker>
{
    public void MakeSound(Vector3 pos,SoundConfig sound)
    {
        Collider[] cols = Physics.OverlapSphere(pos, sound.radious, sound.soundMask);
        foreach(var a in cols)
        {
            NavMeshPath path = new NavMeshPath();
            if(NavMesh.CalculatePath(a.transform.position, pos, NavMesh.AllAreas, path))
            {
                if(NavExtension.GetPathLength(pos,path,a.transform.position)<=sound.radious)
                    EventManager.Instance.TriggerEvent("Hear"+a.gameObject.GetInstanceID(), pos);
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
}
