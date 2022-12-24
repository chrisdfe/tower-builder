using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Attribute
    {
        public List<AttributeModifier> staticModifiers { get; } = new List<AttributeModifier>();
        public List<AttributeModifier> tickModifiers { get; } = new List<AttributeModifier>();

        protected float defaultValue;
        protected float currentValue;

        public float min { get; private set; } = 0;
        public float max { get; private set; } = 100;

        public float value =>
            ClampValue(
                staticModifiers.Aggregate(currentValue, (acc, modifier) => acc + modifier.value)
            );

        public Attribute(float initialValue)
        {
            this.defaultValue = initialValue;
            this.currentValue = initialValue;
        }

        public Attribute(float initialValue, float min, float max) : this(initialValue)
        {
            this.min = min;
            this.max = max;
        }

        public void CalculateTickModifiers()
        {
            tickModifiers.ForEach(modifier => currentValue += modifier.value);
            currentValue = Mathf.Clamp(currentValue, min, max);
        }

        float ClampValue(float value) => Mathf.Clamp(value, min, max);


        public void Reset()
        {
            this.currentValue = this.defaultValue;
        }
    }
}