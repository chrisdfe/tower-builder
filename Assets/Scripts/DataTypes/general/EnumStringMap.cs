using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [Serializable]
    public class EnumStringMap<EnumType> : EnumMap<EnumType, string>
    {
        public EnumStringMap(Dictionary<EnumType, string> map) : base(map)
        {
        }
    }
}