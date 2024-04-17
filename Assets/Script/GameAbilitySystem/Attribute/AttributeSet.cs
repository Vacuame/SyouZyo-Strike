using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttributeSet
{
    public string SetName=>this.GetType().Name;
    public abstract AttributeBase this[string key] { get; }
    public abstract string[] AttributeNames { get; }
}
