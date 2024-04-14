using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using System;


public struct ActionSelct
{
    public int index;
    public float percentage;
}
public class MyRandomSelector : Composite
{
/*    public SharedInt stage;
    public List<string> stagePer;//格式 0-0.3,2-0.4  代表动作0概率30%*/
    public List<float> weightList = new List<float>();

    public int seed = 0;
    public bool useSeed = false;
    // A list of indexes of every child task. This list is used by the Fischer-Yates shuffle algorithm.
    //private List<int> childIndexList = new List<int>();
    // The random child index execution order.
    private Stack<int> childrenExecutionOrder = new Stack<int>();
    // The task status of the last child ran.
    private TaskStatus executionStatus = TaskStatus.Inactive;

    public override void OnAwake()
    {
        // If specified, use the seed provided.
        if (useSeed)
        {
            UnityEngine.Random.InitState(seed);
        }
/*        for (int i = 0; i < percentageList.Count; i++)
        {
            childIndexList.Add(i);
        }*/
    }

    public override void OnStart()
    {
/*        childIndexList.Clear();
        percentageList.Clear();
        string[] datas = stagePer[stage.Value].Split(',');
        foreach (string a in datas)
        {
            string[] data = a.Split('-');
            int index = int.Parse(data[0]);
            float percent = float.Parse(data[1]);
            childIndexList.Add(index);
            percentageList.Add(percent);
        }*/

        ShuffleChilden();
    }

    public override int CurrentChildIndex()
    {
        // Peek will return the index at the top of the stack.
        return childrenExecutionOrder.Peek();
    }

    public override bool CanExecute()
    {
        // Continue exectuion if no task has return success and indexes still exist on the stack.
        return childrenExecutionOrder.Count > 0 && executionStatus != TaskStatus.Success;
    }

    public override void OnChildExecuted(TaskStatus childStatus)
    {
        // Pop the top index from the stack and set the execution status.
        if (childrenExecutionOrder.Count > 0)
        {
            childrenExecutionOrder.Pop();
        }
        executionStatus = childStatus;
    }

    public override void OnConditionalAbort(int childIndex)
    {
        // Start from the beginning on an abort
        childrenExecutionOrder.Clear();
        executionStatus = TaskStatus.Inactive;
        ShuffleChilden();
    }

    public override void OnEnd()
    {
        // All of the children have run. Reset the variables back to their starting values.
        executionStatus = TaskStatus.Inactive;
        childrenExecutionOrder.Clear();
    }

    public override void OnReset()
    {
        // Reset the public properties back to their original values
        seed = 0;
        useSeed = false;
    }

    private void ShuffleChilden()
    {
        int chosen = Calc.SelectRandom(weightList);
        childrenExecutionOrder.Push(chosen);
    }
}
