using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.Vehicles;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Attributes.Vehicles
{
    using VehicleAtributesStateSlice = AttributesStateSlice<
        VehicleAttribute.Key,
        VehicleAttributesGroup,
        VehicleAttribute,
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
                state.list.Find(attributesGroup => attributesGroup.vehicle == vehicle);

            public ListWrapper<VehicleAttribute> FindByKey(VehicleAttribute.Key key) =>
                new ListWrapper<VehicleAttribute>(
                    state.list.items
                        .Select(attributesGroup => attributesGroup.FindByKey(key))
                        .ToList()
                );
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

        protected override void OnPostTick(TimeValue time)
        {
            if (list.Count == 0) return;

            // Update current speed

            // Update journey progress counter
            ListWrapper<VehicleAttribute> currentSpeedAttributes = queries.FindByKey(VehicleAttribute.Key.CurrentSpeed);

            VehicleAttribute currentSpeedAttribute = currentSpeedAttributes.items[0];
            appState.Journeys.UpdateJourneyProgress(currentSpeedAttribute.value);
        }

        protected override void OnPostStaticModifierAdd(VehicleAttributesGroup attributesGroup, VehicleAttribute.Key key, AttributeModifier modifier)
        {
            if (key == VehicleAttribute.Key.IsPiloted)
            {
                VehicleAttribute enginePowerAttribute = attributesGroup.FindByKey(VehicleAttribute.Key.EnginePower);
                VehicleAttribute currentSpeedAttribute = attributesGroup.FindByKey(VehicleAttribute.Key.CurrentSpeed);
                float enginePowerAmount = enginePowerAttribute.value;

                // for now currentSpeed == enginePower
                if (currentSpeedAttribute.value != currentSpeedAttribute.value)
                {

                }
            }
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