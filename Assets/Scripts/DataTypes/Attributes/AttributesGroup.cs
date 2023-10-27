using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;

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

        public AttributesGroup() { }

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public Attribute FindByKey(string key) => attributes.GetValueOrDefault(key);

        public virtual void CalculateDerivedAttributes(AppState appState) { }


        public void AddStaticAttributeModifier(string key, AttributeModifier modifier)
        {
            Attribute attribute = FindByKey(key);
            attribute.staticModifiers.Add(modifier);
        }

        public void RemoveStaticAttributeModifier(string key, AttributeModifier modifier)
        {
            Attribute attribute = FindByKey(key);
            attribute.staticModifiers.Remove(modifier);
        }

        public void AddOrUpdateStaticAttributeModifier(string key, string modifierName, float value)
        {
            Attribute attribute = FindByKey(key);

            AttributeModifier modifier = attribute.staticModifiers.Find((modifier) =>
                modifier.name == modifierName
            );

            if (modifier != null)
            {
                // TODO - probably use an overloaded version of this to avoid re-querying everything
                UpdateStaticAttributeModifier(key, modifierName, value);
            }
            else
            {
                AttributeModifier newModifier = new AttributeModifier(modifierName, value);
                AddStaticAttributeModifier(key, newModifier);
            }
        }

        public void UpdateStaticAttributeModifier(string key, string modifierName, float newValue)
        {
            Attribute attributeToUpdate = FindByKey(key);
            AttributeModifier modifierToUpdate = attributeToUpdate.staticModifiers.Find(modifier => modifier.name == modifierName);
            modifierToUpdate.value = newValue;
        }

        public void AddTickAttributeModifier(string key, AttributeModifier modifier)
        {
            Attribute attribute = FindByKey(key);
            attribute.tickModifiers.Add(modifier);
        }

        public void RemoveTickAttributeModifier(string key, AttributeModifier modifier)
        {
            Attribute attribute = FindByKey(key);
            attribute.tickModifiers.Remove(modifier);
        }
    }
}