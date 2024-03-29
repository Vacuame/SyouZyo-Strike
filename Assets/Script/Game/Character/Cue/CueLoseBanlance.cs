using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理LoseBanlance的实际逻辑
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
            //结束所有行动
            //设置行为树状态 LoseBanlance true
        }

        public override void OnRemove()
        {
            Debug.Log("Los OnRemove");
            //设置行为树状态 LoseBanlance false
        }

        public override void OnTick()
        {
            
        }
    }

}
