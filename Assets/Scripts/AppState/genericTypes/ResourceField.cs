using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Newtonsoft.Json;

namespace TowerBuilder.DataTypes
{
    public class ResourceField<TValue> where TValue : IEquatable<TValue>
    {
        public delegate void ValueChangedEvent(TValue value);
        public ValueChangedEvent onValueChanged;

        TValue _value;
        public TValue value
        {
            get => _value;
            set
            {
                // Using this instead of != to avoid generic ambiguity
                // between reference + value types
                if (!EqualityComparer<TValue>.Default.Equals(value, _value))
                {
                    _value = value;

                    if (onValueChanged != null)
                    {
                        onValueChanged(_value);
                    }
                }
            }
        }

        public ResourceField(TValue value)
        {
            _value = value;
        }

        public ResourceField() { }
    }
}