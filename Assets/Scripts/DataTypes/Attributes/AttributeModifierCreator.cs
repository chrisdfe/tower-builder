using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class AttributeModifierBundle
    {
        // public IAttributesGroup attributesGroup;
        public AttributeModifier attributeModifier;
    }

    public class AttributeModifierCreator<KeyType>
        where KeyType : struct
    {
        public Entity entity;

        public delegate AttributeModifierBundle ModifierBundleCreator(AppState appState);
        public List<AttributeModifierBundle> modifierBundleCreator = new List<AttributeModifierBundle>();
    }
}