using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class Calc
{

    public static float TimePassBy(ref this float w, bool unscaled = false)
    {
        float timePass = unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
        if (w > 0) w -= timePass;
        return w;
    }

    public static Vector2 CircleRandomPoint(Vector2 pos,float radius)
    {
        Vector2 res = Vector2.zero;
        do
        {
            res.x = Random.Range(-radius, radius);
            res.y = Random.Range(-radius, radius);
        }
        while (res.sqrMagnitude > radius * radius);
        return res+pos;
    }


}