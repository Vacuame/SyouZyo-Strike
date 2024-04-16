using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptableObjectExtend
{
    public static T[] InstantiateArray<T>(T[] obj) where T : ScriptableObject
    {
        T[] res = new T[obj.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            res[i] = Object.Instantiate(obj[i]);
        }
        return res;
    }
}
