using BehaviorDesigner.Runtime;
using MoleMole;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PatrolType
{
    ReverseOnEnd, Circle
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorTree))]
public class Enemy : Character
{
    #region ����
    [HideInInspector] public NavMeshAgent nav;
    [HideInInspector] public BehaviorTree bt;

    [SerializeField, Header("Ѳ������")] private Transform patrolPointList;
    [SerializeField] public PatrolType patrolType;
    [HideInInspector] public List<Transform> patrolPoints;
    [SerializeField] private AlertAttrAsset alertSetting;
    [SerializeField] private Transform alertTipPos;

    [Header("���岿λ����"), SerializeField] private List<Pair<string, List<Collider>>> parts;
    private Dictionary<GameObject, string> partDict = new Dictionary<GameObject, string>();

    [SerializeField] private BodyPartSet partSetting;
    private Dictionary<string, WeaknessData> weakDict = new Dictionary<string, WeaknessData>();

    [Header("��������")]
    [SerializeField] private int deadAnimTypeNum;
    #endregion

    #region ��ʼ��
    protected override void Awake()
    {
        base.Awake();
        bt = GetComponent<BehaviorTree>();
        nav = GetComponent<NavMeshAgent>();

        for (int i = 0; i < patrolPointList.childCount; i++)
        {
            patrolPoints.Add(patrolPointList.GetChild(i));
        }
    }
    private void Start()
    {
        RegistBodyPart();

        RegistSense();
    }

    protected virtual void RegistSense()
    {
        //����alertSetting�����¼�
        AlertAttr alertAttr = new AlertAttr(alertSetting);
        ABS.AttributeSetContainer.AddAttributeSet(alertAttr);
        alertAttr.alert.onPostCurrentValueChange += OnAlertPost;
        alertAttr.alert.onPreCurrentValueChange += (AttributeBase b, float v) => Mathf.Clamp(v, 0, b.BaseValue);
        alertAttr.searchTime.onPostCurrentValueChange += OnSearchTimePost;
        alertAttr.searchTime.onPreCurrentValueChange += (AttributeBase b, float v) => Mathf.Clamp(v, 0, b.BaseValue);
        ABS.ApplyGameplayEffectToSelf(new GameplayEffect(alertSetting.reduceAlertEffect));

        //��ʼ������λ��Ϊ��
        bt.SetVariableValue("PosToCheck", Consts.NullV3);

        //���Alert UI
        HUDManager.GetHUD<EnemyAlertHUD>(true).AddAlertTip(gameObject, alertTipPos);

        //�۾�
        ConeDetector eye = transform.GetComponentInChildren<ConeDetector>();
        eye.shouldLook += () =>
        {
            SharedGameObject targetVariable = bt.GetVariable("Target") as SharedGameObject;
            GameObject target = targetVariable.Value;
            return target == null;
        };
        eye.onLook += (GameObject obj) =>
        {
            watchingObj = obj;
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            float alertGrouthSpeed = alertSetting.alertGrowthSpeedCurve.Evaluate(distance);
            ABS.AttrSet<AlertAttr>().alert.SetValueRelative(alertGrouthSpeed * Time.deltaTime, Tags.Calc.Add);
        };

        //����
        EventManager.Instance.AddListener("Hear" + gameObject.GetInstanceID(), (Vector3 pos,SoundInfo info) =>
        {
            switch (info.type)
            {
                case SoundType.Sound:
                    SetPosToCheck(pos);
                    break;
                case SoundType.NotifyPlayer:
                    EnterBattle(info.paras[0] as GameObject,false);
                    break;
            }
        });
    }

    private void RegistBodyPart()
    {
        //��ÿ����λ����ܻ��¼�
        foreach (var partList in parts)
        {
            foreach (var part in partList.value)
            {
                EventManager.Instance.AddListener<HitInfo>("Hit" + part.gameObject.GetInstanceID(), OnHit);
                partDict.Add(part.gameObject, partList.key);
            }
        }
        //��ȡ��λ�����ݣ�ת���ɲ�λ���˺����ʵ��ֵ�
        foreach (var partSet in partSetting.setting)
            weakDict.Add(partSet.key, new WeaknessData(partSet.value));
        foreach (var partSet in partSetting.setting1)
            weakDict.Add(partSet.key, new WeaknessData(partSet.value.data));

        //��Ӳ�λ������ֵ��ABS
        ABS.AttributeSetContainer.AddAttributeSet(new BodyAttr(partSetting));

        //����λ������ֵ�仯����¼�
        BodyAttr bodyAttr = ABS.AttrSet<BodyAttr>();
        foreach (string name in bodyAttr.AttributeNames)
        {
            bodyAttr[name].onPostCurrentValueChange += OnBodyPartToughnessPost;
        }
    }

    #endregion

