using PureMVC.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PureMVC.Patterns.Command
{
    public class SubCommand : SimpleCommand
    {
        private UnityAction<INotification> excute;
        public SubCommand(UnityAction<INotification> excute)
        {
            this.excute = excute;
        }

        public override void Execute(INotification notification)
        {
            excute?.Invoke(notification);
        }
    }
}