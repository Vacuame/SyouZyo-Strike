using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public enum PatrolType
{
    ReverseOnEnd, Circle
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorTree))]
public class Enemy : Character
{
    [HideInInspector] public NavMeshAgent nav;
    [HideInInspector] public BehaviorTree bt;

    [SerializeField, Header("巡逻设置")] private Transform patrolPointList;
    [SerializeField] public PatrolType patrolType;
    [HideInInspector] public List<Transform> patrolPoints;

    [Header("身体部位设置"), SerializeField] private List<Pair<string, List<Collider>>> parts;
    private Dictionary<GameObject, string> partDict = new Dictionary<GameObject, string>();

    [SerializeField] private BodyPartSet partSetting;
    private Dictionary<string, WeaknessData> weakDict = new Dictionary<string, WeaknessData>();

    [Header("攻击设置")]
    public BoxCollider normalAtkRange;

    protected override void Awake()
    {
        base.Awake();
        bt = GetComponent<BehaviorTree>();
        nav = GetComponent<NavMeshAgent>();

        for (int i = 0; i < patrolPointList.childCount; i++)
        {
            patrolPoints.Add(patrolPointList.GetChild(i));
        }
        #region 注册部位

        //给每个部位添加受击事件
        foreach (var partList in parts)
        {
            foreach (var part in partList.value)
            {
                EventManager.Instance.AddListener<HitInfo>("Hit" + part.gameObject.GetInstanceID(), OnHit);
                partDict.Add(part.gameObject, partList.key);
            }
        }
        //读取部位的数据，转换成部位的伤害倍率的字典
        foreach (var partSet in partSetting.setting)
            weakDict.Add(partSet.key, new WeaknessData(partSet.value));
        foreach (var partSet in partSetting.setting1)
            weakDict.Add(partSet.key, new WeaknessData(partSet.value.data));

        //添加部位的生命值到ABS
        ABS.AttributeSetContainer.AddAttributeSet(new BodyAttr(partSetting));

        //给部位的生命值变化添加事件
        BodyAttr bodyAttr = ABS.AttrSet<BodyAttr>();
        foreach (string name in bodyAttr.AttributeNames)
        {
            bodyAttr[name].onPostCurrentValueChange += OnBodyPartToughnessPost;
        }
        #endregion

    }

    protected override void OnHit(HitInfo hitInfo)
    {
        if (bDead) return;
        //计算伤害
        string partName = partDict[hitInfo.target];
        WeaknessData weakData = weakDict[partName];
        float dmgMul = weakData.multiplyDict[hitInfo.type];
        float dmg = hitInfo.damage * dmgMul;

        //生命值和部位生命值都减少
        ABS.AttrSet<CharaAttr>().health.SetValueRelative(dmg, Tags.Calc.Sub);
        ABS.AttrSet<BodyAttr>()[partName].SetValueRelative(dmg, Tags.Calc.Sub);

        //发现玩家
        bt.SetVariableValue("Target", hitInfo.source);
    }

    protected override void Dead()
    {
        foreach (var a in ABS.AbilityContainer.AbilitySpecs.Keys)
            ABS.TryEndAbility(a);
        ABS.GameplayEffectContainer.ClearGameplayEffect();
        nav.isStopped = true;
        bt.SetVariableValue("LoseBanlance", true);
        bt.DisableBehavior();
        int deadType = Random.Range(0, 2);
        anim.SetFloat("DeadType", deadType);
        anim.Play("Dead");
        cc.enabled = false;
        bDead = true;
        TimerManager.Instance.AddTimer(new Timer(() => Destroy(this.gameObject), 1, 20));
    }

    protected override void OnDeadEnd()
    {

    }


    private void OnBodyPartToughnessPost(AttributeBase toughness, float old, float now)
    {
        if (bDead) return;
        if (old > 0 && now <= 0)
        {
            toughness.RefreshCurValue();
            //获得Buff 失衡
            GameplayEffectAsset asset = Resources.Load<GameplayEffectAsset>("ScriptObjectData/Effect/LoseBanlance");
            CuePlayAnim cuePlayAnim = asset.CueOnAdd[0] as CuePlayAnim;
            CueLoseBanlance cueLoseBanlance = asset.CueDurational[0] as CueLoseBanlance;
            string loseBanlanceName;
            if (toughness.ShortName == "Body" && nav.velocity.sqrMagnitude >= 4)
                loseBanlanceName = "Run";
            else
                loseBanlanceName = toughness.ShortName;
            CueLoseBanlance.PartBalanceSet partBalanceSet = cueLoseBanlance.GetPartBanlanceSet(loseBanlanceName);
            asset.duration = partBalanceSet.loseBanlanceDuration;
            cuePlayAnim.animParameters.Add(new Pair<string, float>("HurtType", partBalanceSet.hurtType));
            ABS.ApplyGameplayEffectToSelf(new GameplayEffect(asset));
        }
    }
}
