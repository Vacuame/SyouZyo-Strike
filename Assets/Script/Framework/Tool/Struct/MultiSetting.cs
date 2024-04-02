using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MultiSetting<T, S>
{
    [SerializeField] private List<Pair<T,S>> Setting;
    private Dictionary<T, int> _PairDict;
    private Dictionary<T, int> PairDict
    {
        get
        {
            if (_PairDict == null)
            {
                _PairDict = new Dictionary<T, int>();

                for (int i = 0; i < Setting.Count; i++)
                    PairDict.Add(Setting[i].key, i);
            }
            return _PairDict;
        }
    }
    
    public S Get(T key)
    {
        int index;
        if(PairDict.TryGetValue(key,out index))
            return Setting[index].value;
        return default;
    }
}
