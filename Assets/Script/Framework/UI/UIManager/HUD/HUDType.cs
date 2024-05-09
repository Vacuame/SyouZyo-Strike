using System;
using System.Collections;
using UnityEngine;

namespace MyUI
{
    public class HUDType : UIType
    {
        public Type classType;
        public HUDType(Type type,string path) : base(path)
        {
            classType = type;
        }
    }
}