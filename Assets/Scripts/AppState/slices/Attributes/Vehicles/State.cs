using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Attributes.Vehicles
{
    using VehicleAtributesStateSlice = AttributesStateSlice<
        VehicleAttribute.Key,
        VehicleAttributesGroup,
        VehicleAttribute,
        VehicleAttribute.Modifier,
        State.Events
    >;

    public class State : VehicleAtributesStateSlice
    {
        public class Input
        {
            public ListWrapper<VehicleAttributesGroup> vehicleAttributesGroupList;
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

            public VehicleAttributesGroup FindByVehicle(Vehicle vehicle) =>
                state.list.Find(AttributesGroup => AttributesGroup.vehicle == vehicle);
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

            appState.Vehicles.events.onItemsAdded += OnVehiclesAdded;
            appState.Vehicles.events.onItemsRemoved += OnVehiclesRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Vehicles.events.onItemsAdded -= OnVehiclesAdded;
            appState.Vehicles.events.onItemsRemoved -= OnVehiclesRemoved;
        }

        public void AddAttributesGroupForVehicle(Vehicle vehicle)
        {
            VehicleAttributesGroup vehicleAttributesGroup = new VehicleAttributesGroup(appState, vehicle);
            Add(vehicleAttributesGroup);
        }

        public void RemoveAttributesGroupForVehicle(Vehicle vehicle)
        {
            VehicleAttributesGroup vehicleAttributesGroup = queries.FindByVehicle(vehicle);
            Remove(vehicleAttributesGroup);
        }

        /* 
            Event Handlers
        */
        void OnVehiclesAdded(ListWrapper<Vehicle> vehicleList)
        {
            vehicleList.ForEach(vehicle =>
            {
                AddAttributesGroupForVehicle(vehicle);
            });
        }

        void OnVehiclesRemoved(ListWrapper<Vehicle> vehicleList)
        {
            vehicleList.ForEach(vehicle =>
            {
                RemoveAttributesGroupForVehicle(vehicle);
            });
        }
    }
}