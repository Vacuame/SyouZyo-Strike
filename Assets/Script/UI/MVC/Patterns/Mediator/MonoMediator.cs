/*using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using UnityEngine;

public class MonoMediator : MonoBehaviour,IMediator
{
    public static string NAME = "MonoMediator";

    public MonoMediator(string mediatorName, object viewComponent = null)
    {
        MediatorName = mediatorName ?? NAME;
        ViewComponent = viewComponent;
    }

    public virtual string[] ListNotificationInterests()
    {
        return new string[0];
    }

    public virtual void HandleNotification(INotification notification)
    {
    }

    public virtual void OnRegister()
    {
    }

    public virtual void OnRemove()
    {
    }

    public virtual void SendNotification(string notificationName, object body = null, string type = null)
    {
        Facade.SendNotification(notificationName, body, type);
    }
    protected IFacade Facade
    {
        get
        {
            return PureMVC.Patterns.Facade.Facade.GetInstance(() => new Facade());
        }
    }

    /// <summary>the mediator name</summary>
    public string MediatorName { get; protected set; }

    /// <summary>The view component</summary>
    public object ViewComponent { get; set; }



}
*/