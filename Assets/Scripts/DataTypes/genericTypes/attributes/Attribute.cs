using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Attribute<KeyType>
    {
        public KeyType key;

        public List<Modifier> staticModifiers { get; private set; } = new List<Modifier>();
        public List<Modifier> tickModifiers { get; private set; } = new List<Modifier>();

        public float min { get; private set; } = 0;
        public float max { get; private set; } = 100;

        float currentValue = 100;
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

        public Attribute(KeyType key, List<Modifier> initialStaticModifiers, List<Modifier> initialTickModifiers)
            : this(key)
        {
            this.staticModifiers = initialStaticModifiers;
            this.tickModifiers = initialTickModifiers;
        }

        public Attribute(KeyType key, float initialValue, float min, float max, List<Modifier> initialStaticModifiers, List<Modifier> initialTickModifiers)
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