using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Vehicles;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Attributes.Vehicles
{
    using VehicleAttributesStateSlice = AttributesStateSlice<VehicleAttributes>;

    public class State : VehicleAttributesStateSlice
    {
        public class Input
        {
            public ListWrapper<Attribute> vehicleAttributesList;
        }

        public ItemEvent<VehicleAttributes> onVehicleDerivedAttributesRecalculated;


        public State(AppState appState, Input input) : base(appState)
        {
        }

        public State(AppState appState) : this(appState, new Input()) { }

        /*
            Lifecycle
        */
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

        /*
            Queries
        */
        public VehicleAttributes FindByVehicle(Vehicle vehicle) =>
            list.Find(attributesGroup => attributesGroup.vehicle == vehicle);

        public ListWrapper<Attribute> FindByKey(string key) =>
            new ListWrapper<Attribute>(
                list.items
                    .Select(attributesGroup => attributesGroup.FindByKey(key))
                    .ToList()
            );

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