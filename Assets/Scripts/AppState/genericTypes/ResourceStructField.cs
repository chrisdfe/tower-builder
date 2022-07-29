using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Newtonsoft.Json;

namespace TowerBuilder.DataTypes
{
    public class ResourceStructField<TValue> where TValue : struct
    {
        public delegate void ValueChangedEvent(TValue value, TValue previousValue);
        public ValueChangedEvent onValueChanged;

        TValue _previousValue;
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
                    _previousValue = _value;
                    _value = value;

                    if (onValueChanged != null)
                    {
                        onValueChanged(_value, _previousValue);
                    }
                }
            }
        }

        public ResourceStructField(TValue value)
        {
            _value = value;
        }
    }
}