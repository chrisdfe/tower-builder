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
    > : StateSlice
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
        public class Events
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

        public AttributesWrapperListType attributesWrapperList { get; private set; } = new AttributesWrapperListType();

        public EventsType events { get; private set; }

        public AttributesStateSlice(AppState appState) : base(appState)
        {
            events = new EventsType();

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

        public void AddAttributesWrapper(AttributesWrapperType attributesWrapper)
        {
            attributesWrapperList.Add(attributesWrapper);
            attributesWrapper.Setup();
            events.onAttributesWrapperAdded?.Invoke(attributesWrapper);
        }

        public void RemoveAttributesWrapper(AttributesWrapperType attributesWrapper)
        {
            attributesWrapperList.Remove(attributesWrapper);
            attributesWrapper.Teardown();
            events.onAttributesWrapperRemoved?.Invoke(attributesWrapper);
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
            attributesWrapperList.ForEach((attributesWrapper) =>
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

