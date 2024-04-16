using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NavExtension
{
    public static float GetPathLength(Vector3 fromPos, NavMeshPath path, Vector3 targetPos)
    {
        float pathLength =
            Vector3.Distance(path.corners[0], fromPos) +
            Vector3.Distance(path.corners[path.corners.Length - 1], targetPos);
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            pathLength += Vector3.Distance(path.corners[i + 1], path.corners[i]);
        }
        return pathLength;
    }
}
