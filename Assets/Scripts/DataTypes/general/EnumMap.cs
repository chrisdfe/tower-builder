using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.DataTypes
{
    public class EnumMap<EnumType, ValueType>
    {
        public EnumType enumType;

        public Dictionary<EnumType, ValueType> map { get; }

        public List<EnumType> keys => map.Keys.ToList();

        public List<ValueType> labels => map.Values.ToList();

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