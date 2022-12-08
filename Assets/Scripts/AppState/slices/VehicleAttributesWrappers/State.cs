using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.VehicleAttributesWrappers
{
    using VehicleAtributesStateSlice = AttributesStateSlice<
        VehicleAttributesWrapper,
        VehicleAttributesWrapperList,
        VehicleAttribute.Key,
        VehicleAttribute,
        VehicleAttribute.Modifier,
        State.Events
    >;

    public class State : VehicleAtributesStateSlice
    {
        public class Input
        {
            public VehicleAttributesWrapperList vehicleAttributesWrapperList;
        }

        public new class Events : VehicleAtributesStateSlice.Events { }

        public class Queries
        {
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public VehicleAttributesWrapper FindByVehicle(Vehicle vehicle)
            {
                return state.list.FindByVehicle(vehicle);
            }
        }

        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public override void Setup()
        {
            base.Setup();

            appState.Entities.Vehicles.events.onItemsAdded += OnVehiclesAdded;
            appState.Entities.Vehicles.events.onItemsRemoved += OnVehiclesRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.Vehicles.events.onItemsAdded -= OnVehiclesAdded;
            appState.Entities.Vehicles.events.onItemsRemoved -= OnVehiclesRemoved;
        }

        public void AddAttributesWrapperForVehicle(Vehicle vehicle)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = new VehicleAttributesWrapper(appState, vehicle);
            Add(vehicleAttributesWrapper);
        }

        public void RemoveAttributesWrapperForVehicle(Vehicle vehicle)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = queries.FindByVehicle(vehicle);
            Remove(vehicleAttributesWrapper);
        }

        /* 
            Event Handlers
        */
        void OnVehiclesAdded(VehicleList vehicleList)
        {
            vehicleList.ForEach(vehicle =>
            {
                AddAttributesWrapperForVehicle(vehicle);
            });
        }

        void OnVehiclesRemoved(VehicleList vehicleList)
        {
            vehicleList.ForEach(vehicle =>
            {
                RemoveAttributesWrapperForVehicle(vehicle);
            });
        }
    }
}