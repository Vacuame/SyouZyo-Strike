using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Calc
{
    public static string GetTypeName<T>()where T : class
    {
        return typeof(T).Name;
    }
    public static bool TimerTick(ref this float w, bool unscaled = false)
    {
        float timePass = unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
        if (w > 0) w -= timePass;
        return w <= 0;
    }
    public static void Swap<T>(ref T a, ref T b)
    {
        T c = a;
        a = b;
        b = c;
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
    public static int SelectRandom(List<float> arr)
    {
        float totalWeight = 0;
        foreach (float weight in arr)
        {
            totalWeight += weight;
        }
        //在总范围取一个数，看看落在谁的范围
        float rand = Random.Range(0, totalWeight);
        Debug.Log(rand);
        float selectionSum = 0;
        for (int i = 0; i < arr.Count; i++)
        {
            selectionSum += arr[i];
            if (selectionSum >= rand)
            {
                return i;
            }
        }
        return 0;
    }

    public static float GetMappedRangeValueClamped(Vector2 inputRange,Vector2 outputRange,float input)
    {
        input = Mathf.Clamp(input, inputRange.x, inputRange.y);
        float ratio = (input - inputRange.x)/(inputRange.y-inputRange.x);
        return Mathf.Lerp(outputRange.x, outputRange.y, ratio);

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