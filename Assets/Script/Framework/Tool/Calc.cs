using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class Calc
{

    public static bool TimerTick(ref this float w, bool unscaled = false)
    {
        float timePass = unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
        if (w > 0) w -= timePass;
        return w <= 0;
    }

    public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        return Mathf.Clamp(lfAngle % 360, lfMin, lfMax);
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

    public static int GetUnuseIntInDic(IDictionary dic)
    {
        int randomNum;
        do
        {
            randomNum = Random.Range(int.MinValue, int.MaxValue);
        }
        while (dic.Contains(randomNum));//不能是0，0代表失败
        return randomNum;
    }
    public static string GetUnuseStrInDic(IDictionary dic)
    {
        int randomNum;
        do
        {
            randomNum = Random.Range(int.MinValue, int.MaxValue);
        }
        while (dic.Contains(randomNum.ToString()));//不能是0，0代表失败
        return randomNum.ToString();
    }

    public static bool TryGetInterfaceInTransform<T>(Transform transform, out T res) where T : class
    {
        res = null;
        if (transform != null)
            foreach (var a in transform.GetComponents<MonoBehaviour>())
            {
                res = a as T;
                if (res != null)
                    break;
            }
        return res != null;
    }


}