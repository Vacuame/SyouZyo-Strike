using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum PatrolType
{
    ReverseOnEnd,Circle
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorTree))]
public class Enemy : Character
{
    [HideInInspector]public NavMeshAgent nav;
    protected BehaviorTree bt;

    [SerializeField,Header("Ѳ������")] private Transform patrolPointList;
    [SerializeField] public PatrolType patrolType;
    [HideInInspector]public List<Transform> patrolPoints;

    [Header("���岿λ����"),SerializeField] private List<Pair<string, List<Collider>>> parts;
    private Dictionary<GameObject,string> partDict = new Dictionary<GameObject,string>();

    [SerializeField]private BodyPartSet partSetting;
    private Dictionary<string, WeaknessData> weakDict = new Dictionary<string, WeaknessData>();

    protected override void Awake()
    {
        base.Awake();
        bt = GetComponent<BehaviorTree>();
        nav = GetComponent<NavMeshAgent>();
        for(int i=0;i<patrolPointList.childCount;i++)
        {
            patrolPoints.Add(patrolPointList.GetChild(i));
        }

        //��ÿ����λ����ܻ��¼�
        foreach(var partList in parts)
        {
            foreach(var part in partList.value)
            {
                EventManager.Instance.AddListener<HitInfo>("Hit" + part.gameObject.GetInstanceID(), OnHit);
                partDict.Add(part.gameObject, partList.key);
            }
        }
        //��ȡ��λ������
        foreach (var partSet in partSetting.setting)
            weakDict.Add(partSet.key, new WeaknessData(partSet.value));
        foreach (var partSet in partSetting.setting1)
            weakDict.Add(partSet.key, new WeaknessData(partSet.value.data));

        ABS.AttributeSetContainer.AddAttributeSet(new BodyAttr(partSetting));
    }

    protected override void OnHit(HitInfo hitInfo)
    {
        //�����˺������㲿λ���ԡ�����
        string partName = partDict[hitInfo.target];
        WeaknessData weakData = weakDict[partName];
        float dmgMul = weakData.multiplyDict[hitInfo.type];
        float dmg = hitInfo.damage * dmgMul;
        Debug.Log("dmg = " + dmg);
        ABS.AttrSet<CharaAtrr>()["health"].SetCurValueRelative(dmg, Tags.Calc.Sub);
        ABS.AttrSet<BodyAttr>()[partName].SetCurValueRelative(dmg, Tags.Calc.Sub);
        Debug.Log(ABS.AttrSet<CharaAtrr>().health.Value.currentValue);
        Debug.Log(partName+ " "+ABS.AttrSet<BodyAttr>()[partName].CurrentValue);
    }

    protected override void OnDead()
    {
        
    }

    protected override void OnDeadEnd()
    {
        
    }

    protected override void OnDeadStart()
    {
        
    }
}
