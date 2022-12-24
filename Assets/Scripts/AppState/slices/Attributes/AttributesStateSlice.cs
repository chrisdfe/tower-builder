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
        EventsType
    > : ListStateSlice<AttributesGroupType, EventsType>
        where AttributeType : Attribute<KeyType>
        where AttributesGroupType : AttributesGroup<AttributeType, KeyType>
        where EventsType : AttributesStateSlice<
            KeyType,
            AttributesGroupType,
            AttributeType,
            EventsType
        >.Events, new()
    {
        public new class Events : ListStateSlice<AttributesGroupType, EventsType>.Events
        {
            public delegate void AttributesEvent(AttributesGroupType AttributesGroup, AttributeType attribute);
            public AttributesEvent onAttributeValueUpdated;

            public delegate void AttributeModifierEvent(AttributesGroupType AttributesGroup, KeyType key, AttributeModifier modifier);
            public AttributeModifierEvent onStaticAttributeModifierAdded;
            public AttributeModifierEvent onStaticAttributeModifierRemoved;
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
            AttributeType attribute = AttributesGroup.FindByKey(key);

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
                attributesGroup.attributes.ForEach(attribute =>
                {
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

