using System;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Attributes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Routes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.ResidentAttributesWrappers
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {
            public delegate void ResidentAttributesWrapperEvent(ResidentAttributesWrapper residentAttributesWrapper);
            public ResidentAttributesWrapperEvent onResidentAttributesWrapperAdded;
            public ResidentAttributesWrapperEvent onResidentAttributesWrapperRemoved;

            public delegate void ResidentAttributeModifierEvent(Resident resident, ResidentAttribute.Key key, ResidentAttribute.Modifier modifier);
            public ResidentAttributeModifierEvent onResidentStaticAttributeModifierAdded;
            public ResidentAttributeModifierEvent onResidentStaticAttributeModifierRemoved;
            public ResidentAttributeModifierEvent onResidentTickAttributeModifierAdded;
            public ResidentAttributeModifierEvent onResidentTickAttributeModifierRemoved;

            public delegate void ResidentAttributeEvent(Resident resident, ResidentAttribute attribute);
            public ResidentAttributeEvent onResidentAttributeValueUpdated;
        }

        public class Queries
        {
            State state;
            AppState appState;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public ResidentAttributesWrapper FindByResident(Resident resident)
            {
                return state.residentAttributesWrapperList.FindByResident(resident);
            }
        }

        public ResidentAttributesWrapperList residentAttributesWrapperList { get; private set; } = new ResidentAttributesWrapperList();

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            events = new Events();
            queries = new Queries(appState, this);

            Setup();
        }

        public void Setup()
        {
            appState.Time.events.onTick += OnTick;

            appState.Residents.events.onResidentsAdded += OnResidentsAdded;
            appState.Residents.events.onResidentsRemoved += OnResidentsRemoved;
            appState.Residents.events.onResidentsBuilt += OnResidentsBuilt;
        }

        public void Teardown()
        {
            appState.Time.events.onTick -= OnTick;

            appState.Residents.events.onResidentsAdded -= OnResidentsAdded;
            appState.Residents.events.onResidentsRemoved -= OnResidentsRemoved;
            appState.Residents.events.onResidentsBuilt -= OnResidentsBuilt;
        }

        /* 
            Public Interface
         */
        public void AddAttributesForResident(Resident resident)
        {
            ResidentAttributesWrapper ResidentAttributesWrapper = new ResidentAttributesWrapper(resident);
            ResidentAttributesWrapper.Setup();
            AddResidentAttributesWrapper(ResidentAttributesWrapper);
        }

        public void RemoveAttributesForResident(Resident resident)
        {
            ResidentAttributesWrapper ResidentAttributesWrapper = residentAttributesWrapperList.FindByResident(resident);

            if (ResidentAttributesWrapper != null)
            {
                RemoveResidentAttributesWrapper(ResidentAttributesWrapper);
            }
        }

        public void AddResidentAttributesWrapper(ResidentAttributesWrapper ResidentAttributesWrapper)
        {
            residentAttributesWrapperList.Add(ResidentAttributesWrapper);

            if (events.onResidentAttributesWrapperAdded != null)
            {
                events.onResidentAttributesWrapperAdded(ResidentAttributesWrapper);
            }
        }

        public void RemoveResidentAttributesWrapper(ResidentAttributesWrapper ResidentAttributesWrapper)
        {
            residentAttributesWrapperList.Remove(ResidentAttributesWrapper);

            if (events.onResidentAttributesWrapperRemoved != null)
            {
                events.onResidentAttributesWrapperRemoved(ResidentAttributesWrapper);
            }
        }

        public void AddStaticAttributeModifier(Resident resident, ResidentAttribute.Key key, ResidentAttribute.Modifier modifier)
        {
            ResidentAttributesWrapper residentAttributesWrapper = residentAttributesWrapperList.FindByResident(resident);
            residentAttributesWrapper.FindByKey(key).staticModifiers.Add(modifier);

            if (events.onResidentStaticAttributeModifierAdded != null)
            {
                events.onResidentStaticAttributeModifierAdded(resident, key, modifier);
            }
        }

        public void RemoveStaticAttributeModifier(Resident resident, ResidentAttribute.Key key, ResidentAttribute.Modifier modifier)
        {
            ResidentAttributesWrapper residentAttributesWrapper = residentAttributesWrapperList.FindByResident(resident);
            residentAttributesWrapper.FindByKey(key).staticModifiers.Remove(modifier);

            if (events.onResidentStaticAttributeModifierRemoved != null)
            {
                events.onResidentStaticAttributeModifierRemoved(resident, key, modifier);
            }
        }

        public void AddTickAttributeModifier(Resident resident, ResidentAttribute.Key key, ResidentAttribute.Modifier modifier)
        {
            ResidentAttributesWrapper residentAttributesWrapper = residentAttributesWrapperList.FindByResident(resident);
            residentAttributesWrapper.FindByKey(key).tickModifiers.Add(modifier);

            if (events.onResidentTickAttributeModifierAdded != null)
            {
                events.onResidentTickAttributeModifierAdded(resident, key, modifier);
            }
        }

        public void RemoveTickAttributeModifier(Resident resident, ResidentAttribute.Key key, ResidentAttribute.Modifier modifier)
        {
            ResidentAttributesWrapper residentAttributesWrapper = residentAttributesWrapperList.FindByResident(resident);
            residentAttributesWrapper.FindByKey(key).tickModifiers.Remove(modifier);

            if (events.onResidentTickAttributeModifierRemoved != null)
            {
                events.onResidentTickAttributeModifierRemoved(resident, key, modifier);
            }
        }

        /* 
            Event handlers
         */
        void OnTick(TimeValue time)
        {
            residentAttributesWrapperList.ForEach((residentAttributesWrapper) =>
            {
                residentAttributesWrapper.attributes.ForEach(attribute =>
                {
                    attribute.CalculateTickModifiers();

                    if (events.onResidentAttributeValueUpdated != null)
                    {
                        events.onResidentAttributeValueUpdated(residentAttributesWrapper.resident, attribute);
                    }
                });
            });
        }

        void OnResidentsAdded(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                if (!resident.isInBlueprintMode)
                {
                    AddAttributesForResident(resident);
                }
            }
        }

        void OnResidentsBuilt(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                AddAttributesForResident(resident);
            }
        }

        void OnResidentsRemoved(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                RemoveAttributesForResident(resident);
            }
        }
    }
}