using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class AttributesGroup<KeyType>
    {
        public virtual Dictionary<KeyType, Attribute> attributes { get; } = new Dictionary<KeyType, Attribute>();

        public List<(KeyType, Attribute)> asTupleList
        {
            get
            {
                List<(KeyType, Attribute)> result = new List<(KeyType, Attribute)>();

                foreach (KeyType key in attributes.Keys)
                {
                    result.Add((key, attributes[key]));
                }

                return result;
            }
        }

        protected AppState appState;

        public AttributesGroup(AppState appState)
        {
            this.appState = appState;
        }

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public Attribute FindByKey(KeyType key) => attributes.GetValueOrDefault(key);

        public virtual void CalculateDerivedAttributes(AppState appState) { }
    }
}