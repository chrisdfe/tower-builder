using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Attributes
{
    public class ResidentAttribute
    {
        public enum Key
        {
            Energy
        }

        public Key key;

        public List<Modifier> staticModifiers { get; private set; } = new List<Modifier>();
        public List<Modifier> tickModifiers { get; private set; } = new List<Modifier>();

        public float min { get; private set; }
        public float max { get; private set; }

        float currentValue;
        public float value
        {
            get
            {
                float result = currentValue;
                staticModifiers.ForEach(modifier => result += modifier.amount);
                result = Mathf.Clamp(result, min, max);
                return result;
            }
        }

        public ResidentAttribute(Key key, float min, float max, float initialValue)
        {
            this.key = key;
            this.min = min;
            this.max = max;
            this.currentValue = initialValue;
        }

        public ResidentAttribute(Key key, float min, float max, float initialValue, List<Modifier> initialStaticModifiers, List<Modifier> initialTickModifiers)
            : this(key, min, max, initialValue)
        {
            this.staticModifiers = initialStaticModifiers;
            this.tickModifiers = initialTickModifiers;
        }

        public void CalculateTickModifiers()
        {
            tickModifiers.ForEach(modifier => currentValue += modifier.amount);
            currentValue = Mathf.Clamp(currentValue, min, max);
        }

        public class Modifier
        {
            public string name;
            public float amount;

            public Modifier(string name, float amount)
            {
                this.name = name;
                this.amount = amount;
            }
        }
    }
}