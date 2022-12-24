using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
    [System.Serializable]
    public abstract class AttributesStateSlice<
        KeyType,
        AttributesGroupType,
        EventsType
    > : ListStateSlice<AttributesGroupType, EventsType>
        where AttributesGroupType : AttributesGroup<KeyType>
        where EventsType : AttributesStateSlice<
            KeyType,
            AttributesGroupType,
            EventsType
        >.Events, new()
    {
        public new class Events : ListStateSlice<AttributesGroupType, EventsType>.Events
        {
            public delegate void AttributesEvent(AttributesGroupType AttributesGroup, Attribute attribute);
            public AttributesEvent onAttributeValueUpdated;

            public delegate void AttributeModifierEvent(AttributesGroupType AttributesGroup, KeyType key, AttributeModifier modifier);

            public AttributeModifierEvent onStaticAttributeModifierAdded;
            public AttributeModifierEvent onStaticAttributeModifierRemoved;
            public AttributeModifierEvent onStaticAttributeModifierUpdated;

            public AttributeModifierEvent onTickAttributeModifierAdded;
            public AttributeModifierEvent onTickAttributeModifierRemoved;
        }

        public AttributesStateSlice(AppState appState) : base(appState)
        {
        }

        public override void Setup()
        {
            base.Setup();
            appState.Time.events.onTick += OnTick;
        }

        public override void Teardown()
        {
            base.Teardown();
            appState.Time.events.onTick -= OnTick;
        }

        public override void Add(AttributesGroupType AttributesGroup)
        {
            AttributesGroup.Setup();
            base.Add(AttributesGroup);
        }

        public override void Remove(AttributesGroupType AttributesGroup)
        {
            AttributesGroup.Teardown();
            base.Remove(AttributesGroup);
        }

        public void AddStaticAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, AttributeModifier modifier)
        {
            Attribute attribute = AttributesGroup.FindByKey(key);

            attribute.staticModifiers.Add(modifier);

            events.onStaticAttributeModifierAdded?.Invoke(AttributesGroup, key, modifier);

            OnPostStaticModifierAdd(AttributesGroup, key, modifier);
        }

        public void RemoveStaticAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, AttributeModifier modifier)
        {
            AttributesGroup.FindByKey(key).staticModifiers.Remove(modifier);

            events.onStaticAttributeModifierRemoved?.Invoke(AttributesGroup, key, modifier);

            OnPostStaticModifierRemove(AttributesGroup, key, modifier);
        }

        public void AddOrUpdateStaticAttributeModifier(AttributesGroupType attributesGroup, KeyType key, string modifierName, float value)
        {
            Debug.Log("AddOrUpdateStaticAttributeModifier");
            Debug.Log("modifierName");
            Debug.Log(modifierName);
            Attribute attribute = attributesGroup.FindByKey(key);

            Debug.Log("finding modiifer");
            AttributeModifier modifier = attribute.staticModifiers.Find((modifier) =>
            {
                Debug.Log("modifier.name");
                Debug.Log(modifier.name);
                return modifier.name == modifierName;
            });

            Debug.Log("modiifer:");
            Debug.Log(modifier);

            if (modifier != null)
            {
                // TODO - probably use an overloaded version of this to avoid re-querying everything
                UpdateStaticAttributeModifier(attributesGroup, key, modifierName, value);
            }
            else
            {
                AttributeModifier newModifier = new AttributeModifier(modifierName, value);
                AddStaticAttributeModifier(attributesGroup, key, newModifier);
            }
        }

        public void UpdateStaticAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, string modifierName, float newValue)
        {
            Attribute attributeToUpdate = AttributesGroup.FindByKey(key);
            AttributeModifier modifierToUpdate = attributeToUpdate.staticModifiers.Find(modifier => modifier.name == modifierName);
            modifierToUpdate.value = newValue;

            events.onStaticAttributeModifierUpdated?.Invoke(AttributesGroup, key, modifierToUpdate);
        }

        public void AddTickAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, AttributeModifier modifier)
        {
            AttributesGroup.FindByKey(key).tickModifiers.Add(modifier);

            events.onTickAttributeModifierAdded?.Invoke(AttributesGroup, key, modifier);
        }

        public void RemoveTickAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, AttributeModifier modifier)
        {
            AttributesGroup.FindByKey(key).tickModifiers.Remove(modifier);

            events.onTickAttributeModifierRemoved?.Invoke(AttributesGroup, key, modifier);
        }

        /*
            Event Handlers
        */
        protected void OnTick(TimeValue time)
        {
            OnPreTick(time);

            list.ForEach((attributesGroup) =>
            {
                attributesGroup.asTupleList.ForEach(tuple =>
                {
                    var (key, attribute) = tuple;
                    attribute.CalculateTickModifiers();
                    events.onAttributeValueUpdated?.Invoke(attributesGroup, attribute);
                });

                ListWrapper<AttributesGroupType> wrapperList = new ListWrapper<AttributesGroupType>(attributesGroup);
                events.onItemsUpdated?.Invoke(wrapperList);
            });

            OnPostTick(time);
        }

        protected virtual void OnPreTick(TimeValue time) { }
        protected virtual void OnPostTick(TimeValue time) { }

        protected virtual void OnPostStaticModifierAdd(AttributesGroupType AttributesGroup, KeyType key, AttributeModifier modifier) { }
        protected virtual void OnPostStaticModifierRemove(AttributesGroupType AttributesGroup, KeyType key, AttributeModifier modifier) { }
    }
}

