using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.DataTypes
{
    public class EnumStringMap<EnumType>
    {
        public EnumType enumType;

        public Dictionary<EnumType, string> map { get; }

        public List<EnumType> keys => map.Keys.ToList();

        public List<string> labels => map.Values.ToList();

        public EnumStringMap(Dictionary<EnumType, string> map)
        {
            this.map = map;
        }

        public EnumType KeyFromValue(string label) =>
            map
                .Where(kv => kv.Value == label)
                .Select(kv => kv.Key)
                .DefaultIfEmpty() // or no argument -> null
                .First();

        public string ValueFromKey(EnumType cellPosition) =>
            map[cellPosition];
    }
}