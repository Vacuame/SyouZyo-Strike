using System.Collections;
using UnityEngine;


public static class ColliderExtend
{
    public static Collider[] Overlap(this Collider col,int layerMask)
    {
        if( col is BoxCollider)
        {
            BoxCollider bCol = col as BoxCollider;
            return Physics.OverlapBox(bCol.center + col.transform.position, 
                bCol.size / 2, bCol.transform.rotation, layerMask);
        }
        else if (col is SphereCollider)
        {
            SphereCollider sCol = col as SphereCollider;
            return Physics.OverlapSphere(sCol.center + col.transform.position, sCol.radius, layerMask);
        }
        return new Collider[0];
    }

}