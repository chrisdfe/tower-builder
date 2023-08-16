using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Vehicles;
using UnityEngine;

namespace TowerBuilder.DataTypes.Behaviors.Furnitures
{
    public class CockpitBehavior : FurnitureBehavior
    {
        public override Key key => FurnitureBehavior.Key.Cockpit;

        public CockpitBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        // Vehicle vehicle => appState.Entities.Vehicles.queries.FindVehicleByFurniture(furniture);
        // VehicleAttributes vehicleAttributesGroup => appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);
        AttributeModifier modifier;

        protected override Validator<FurnitureBehavior> createValidator() => new CockpitBehaviorValidator(this);

        protected override void OnInteractStart(Resident resident)
        {
            // modifier = new BoolAttributeModifier("Piloting", 1f);
            // appState.Attributes.Vehicles.AddStaticAttributeModifier(vehicleAttributesGroup, VehicleAttributes.Key.IsPiloted, modifier);
            // appState.Entities.Vehicles.SetVehicleIsPiloted(vehicle, true);
        }

        protected override void OnInteractEnd(Resident resident)
        {
            // TODO - support multiple pilot seats by checking that other CockpitBehaviors exist first
            // appState.Entities.Vehicles.SetVehicleIsPiloted(vehicle, false);
            modifier = null;
        }

        public class CockpitBehaviorValidator : Validator<FurnitureBehavior>
        {
            protected override List<ValidationFunc> customValidators => new List<ValidationFunc>()
            {
                ValidateEnginePowerIsNotZero
            };

            public CockpitBehaviorValidator(FurnitureBehavior FurnitureBehavior) : base(FurnitureBehavior) { }

            static List<ValidationError> ValidateEnginePowerIsNotZero(AppState appState, FurnitureBehavior furnitureBehavior)
            {
                // Furniture furniture = furnitureBehavior.furniture;
                // Room room = appState.Entities.Rooms.queries.FindRoomAtCell(furniture.cellCoordinatesList.items[0]);
                // Vehicle vehicle = appState.Entities.Vehicles.queries.FindVehicleByRoom(room);
                // VehicleAttributes vehicleAttributes = appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);

                // Attribute enginePower = vehicleAttributes.FindByKey(VehicleAttributes.Key.EnginePower);

                // if (enginePower.value == 0)
                // {
                //     return Validator.CreateSingleItemValidationErrorList("Vehicle requires engine power to be piloted.");
                // }

                return new List<ValidationError>();
            }
        }
    }
}
