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
                if(GetPathLength(pos,path,a.transform.position)<=sound.radious)
                    EventManager.Instance.TriggerEvent("Hear"+a.gameObject.GetInstanceID(), pos);
            }
        }
    }

    float GetPathLength(Vector3 fromPos,NavMeshPath path,Vector3 targetPos)
    {
        float pathLength = 
            Vector3.Distance(path.corners[0],fromPos)+ 
            Vector3.Distance(path.corners[path.corners.Length - 1], targetPos);
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            pathLength += Vector3.Distance(path.corners[i + 1], path.corners[i]);
        }
        return pathLength;
    }

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
