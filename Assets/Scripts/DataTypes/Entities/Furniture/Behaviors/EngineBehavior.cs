using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures.Behaviors
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

            Vehicle vehicle = appState.Vehicles.queries.FindVehicleByFurniture(furniture);
            VehicleAttributesWrapper vehicleAttributesWrapper = appState.VehicleAttributesWrappers.queries.FindByVehicle(vehicle);

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

            Debug.Log("engine interact start");
        }

        public override void InteractEnd(Resident resident)
        {
            base.InteractEnd(resident);
        }
    }
}