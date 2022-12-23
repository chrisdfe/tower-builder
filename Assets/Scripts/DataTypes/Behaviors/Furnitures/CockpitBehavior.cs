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
    public class CockpitBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Cockpit;

        public override FurnitureBehaviorTag[] tags { get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Cockpit }; } }

        public CockpitBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }


        Vehicle vehicle => appState.Entities.Vehicles.queries.FindVehicleByFurniture(furniture);
        VehicleAttributesGroup vehicleAttributesGroup => appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);
        VehicleAttribute.Modifier pilotModifier;

        public override void InteractStart(Resident resident)
        {
            base.InteractStart(resident);

            pilotModifier = new VehicleAttribute.Modifier("Piloting", 1f);
            appState.Attributes.Vehicles.AddStaticAttributeModifier(vehicleAttributesGroup, VehicleAttribute.Key.CurrentSpeed, pilotModifier);
        }

        public override void InteractEnd(Resident resident)
        {
            base.InteractEnd(resident);
            appState.Attributes.Vehicles.RemoveStaticAttributeModifier(vehicleAttributesGroup, VehicleAttribute.Key.CurrentSpeed, pilotModifier);
        }

        // TODO - this should go in vehicle queries
        public int CalculateEnginePower()
        {
            // foreach (Room room in vehicle.roomList.items)
            // {
            //     FurnitureBehaviorList furnitureBehaviorList =
            //         appState.FurnitureBehaviors.furnitureBehaviorList
            //             .FindByRoom(room);
            //     FurnitureBehaviorList engineBehaviorList =
            //         furnitureBehaviorList
            //             .FilterByType(FurnitureBehaviorBase.Key.Engine);

            //     // TODO - some engines produce more engine power than others
            //     result += engineBehaviorList.Count;
            // }

            return 1;
        }
    }
}