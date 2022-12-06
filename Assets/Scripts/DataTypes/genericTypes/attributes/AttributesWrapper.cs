using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class AttributesWrapper<AttributeType, KeyType>
        where AttributeType : Attribute<KeyType>
    {
        public virtual List<AttributeType> attributes { get; } = new List<AttributeType>();

        protected AppState appState;

        public AttributesWrapper(AppState appState)
        {
            this.appState = appState;
        }

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public AttributeType FindByKey(KeyType key) => attributes.Find(attribute => attribute.key.Equals(key));
    }
}