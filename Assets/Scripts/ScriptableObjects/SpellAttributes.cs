using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SpellAttributes : ScriptableObject
{
    [SerializeField]
    private List<SpellAttributeModifier> _attributeValues = new()
    {
        new() { Type = AttributeTypes.Power, IncrementValue = 1f},
        new() { Type = AttributeTypes.Firerate, IncrementValue = 1f},
        new() { Type = AttributeTypes.Speed, IncrementValue = 1f},
        new() { Type = AttributeTypes.Size, IncrementValue = 1f},
        new() { Type = AttributeTypes.Pierce, IncrementValue = 0f},
        new() { Type = AttributeTypes.TimeToLive, IncrementValue = float.PositiveInfinity},
        new() { Type = AttributeTypes.Inaccuracy, IncrementValue = 0f},
    };

    [SerializeField]
    public Color Color = Color.black;

    public float GetAttributeValue(AttributeTypes attributeType)
    {
        var hasValue = false;
        var value = 0f;

        foreach (var attribute in _attributeValues)
        {
            if (attribute.Type != attributeType)
            {
                continue;
            }

            value += attribute.IncrementValue;
            value *= attribute.MultiplierValue;
            hasValue = true;
        }

        if (!hasValue)
        {
            Debug.LogWarning($"No value was set for attribute: {attributeType}");
        }

        return value;
    }

    public void SetAttributeValue(AttributeTypes attributeType, float value)
    {
        _attributeValues.RemoveAll(a => a.Type == attributeType);
        IncrementAttributeValue(attributeType, value);
    }

    public void IncrementAttributeValue(AttributeTypes attributeType, float value)
    {
        _attributeValues.Add(new SpellAttributeModifier() { Type = attributeType, IncrementValue = value });
    }

    public void MultiplyAttributeValue(AttributeTypes attributeType, float value)
    {
        _attributeValues.Add(new SpellAttributeModifier() { Type = attributeType, MultiplierValue = value });
    }
}
