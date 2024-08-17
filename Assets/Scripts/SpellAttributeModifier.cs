using System;
using UnityEngine;

[Serializable]
public class SpellAttributeModifier
{
    [SerializeField]
    public AttributeTypes Type;
    [SerializeField]
    public float IncrementValue;
    [SerializeField]
    public float MultiplierValue = 1f;
}