    #region ����
    private GameObject watchingObj;
    private bool bAlert,bBattle;
    private void EnterBattle(GameObject obj,bool discoverBySelf)
    {
        bt.SetVariableValue("Target", obj);
        if(discoverBySelf)
            bt.SetVariableValue("ToEnterBattle", true);
        BehaviorExtension.Restart(bt);
        bBattle = true;
    }
    private void OnAlertPost(AttributeBase alert, float old, float value)
    {
        EnemyAlertHUD alertHUD = HUDManager.GetHUD<EnemyAlertHUD>();
        alertHUD.SetVisiable(!bBattle && value > 0);
        alertHUD.GetAlertTip(gameObject).SetValue(alert.GetProportion());

        if (bBattle) return;

        if(old < value) //����
        {
            if (value >= alertSetting.alertToFind && old < alertSetting.alertToFind)
            {
                EnterBattle(watchingObj,true);
            }
            else if(value >= alertSetting.alertToCheck)
            {
                SetPosToCheck(watchingObj.transform.position);
                bAlert = true;
            }
        }
        else if(bAlert)
        {
            if(old>0 && value<=0)
            {
                bAlert = false;
                ABS.AttrSet<AlertAttr>().searchTime.SetCurValue(0);
            }
        }
    }
    private void OnSearchTimePost(AttributeBase searchTime, float old, float value)
    {
        if (old > 0 && value <= 0)
        {
            SetPosToCheck(Consts.NullV3);
        }
    }
    private void SetPosToCheck(Vector3 pos)
    {
        if (bBattle) return;

        if(pos == Consts.NullV3)
        {
            bt.SetVariableValue("PosToCheck", pos);
            return;
        }

        Vector3 oldPosToCheck = (bt.GetVariable("PosToCheck") as SharedVector3).Value;
        ABS.AttrSet<AlertAttr>().searchTime.RefreshCurValue();
        bt.SetVariableValue("PosToCheck", pos);
        bt.SetVariableValue("CheckRadius", 0.5f);
        
        if (oldPosToCheck == Consts.NullV3)//��һ������ʱ��ϵ�ǰ״̬
            BehaviorExtension.Restart(bt);
    }
    #endregion

    #region �˺�����
    protected override void OnHit(HitInfo hitInfo)
    {
        if (bDead) return;
        //�����˺�
        string partName = partDict[hitInfo.target];
        WeaknessData weakData = weakDict[partName];
        float dmgMul = weakData.multiplyDict[hitInfo.type];
        float dmg = hitInfo.damage * dmgMul;

        //����ֵ�Ͳ�λ����ֵ������
        ABS.AttrSet<CharaAttr>().health.SetValueRelative(dmg, Tags.Calc.Sub);

        if (!ABS.HasTag("LoseBanlance"))
            ABS.AttrSet<BodyAttr>()[partName].SetValueRelative(dmg, Tags.Calc.Sub);

        //�������
        if(bt.GetVariable("Target").GetValue() == null)
        {
            bt.Restart();
            bt.SetVariableValue("Target", hitInfo.source);
        }
        
    }
    private void OnBodyPartToughnessPost(AttributeBase toughness, float old, float now)
    {
        if (bDead) return;
        if (old > 0 && now <= 0)
        {
            toughness.RefreshCurValue();
            //���Buff ʧ��
            GameplayEffectAsset asset = Instantiate(Resources.Load<GameplayEffectAsset>("ScriptObjectData/Effect/LoseBanlance"));
            asset.CueOnAdd[0] = Instantiate(asset.CueOnAdd[0]);
            CuePlayAnim cuePlayAnim = asset.CueOnAdd[0] as CuePlayAnim;
            CueLoseBanlance_Enemy cueLoseBanlance = asset.CueDurational[0] as CueLoseBanlance_Enemy;
            string loseBanlanceName;
            if (toughness.ShortName == "Body" && nav.velocity.sqrMagnitude >= 4)
                loseBanlanceName = "Run";
            else
                loseBanlanceName = toughness.ShortName;
            CueLoseBanlance_Enemy.PartBalanceSet partBalanceSet = cueLoseBanlance.GetPartBanlanceSet(loseBanlanceName);
            asset.duration = partBalanceSet.loseBanlanceDuration;
            cuePlayAnim.animConfig = new AnimPlayConfig(partBalanceSet.animName);
            ABS.ApplyGameplayEffectToSelf(new GameplayEffect(asset));
        }
    }
    #endregion

    #region ����
    protected override void Dead()
    {
        HUDManager.GetHUD<EnemyAlertHUD>(true).RemoveAlertTip(gameObject);

        foreach (var a in ABS.AbilityContainer.AbilitySpecs.Keys)
            ABS.TryEndAbility(a);
        ABS.GameplayEffectContainer.ClearGameplayEffect();

        nav.isStopped = true;

        bt.SetVariableValue("LoseBanlance", true);
        bt.DisableBehavior();

        int deadType = Random.Range(0, deadAnimTypeNum);
        anim.SetFloat("DeadType", deadType);
        anim.Play("Dead");

        gameObject.layer = LayerMask.NameToLayer("Ignore");
        bDead = true;

        TimerManager.Instance.AddTimer(new Timer(OnDeadEnd, 1, 0.6f));
        TimerManager.Instance.AddTimer(new Timer(() => Destroy(this.gameObject), 1, 20));
    }

    protected override void OnDeadEnd() //TODO ���������������ִ�У�������Ƥ��
    {
        if(TryGetComponent(out ItemDroper itemDroper))
        {
            itemDroper.DropItem(feetTransform.position, transform.rotation);
        }
    }
    #endregion
}

