using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;
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
                return state.attributesWrapperList.FindByVehicle(vehicle);
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

            appState.Vehicles.events.onVehicleAdded += OnVehicleAdded;
            appState.Vehicles.events.onVehicleRemoved += OnVehicleRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Vehicles.events.onVehicleAdded -= OnVehicleAdded;
            appState.Vehicles.events.onVehicleRemoved -= OnVehicleRemoved;
        }

        public void AddAttributesWrapperForVehicle(Vehicle vehicle)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = new VehicleAttributesWrapper(appState, vehicle);
            AddAttributesWrapper(vehicleAttributesWrapper);
        }

        public void RemoveAttributesWrapperForVehicle(Vehicle vehicle)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = queries.FindByVehicle(vehicle);
            RemoveAttributesWrapper(vehicleAttributesWrapper);
        }

        /* 
            Event Handlers
        */
        void OnVehicleAdded(Vehicle vehicle)
        {
            AddAttributesWrapperForVehicle(vehicle);
        }

        void OnVehicleRemoved(Vehicle vehicle)
        {
            RemoveAttributesWrapperForVehicle(vehicle);
        }
    }
}