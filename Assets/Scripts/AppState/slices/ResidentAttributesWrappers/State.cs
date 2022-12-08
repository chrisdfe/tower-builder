using System;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Residents.Attributes;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Routes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.ResidentAttributesWrappers
{
    using ResidentAtributesStateSlice = AttributesStateSlice<
        ResidentAttributesWrapper,
        ResidentAttributesWrapperList,
        ResidentAttribute.Key,
        ResidentAttribute,
        ResidentAttribute.Modifier,
        State.Events
    >;

    public class State : ResidentAtributesStateSlice
    {
        public class Input { }

        public new class Events : ResidentAtributesStateSlice.Events { }

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
                return state.list.FindByResident(resident);
            }
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);

            Setup();
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public override void Setup()
        {
            base.Setup();

            appState.Entities.Residents.events.onItemsAdded += OnResidentsAdded;
            appState.Entities.Residents.events.onItemsRemoved += OnResidentsRemoved;
            appState.Entities.Residents.events.onItemsBuilt += OnResidentsBuilt;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.Residents.events.onItemsAdded -= OnResidentsAdded;
            appState.Entities.Residents.events.onItemsRemoved -= OnResidentsRemoved;
            appState.Entities.Residents.events.onItemsBuilt -= OnResidentsBuilt;
        }

        /* 
            Public Interface
         */
        public void AddAttributesForResident(Resident resident)
        {
            ResidentAttributesWrapper residentAttributesWrapper = new ResidentAttributesWrapper(appState, resident);
            Add(residentAttributesWrapper);
        }

        public void RemoveAttributesForResident(Resident resident)
        {
            ResidentAttributesWrapper residentAttributesWrapper = list.FindByResident(resident);

            if (residentAttributesWrapper != null)
            {
                Remove(residentAttributesWrapper);
            }
        }

        /* 
            Event handlers
         */
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