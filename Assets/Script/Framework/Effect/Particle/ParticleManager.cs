using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//TODO �ĳ�Singleton
public class ParticleManager : SingletonMono<ParticleManager>
{
    [SerializeField] private ParticleSet particleSet;

    private Dictionary<string,ParticleGroup> particleDic = new Dictionary<string, ParticleGroup>();

    private LinkedList<MyParticle> activeParticle=new LinkedList<MyParticle>();
    private Dictionary<int,MyParticle>keepParticle=new Dictionary<int,MyParticle>();

    protected override void Init()
    {
        foreach(var a in particleSet.lists)
        {
            var home = new GameObject(a.name).transform;
            home.SetParent(transform);
            particleDic.Add(a.name, new ParticleGroup(a,home));
        }
    }

    private void Update()
    {
        var node = activeParticle.First;
        while(node!=null)
        {
            MyParticle p = node.Value;
            var nextNode=node.Next;
            if(p.lifetime.TimerTick())
            {
                p.particle.gameObject.SetActive(false);
                particleDic[p.name].partis.Push(p);
                activeParticle.Remove(node);
            }
            node = nextNode;
        }

        foreach(var group in particleDic)//�ʵ�ɾ��������Ч
            group.Value.Update();
    }

    public void PlayEffect(string name,Vector3 pos,Quaternion rotation,bool keep=false,int keepId=-1)
    {
        if (particleDic.ContainsKey(name))
        {
            ParticleGroup group = particleDic[name];
            MyParticle p;

            p=group.GetParicle();

            p.particle.gameObject.SetActive(true);
            p.particle.transform.position = pos;
            p.particle.transform.rotation = rotation;
            p.lifetime = group.info.maxLifeTime;
            p.particle.Play();
            if (!keep)
                activeParticle.AddLast(p);
            else
                keepParticle.Add(keepId, p);
        }
        else
            Debug.LogWarning($"û���ҵ�[{name}]��Ч");
    }
    
    public int GetKeepId()
    {
        int res;
        do res = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        while (keepParticle.ContainsKey(res));
        return res;
    }
    public void CloseKeep(int keepId, bool setActiveFalse=false)
    {
        if(keepParticle.TryGetValue(keepId,out MyParticle p))
        {
            p.particle.Stop();
            if (setActiveFalse)
                p.particle.gameObject.SetActive(false);
            particleDic[p.name].partis.Push(p);
            keepParticle.Remove(keepId);
        }
    }
    public void SetKeep(int keepId, Vector3 pos, Quaternion rotation)
    {
        if (keepParticle.TryGetValue(keepId, out MyParticle p))
        {
            p.particle.transform.position = pos;
            p.particle.transform.rotation = rotation;
        }
    }

}

public class MyParticle
{
    public float lifetime;
    public string name;
    public ParticleSystem particle;
}

public class ParticleGroup
{
    public Stack<MyParticle> partis;//��Count���ǿ��е���Ч��
    public CustomPartcle info;

    private Transform home;

    private int curSub;//��ǰһ�μ�����Ŀ������ʼ�ļ�С��Ч
    private int sumNum;

    private float timeToSub;//����ʱ��̫���˾��𽥼��ٵ�Ĭ��ֵ
    private float maxSubTime;

    public void Update()
    {
        if(timeToSub.TimerTick())
        {
            int sub = Mathf.Min(partis.Count - info.defaultMaxNum, curSub);//ʹ������Ч������defaltMaxNum
            while (sub>0)
            {
                --sub;
                MyParticle p = partis.Pop();
                GameObject.Destroy(p.particle);
            }
            timeToSub = maxSubTime;
            curSub = curSub << 1;
        }
    }

    public MyParticle GetParicle()
    {
        //ʹ����Ч��ʹ������صı�������
        timeToSub = maxSubTime;
        curSub = 1;
        if (partis.Count <= 0)
            return CreateParticle();
        else
            return partis.Pop();
    }
    private MyParticle CreateParticle()
    {
        MyParticle newP = new MyParticle();
        newP.particle = GameObject.Instantiate(info.particle,home);
        newP.name = info.name;
        newP.particle.gameObject.SetActive(false);
        return newP;
    }

    public ParticleGroup(CustomPartcle info,Transform home,bool initByDefaultNum=true)
    {
        this.partis = new Stack<MyParticle>();
        this.info = info;
        this.home=home;
        maxSubTime = info.defaultMaxNum * info.maxLifeTime * 2;
        curSub = 1;
        if (initByDefaultNum)
            for (int i = 0; i < info.defaultMaxNum; i++)
                partis.Push(CreateParticle());
    }
}

