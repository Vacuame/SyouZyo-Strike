using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAttr : AttributeSet
{
    public AttributeBase health;

    public CharaAttr(CharaAttr_SO asset)
    {
        health = new AttributeBase(SetName, "health", asset.health);
    }

    public override AttributeBase this[string key]
    {
        get
        {
            switch (key)
            {
                case "health": return health;
            }
            return null;
        }
    }

    public override string[] AttributeNames { get; } =
      {
          "health",
      };

}
