using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Vehicles;
using UnityEngine;

namespace TowerBuilder.DataTypes.Behaviors.Furnitures
{
    public class CockpitBehavior : FurnitureBehavior
    {
        public override Key key => FurnitureBehavior.Key.Cockpit;

        public CockpitBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        Vehicle vehicle => appState.Entities.Vehicles.queries.FindVehicleByFurniture(furniture);
        VehicleAttributesGroup vehicleAttributesGroup => appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);
        VehicleAttribute.Modifier modifier;

        protected override void OnInteractStart(Resident resident)
        {
            modifier = new VehicleAttribute.Modifier("Piloting", 1f);
            appState.Attributes.Vehicles.AddStaticAttributeModifier(vehicleAttributesGroup, VehicleAttribute.Key.CurrentSpeed, modifier);
        }

        protected override void OnInteractEnd(Resident resident)
        {
            appState.Attributes.Vehicles.RemoveStaticAttributeModifier(vehicleAttributesGroup, VehicleAttribute.Key.CurrentSpeed, modifier);
            modifier = null;
        }
    }
}