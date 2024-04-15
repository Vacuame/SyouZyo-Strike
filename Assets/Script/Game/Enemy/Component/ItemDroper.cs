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
        public Vector2Int numRange;
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

    public void DropItem(Vector3 pos,Quaternion rotation)
    {
        int dropIndex = Calc.SelectRandom(weightList);
        DropConfig dropConfig = dropConfigs[dropIndex];
        if (dropConfig.id == 0)
            return;
        PickableObj prefab = ItemManager.Instance.GetPickablePrefab(dropConfig.id);
        if (prefab == null)
        {
            Debug.LogError($"想Drop的id = {dropConfig.id}的prefab没有找到");
            return;
        }
        PickableObj obj = GameObject.Instantiate(prefab, pos, rotation);
        int num = Random.Range(dropConfig.numRange.x, dropConfig.numRange.y + 1);
        obj.extraSet.num = num;

        obj.AddLightColum();
    }
}
