using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Vehicles;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Attributes.Vehicles
{
    using VehicleAttributesStateSlice = AttributesStateSlice<
        VehicleAttributes.Key,
        VehicleAttributes,
        State.Events
    >;

    public class State : VehicleAttributesStateSlice
    {
        public class Input
        {
            public ListWrapper<Attribute> vehicleAttributesList;
        }

        public new class Events : VehicleAttributesStateSlice.Events
        {
            public ItemEvent<VehicleAttributes> onVehicleDerivedAttributesRecalculated;
        }

        public class Queries
        {
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public VehicleAttributes FindByVehicle(Vehicle vehicle) =>
                state.list.Find(attributesGroup => attributesGroup.vehicle == vehicle);

            public ListWrapper<Attribute> FindByKey(VehicleAttributes.Key key) =>
                new ListWrapper<Attribute>(
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

            // appState.Entities.Vehicles.events.onItemsAdded += OnVehiclesAdded;
            // appState.Entities.Vehicles.events.onItemsRemoved += OnVehiclesRemoved;
            // appState.Entities.Vehicles.events.onVehicleIsPilotedUpdated += OnVehicleIsPilotedUpdated;
        }

        public override void Teardown()
        {
            base.Teardown();

            // appState.Entities.Vehicles.events.onItemsAdded -= OnVehiclesAdded;
            // appState.Entities.Vehicles.events.onItemsRemoved -= OnVehiclesRemoved;
            // appState.Entities.Vehicles.events.onVehicleIsPilotedUpdated -= OnVehicleIsPilotedUpdated;
        }

        // public void AddAttributesGroupForVehicle(Vehicle vehicle)
        // {
        //     VehicleAttributes vehicleAttributes = new VehicleAttributes(appState, vehicle);
        //     Add(vehicleAttributes);
        // }

        // public void RemoveAttributesGroupForVehicle(Vehicle vehicle)
        // {
        //     VehicleAttributes vehicleAttributes = queries.FindByVehicle(vehicle);
        //     Remove(vehicleAttributes);
        // }

        // protected override void OnPostTick(TimeValue time)
        // {
        //     if (list.Count == 0) return;

        //     // Update journey progress counter
        //     // TODO - use convoy speed instead of first vehicle only
        //     VehicleAttributes vehicleAttributes = list.items[0];

        //     appState.Journeys.UpdateJourneyProgress(vehicleAttributes.currentSpeed);
        // }

        // protected override void OnPostStaticModifierAdd(VehicleAttributes vehicleAttributes, VehicleAttributes.Key key, AttributeModifier modifier)
        // {
        //     if (key == VehicleAttributes.Key.EnginePower)
        //     {
        //         CalculateDerivedAttributes(vehicleAttributes);
        //     }
        // }

        // /* 
        //     Internals
        // */
        // void CalculateDerivedAttributes(VehicleAttributes vehicleAttributes)
        // {
        //     vehicleAttributes.CalculateDerivedAttributes(appState);
        //     events.onVehicleDerivedAttributesRecalculated?.Invoke(vehicleAttributes);
        // }

        // /* 
        //     Event Handlers
        // */
        // void OnVehiclesAdded(ListWrapper<Vehicle> vehicleList)
        // {
        //     vehicleList.ForEach(vehicle =>
        //     {
        //         AddAttributesGroupForVehicle(vehicle);
        //     });
        // }

        // void OnVehiclesRemoved(ListWrapper<Vehicle> vehicleList)
        // {
        //     vehicleList.ForEach(vehicle =>
        //     {
        //         RemoveAttributesGroupForVehicle(vehicle);
        //     });
        // }

        // void OnVehicleIsPilotedUpdated(Vehicle vehicle)
        // {
        //     VehicleAttributes attributes = queries.FindByVehicle(vehicle);
        //     CalculateDerivedAttributes(attributes);
        // }
    }
}