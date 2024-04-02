using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PureMVC.Patterns.Command
{
    public abstract class GroupCommand 
    {
        public GroupCommand()
        {

        }

        public abstract SubCommandRegister[] CommandList();
        protected IFacade Facade
        {
            get
            {
                return Patterns.Facade.Facade.GetInstance(() => new Facade.Facade());
            }
        }
    }

    public struct SubCommandRegister
    {
        public SubCommandRegister(string notificationName, UnityAction<INotification> excute)
        {
            this.notificationName = notificationName;
            this.excute = excute;
        }
        public string notificationName;
        public UnityAction<INotification> excute;
    }

}