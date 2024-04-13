
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Mine/Extension/")]
public class SetBool : Action
{
    public bool value;
    [RequiredField]
    [Tooltip("The SharedBool to set")]
    public SharedBool sharedBool;

    public override TaskStatus OnUpdate()
    {
        sharedBool.Value = value;

        return TaskStatus.Success;
    }
}
