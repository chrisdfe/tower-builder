using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Attribute<KeyType>
    {
        public KeyType key;

        public List<AttributeModifier> staticModifiers { get; } = new List<AttributeModifier>();
        public List<AttributeModifier> tickModifiers { get; } = new List<AttributeModifier>();

        public float min { get; private set; } = 0;
        public float max { get; private set; } = 100;

        float currentValue = 100;
        public float value =>
            ClampValue(
                staticModifiers.Aggregate(currentValue, (acc, modifier) => acc + modifier.amount)
            );

        public Attribute(KeyType key)
        {
            this.key = key;
        }

        public Attribute(KeyType key, float initialValue) : this(key)
        {
            this.currentValue = initialValue;
        }

        public Attribute(KeyType key, float initialValue, float min, float max) : this(key, initialValue)
        {
            this.min = min;
            this.max = max;
        }

        public Attribute(KeyType key, List<AttributeModifier> initialStaticModifiers, List<AttributeModifier> initialTickModifiers)
            : this(key)
        {
            this.staticModifiers = initialStaticModifiers;
            this.tickModifiers = initialTickModifiers;
        }

        public Attribute(
            KeyType key,
            float initialValue,
            float min,
            float max,
            List<AttributeModifier> initialStaticModifiers,
            List<AttributeModifier> initialTickModifiers
        )
            : this(key, initialValue, min, max)
        {
            this.staticModifiers = initialStaticModifiers;
            this.tickModifiers = initialTickModifiers;
        }

        public void CalculateTickModifiers()
        {
            tickModifiers.ForEach(modifier => currentValue += modifier.amount);
            currentValue = Mathf.Clamp(currentValue, min, max);
        }

        float ClampValue(float value) => Mathf.Clamp(value, min, max);
    }
}