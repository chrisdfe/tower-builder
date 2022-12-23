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
    public class EngineBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Engine;

        public override FurnitureBehaviorTag[] tags
        {
            get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Engine }; }
        }

        public EngineBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        public override void InteractStart(Resident resident)
        {
            base.InteractStart(resident);

            Vehicle vehicle = appState.Entities.Vehicles.queries.FindVehicleByFurniture(furniture);
            VehicleAttributesGroup vehicleAttributesGroup = appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);

            VehicleAttribute.Modifier modifier = new VehicleAttribute.Modifier("Engine Power", 1f);
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
        }

        public override void InteractEnd(Resident resident)
        {
            base.InteractEnd(resident);
        }
    }
}