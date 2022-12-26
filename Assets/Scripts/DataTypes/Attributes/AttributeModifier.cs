using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class AttributeModifier
    {
        public float value { get; set; }
        public string name { get; }

        public AttributeModifier(string name)
        {
            this.name = name;
        }

        public AttributeModifier(string name, float value) : this(name)
        {
            this.value = value;
        }
    }
}