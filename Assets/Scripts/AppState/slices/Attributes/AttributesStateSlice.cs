using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
    [Serializable]
    public abstract class AttributesStateSlice<
        KeyType,
        AttributesGroupType,
        AttributeType,
        AttributeModifierType,
        EventsType
    > : ListStateSlice<AttributesGroupType, EventsType>
        where AttributeType : Attribute<KeyType>
        where AttributeModifierType : Attribute<KeyType>.Modifier
        where AttributesGroupType : AttributesGroup<AttributeType, KeyType>
        where EventsType : AttributesStateSlice<
            KeyType,
            AttributesGroupType,
            AttributeType,
            AttributeModifierType,
            EventsType
        >.Events, new()
    {
        public new class Events : ListStateSlice<AttributesGroupType, EventsType>.Events
        {
            public delegate void AttributesEvent(AttributesGroupType AttributesGroup, AttributeType attribute);
            public AttributesEvent onAttributeValueUpdated;

            public delegate void AttributeModifierEvent(AttributesGroupType AttributesGroup, KeyType key, AttributeModifierType modifier);
            public AttributeModifierEvent onStaticAttributeModifierAdded;
            public AttributeModifierEvent onStaticAttributeModifierRemoved;
            public AttributeModifierEvent onTickAttributeModifierAdded;
            public AttributeModifierEvent onTickAttributeModifierRemoved;
        }

        public AttributesStateSlice(AppState appState) : base(appState)
        {
            Setup();
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

        public void AddStaticAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, AttributeModifierType modifier)
        {
            AttributesGroup.FindByKey(key).staticModifiers.Add(modifier);

            events.onStaticAttributeModifierAdded?.Invoke(AttributesGroup, key, modifier);
        }

        public void RemoveStaticAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, AttributeModifierType modifier)
        {
            AttributesGroup.FindByKey(key).staticModifiers.Remove(modifier);

            events.onStaticAttributeModifierRemoved?.Invoke(AttributesGroup, key, modifier);
        }

        public void AddTickAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, AttributeModifierType modifier)
        {
            AttributesGroup.FindByKey(key).tickModifiers.Add(modifier);

            events.onTickAttributeModifierAdded?.Invoke(AttributesGroup, key, modifier);
        }

        public void RemoveTickAttributeModifier(AttributesGroupType AttributesGroup, KeyType key, AttributeModifierType modifier)
        {
            AttributesGroup.FindByKey(key).tickModifiers.Remove(modifier);

            events.onTickAttributeModifierRemoved?.Invoke(AttributesGroup, key, modifier);
        }

        /*
            Event Handlers
        */
        protected void OnTick(TimeValue time)
        {
            list.ForEach((AttributesGroup) =>
            {
                AttributesGroup.attributes.ForEach(attribute =>
                {
                    attribute.CalculateTickModifiers();
                    events.onAttributeValueUpdated?.Invoke(AttributesGroup, attribute);
                });

                ListWrapper<AttributesGroupType> wrapperList = new ListWrapper<AttributesGroupType>();
                wrapperList.Add(AttributesGroup);
                events.onItemsUpdated?.Invoke(wrapperList);
            });
        }
    }
}

