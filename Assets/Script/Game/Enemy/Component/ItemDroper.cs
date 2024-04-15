using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroper : MonoBehaviour
{
    [System.Serializable]
    public struct DropConfig
    {
        public int id;
        public float weight;
        public int numRange;
    }

    public List<DropConfig> dropConfigs=new List<DropConfig>();
    private List<float> weightList = new List<float>();

    private void Awake()
    {
        foreach(var config in dropConfigs)
        {
            weightList.Add(config.weight);
        }
    }

    public void DropItem(Vector3 pos)
    {
        int dropIndex = Calc.SelectRandom(weightList);

    }
}
