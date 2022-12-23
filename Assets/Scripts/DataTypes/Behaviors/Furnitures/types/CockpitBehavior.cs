using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.Vehicles;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Behaviors.Furnitures;
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

        protected override FurnitureBehaviorValidator createValidator() => new CockpitBehaviorValidator(this);

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

        public class CockpitBehaviorValidator : FurnitureBehaviorValidator
        {
            protected override List<ValidationFunc> customValidators => new List<ValidationFunc>()
            {
                ValidateEnginePowerIsNotZero
            };

            public CockpitBehaviorValidator(FurnitureBehavior FurnitureBehavior) : base(FurnitureBehavior) { }

            static ListWrapper<ValidationError> ValidateEnginePowerIsNotZero(AppState appState, FurnitureBehavior furnitureBehavior)
            {
                Furniture furniture = furnitureBehavior.furniture;
                Room room = appState.Entities.Rooms.queries.FindRoomAtCell(furniture.cellCoordinatesList.items[0]);
                Vehicle vehicle = appState.Entities.Vehicles.queries.FindVehicleByRoom(room);
                VehicleAttributesGroup vehicleAttributesGroup = appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);

                VehicleAttribute enginePower = vehicleAttributesGroup.FindByKey(VehicleAttribute.Key.EnginePower);

                if (enginePower.value == 0)
                {
                    return Validator.CreateSingleItemValidationErrorList("Vehicle requires engine power to be piloted.");
                }

                return new ListWrapper<ValidationError>();
            }
        }
    }
}
