using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����LoseBanlance��ʵ���߼�
/// </summary>
[CreateAssetMenu(fileName = "NewData", menuName = "ABS/GameplayEffect/Cue/LoseBanlance")]
public class CueLoseBanlance : GameplayCueDurational
{
    public override GameplayCueDurationalSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueLoseBanlanceSpec(this, parameters);
    }

    public class CueLoseBanlanceSpec : GameplayCueDurationalSpec
    {
        public CueLoseBanlanceSpec(GameplayCueDurational cue, GameplayCueParameters parameters) : base(cue, parameters)
        {

        }

        public override void OnAdd()
        {
            Debug.Log("Los OnAdd");
            //���������ж�
            //������Ϊ��״̬ LoseBanlance true
        }

        public override void OnRemove()
        {
            Debug.Log("Los OnRemove");
            //������Ϊ��״̬ LoseBanlance false
        }

        public override void OnTick()
        {
            
        }
    }

}
