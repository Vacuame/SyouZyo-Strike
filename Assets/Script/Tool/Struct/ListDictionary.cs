using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ListDictionary<TKey,TValue>
{
    private Dictionary<TKey, List<TValue>> _dict = new Dictionary<TKey, List<TValue>>();
    public void Add(TKey key,TValue item)
    {
        if (_dict.ContainsKey(key))
        {
            _dict[key].Add(item);
        }
        else
        {
            _dict.Add(key, new List<TValue>() { item });
        }
    }
    public void Remove(TKey key, TValue item)
    {
        if (_dict.TryGetValue(key, out var list))
        {
            list.Remove(item);
            if (list.Count <= 0)
                _dict.Remove(key);
        }
    }
    public bool TryGetList(TKey id, out List<TValue> list)
    {
        return _dict.TryGetValue(id, out list);
    }

}
