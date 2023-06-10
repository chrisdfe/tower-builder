using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class AttributesGroup
    {
        public virtual Dictionary<string, Attribute> attributes { get; } = new Dictionary<string, Attribute>();

        public List<Attribute> list => attributes.Values.ToList();

        public List<(string, Attribute)> asTupleList
        {
            get
            {
                List<(string, Attribute)> result = new List<(string, Attribute)>();

                foreach (string key in attributes.Keys)
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

        public Attribute FindByKey(string key) => attributes.GetValueOrDefault(key);

        public virtual void CalculateDerivedAttributes(AppState appState) { }
    }
}