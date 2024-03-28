using System.Collections;
using UnityEngine;


public enum HitType { None ,HG ,SMG ,AR ,RF ,SG ,Cut ,Explode }
[System.Serializable]
public class HitInfo
{
    public HitType type;
    public float damage;
    public GameObject source;
    public GameObject target;
    public Vector3 dire;
    public Vector3 pos;

    public HitInfo() : this(HitType.None, 0)
    {

    }
    public HitInfo(float damage):this()
    {
        this.damage = damage;
    }

    public HitInfo(HitType type, float damage, GameObject source = null,GameObject target = null, Vector3 pos = new Vector3(), Vector3 hitDire = new Vector3())
    {
        this.type = type;
        this.source = source;
        this.target = target;
        this.damage = damage;
        this.dire = hitDire;
        this.pos = pos;
    }
}