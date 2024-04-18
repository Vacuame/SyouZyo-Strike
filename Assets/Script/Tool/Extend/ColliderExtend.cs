using System.Collections;
using UnityEngine;


public static class ColliderExtend
{
    public static Collider[] Overlap(this Collider col,int layerMask)
    {
        col.enabled = true;
        Collider[] res = new Collider[0];
        if ( col is BoxCollider)
        {
            BoxCollider bCol = col as BoxCollider;
            res = Physics.OverlapBox(bCol.center + col.transform.position, 
                bCol.size / 2, bCol.transform.rotation, layerMask);
        }
        else if (col is SphereCollider)
        {
            SphereCollider sCol = col as SphereCollider;
            res = Physics.OverlapSphere(sCol.center + col.transform.position, sCol.radius, layerMask);
        }
        col.enabled = false;
        return res;
    }

}