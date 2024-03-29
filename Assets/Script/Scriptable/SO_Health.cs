using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewHealth", menuName = "Data/Attribute/Health")]
public class SO_Health : ScriptableObject
{
    [HideInInspector] public float curHealth;
    public float maxHealth;
    public SO_Health()
    {
        curHealth = maxHealth = -1;//нчоч
    }
    public SO_Health(ref SO_Health template)
    {
        curHealth = maxHealth = template.maxHealth;
    }



}
