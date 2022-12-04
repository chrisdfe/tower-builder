using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
    [Serializable]
    public abstract class AttributesStateSlice<
        AttributesWrapperType,
        AttributesWrapperListType,
        KeyType,
        AttributeType,
        AttributeModifierType,
        EventsType
    > : ListStateSlice<AttributesWrapperListType, AttributesWrapperType, EventsType>
        where AttributeType : Attribute<KeyType>
        where AttributeModifierType : Attribute<KeyType>.Modifier
        where AttributesWrapperType : AttributesWrapper<AttributeType, KeyType>
        where AttributesWrapperListType : ListWrapper<AttributesWrapperType>, new()
        where EventsType : AttributesStateSlice<
            AttributesWrapperType,
            AttributesWrapperListType,
            KeyType,
            AttributeType,
            AttributeModifierType,
            EventsType
        >.Events, new()
    {
        public new class Events : ListStateSlice<AttributesWrapperListType, AttributesWrapperType, EventsType>.Events
        {
            public delegate void AttributesWrapperEvent(AttributesWrapperType attributesWrapper);
            public AttributesWrapperEvent onAttributesWrapperAdded;
            public AttributesWrapperEvent onAttributesWrapperRemoved;
            public AttributesWrapperEvent onAttributesWrapperUpdated;

            public delegate void AttributesEvent(AttributesWrapperType attributesWrapper, AttributeType attribute);
            public AttributesEvent onAttributeValueUpdated;

            public delegate void AttributeModifierEvent(AttributesWrapperType attributesWrapper, KeyType key, AttributeModifierType modifier);
            public AttributeModifierEvent onStaticAttributeModifierAdded;
            public AttributeModifierEvent onStaticAttributeModifierRemoved;
            public AttributeModifierEvent onTickAttributeModifierAdded;
            public AttributeModifierEvent onTickAttributeModifierRemoved;
        }

        public AttributesStateSlice(AppState appState) : base(appState)
        {
            Setup();
        }

        public virtual void Setup()
        {
            appState.Time.events.onTick += OnTick;
        }

        public virtual void Teardown()
        {
            appState.Time.events.onTick -= OnTick;
        }

        public override void Add(AttributesWrapperType attributesWrapper)
        {
            attributesWrapper.Setup();
            base.Add(attributesWrapper);
        }

        public override void Remove(AttributesWrapperType attributesWrapper)
        {
            attributesWrapper.Teardown();
            base.Remove(attributesWrapper);
        }

        public void AddStaticAttributeModifier(AttributesWrapperType attributesWrapper, KeyType key, AttributeModifierType modifier)
        {
            attributesWrapper.FindByKey(key).staticModifiers.Add(modifier);

            events.onStaticAttributeModifierAdded?.Invoke(attributesWrapper, key, modifier);
        }

        public void RemoveStaticAttributeModifier(AttributesWrapperType attributesWrapper, KeyType key, AttributeModifierType modifier)
        {
            attributesWrapper.FindByKey(key).staticModifiers.Remove(modifier);

            events.onStaticAttributeModifierRemoved?.Invoke(attributesWrapper, key, modifier);
        }

        public void AddTickAttributeModifier(AttributesWrapperType attributesWrapper, KeyType key, AttributeModifierType modifier)
        {
            attributesWrapper.FindByKey(key).tickModifiers.Add(modifier);

            events.onTickAttributeModifierAdded?.Invoke(attributesWrapper, key, modifier);
        }

        public void RemoveTickAttributeModifier(AttributesWrapperType attributesWrapper, KeyType key, AttributeModifierType modifier)
        {
            attributesWrapper.FindByKey(key).tickModifiers.Remove(modifier);

            events.onTickAttributeModifierRemoved?.Invoke(attributesWrapper, key, modifier);
        }

        /*
            Event Handlers
        */
        protected void OnTick(TimeValue time)
        {
            list.ForEach((attributesWrapper) =>
            {
                attributesWrapper.attributes.ForEach(attribute =>
                {
                    attribute.CalculateTickModifiers();
                    events.onAttributeValueUpdated?.Invoke(attributesWrapper, attribute);
                });

                events.onAttributesWrapperUpdated?.Invoke(attributesWrapper);
            });
        }
    }
}

