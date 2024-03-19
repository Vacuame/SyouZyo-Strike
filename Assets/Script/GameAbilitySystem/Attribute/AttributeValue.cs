using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeValue
{
    public AttributeValue(float value)
    {
        currentValue = baseValue = value;
    }

    public float baseValue;

    public float currentValue;

}
