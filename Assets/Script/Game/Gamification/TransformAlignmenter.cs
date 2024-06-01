using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformAlignmenter : SingletonMono<TransformAlignmenter>
{
    private List<AlignInfo> alignList = new List<AlignInfo>();

    public void AddAlignRequest(AlignInfo alignInfo)
    {
        alignList.Add(alignInfo);
    }

    void Update()
    {
        List<int>delIndexs = new List<int>();
        for (int i = 0; i <alignList.Count; i++)
        {
            AlignInfo align = alignList[i];

            align.trans.position = Vector3.MoveTowards(align.trans.position, align.targetPos, 
                align.moveSpeed * Time.deltaTime);

            align.trans.rotation = Quaternion.RotateTowards(align.trans.rotation, 
                align.targetRotate, align.rotateSpeed * Time.deltaTime);

            if(align.time.TimerTick())
            {
                align.onAlignOver?.Invoke();
                delIndexs.Add(i);
            }
        }

        foreach (var index in delIndexs)
            alignList.RemoveAt(index);

    }

    public class AlignInfo
    {
        public Transform trans;
        public Vector3 targetPos;
        public Quaternion targetRotate;
        public float moveSpeed;
        public float rotateSpeed;
        public float time;
        public UnityAction onAlignOver;

        public AlignInfo(Transform trans, Vector3 targetPos, Vector3 targetForward, float time, UnityAction onAlignOver = null) :
            this(trans,targetPos,Quaternion.LookRotation(targetForward),time,onAlignOver)
        {
            
        }

        public AlignInfo(Transform trans, Vector3 targetPos, Quaternion targetRotate, float time ,UnityAction onAlignOver = null)
        {
            this.time = time;
            this.trans = trans;
            this.targetPos = targetPos;
            this.targetRotate = targetRotate;
            moveSpeed = Vector3.Distance(trans.position, targetPos)/time;
            rotateSpeed = Quaternion.Angle(trans.rotation, targetRotate) / time;
            this.onAlignOver = onAlignOver;
        }
    }


}
