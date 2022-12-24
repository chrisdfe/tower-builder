using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class AttributeModifier
    {
        public string name;
        public float amount;

        public AttributeModifier(string name, float amount)
        {
            this.name = name;
            this.amount = amount;
        }
    }
}