using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MoleMole
{
	public class BaseContext
    {
        public UIType uiType { get; private set; }

        public BaseContext(UIType viewType)
        {
            uiType = viewType;
        }
	}
}
