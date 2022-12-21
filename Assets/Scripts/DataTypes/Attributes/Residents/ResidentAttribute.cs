using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes.Attributes.Residents
{
    public class ResidentAttribute : Attribute<ResidentAttribute.Key>
    {
        public enum Key
        {
            Energy
        }

        public ResidentAttribute(Key key) : base(key) { }

        public ResidentAttribute(Key key, List<Modifier> initialStaticModifiers, List<Modifier> initialTickModifiers)
            : base(key, initialTickModifiers, initialTickModifiers) { }
    }
}