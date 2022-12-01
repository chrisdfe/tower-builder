using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Vehicles
{
    public class VehicleAttributesWrapper : AttributesWrapper<VehicleAttribute, VehicleAttribute.Key>
    {
        public Vehicle vehicle { get; private set; }

        public override List<VehicleAttribute> attributes { get; } = new List<VehicleAttribute>() {
            new VehicleAttribute(VehicleAttribute.Key.Weight, 0),
            new VehicleAttribute(VehicleAttribute.Key.Fuel, 0),
            new VehicleAttribute(VehicleAttribute.Key.EnginePower, 0),
            new VehicleAttribute(VehicleAttribute.Key.MaxSpeed, 0),
            new VehicleAttribute(VehicleAttribute.Key.TargetSpeed, 0),
            new VehicleAttribute(VehicleAttribute.Key.CurrentSpeed, 0),
        };

        public bool isMoving
        {
            get => FindByKey(VehicleAttribute.Key.CurrentSpeed).value > 0;
        }

        public VehicleAttributesWrapper(AppState appState, Vehicle vehicle) : base(appState)
        {
            this.vehicle = vehicle;
        }

        /*        
        public void RecalculateWeight()
        {
            // TODO use room type to calculate room cell weight
            this.weight = this.vehicle.roomList.items.Aggregate(0, (acc, room) =>
            {
                return acc + room.blocks.cells.Count;
            });
        }

        public void RecalculateEnginePower()
        {
            int result = 0;

            foreach (Room room in vehicle.roomList.items)
            {
                FurnitureBehaviorList furnitureBehaviorList =
                    appState.FurnitureBehaviors.furnitureBehaviorList
                        .FindByRoom(room);
                FurnitureBehaviorList engineBehaviorList =
                    furnitureBehaviorList
                        .FilterByType(FurnitureBehaviorBase.Key.Engine);

                // TODO - some engines produce more engine power than others
                result += engineBehaviorList.Count;
            }

            enginePower = result;
        }
        */
    }
}