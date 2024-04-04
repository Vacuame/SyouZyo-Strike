using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MoleMole
{
    /// <summary>
    /// 用处是重写后可以给Panel带数据，比如确定栏显示的字
    /// </summary>
	public class PanelContext
    {
        public UIType uiType { get; private set; }

        public PanelContext(UIType viewType)
        {
            uiType = viewType;
        }
	}
}
