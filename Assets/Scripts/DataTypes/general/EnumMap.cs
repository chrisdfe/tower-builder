using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [Serializable]
    public class EnumMap<EnumType, ValueType>
    {
        public Dictionary<EnumType, ValueType> map = new Dictionary<EnumType, ValueType>();

        public List<EnumType> keys => map.Keys.ToList();

        public List<ValueType> values => map.Values.ToList();

        public EnumMap() { }

        public EnumMap(Dictionary<EnumType, ValueType> map)
        {
            this.map = map;
        }

        public EnumType KeyFromValue(ValueType label) =>
            map
                .Where(kv => kv.Value.Equals(label))
                .Select(kv => kv.Key)
                .DefaultIfEmpty() // or no argument -> null
                .First();

        public ValueType ValueFromKey(EnumType cellPosition) =>
            map[cellPosition];
    }
}