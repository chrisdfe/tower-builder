using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.DataTypes
{
    public class EnumStringMap<EnumType> : EnumMap<EnumType, string>
    {
        public EnumStringMap(Dictionary<EnumType, string> map) : base(map)
        {
        }
    }
}