using System;
using UnityEngine;

public class AbilityTimeLine
{
    private bool bStart;//�Ƿ�ʼ
    public bool bPause;//�Ƿ���ͣ
    private float curTime;//��ǰ��ʱ    
    private Action reset;//�����¼�
    private Action<float> update;//ÿ֡�ص�

    public AbilityTimeLine()
    {
        Reset();
    }

    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="delay">�ӳ�ʱ��</param>
    /// <param name="id">ID����͸������</param>
    /// <param name="method">ִ�еĻص�</param>
    public void AddEvent(float delay, Action method)
    {
        LineEvent param = new LineEvent(delay,method);
        update += param.Invoke;//�����ж�
        reset += param.Reset;
    }

    //��ʼʱ����
    public void Start()
    {
        Reset();
        bStart = true;
        bPause = false;
    }

    //���ã���ԭ��
    public void Reset()
    {
        curTime = 0;//ʱ���߼�ʱ����
        bStart = false;//����ʼ
        bPause = false;//û��ʼ�Ͳ���̸��ͣ

        if (null != reset)
        {
            reset();//��ʱ���ߵ�LineEvent����ȥ���������¼���Event����reset���������е�ʱ�����¼���LineEvent��ҲҪ����
        }
    }

    public void Update()
    {
        if (!bStart || bPause)//ʱ���߿�ʼ����û�б���ͣ�ͽ�������
        {
            return;
        }
        float deltaTime = Time.deltaTime;
        curTime += deltaTime;
        if (null != update)
        {
            update(curTime);
        }
    }


    /* ==================��������LineEvent================== */
    private class LineEvent
    {
        public float Delay { get; protected set; }//�ӳ�ʱ��
        public Action Method { get; protected set; }//�ص�����
        public bool isInvoke = false;

        public LineEvent(float delay,Action method)
        {
            Delay = delay;
            Method = method;
            Reset();//���ø���״̬
        }

        public void Reset()
        {
            isInvoke = false;
        }

        //ÿִ֡�У��Լ�������֡��,time�Ǵ�ʱ���߿�ʼ����ĿǰΪֹ������ʱ��
        public void Invoke(float time)
        {
            //��ǰ�¼���û���ӳ�ʱ�䣬ֱ�ӷ���
            if (time < Delay)
            {
                return;
            }
            if (!isInvoke && null != Method)
            {
                isInvoke = true;
                Method();//��֤Method��ʱ���ߵ���������������ֻ��ִ��һ��
            }
        }
    }


}