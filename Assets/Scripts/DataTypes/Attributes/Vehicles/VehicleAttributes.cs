using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Vehicles;
using UnityEngine;

namespace TowerBuilder.DataTypes.Attributes.Vehicles
{
    public class VehicleAttributes : AttributesGroup<VehicleAttributes.Key>
    {
        public Vehicle vehicle { get; private set; }

        public enum Key
        {
            CurrentSpeed,
            Fuel,
            Weight,
            EnginePower,
            TargetSpeed,
            MaxSpeed,
            IsPiloted,
        }

        public override Dictionary<Key, Attribute> attributes { get; } = new Dictionary<Key, Attribute>() {
            { Key.Weight,       new Attribute(0) },
            { Key.Fuel,         new Attribute(0) },
            { Key.EnginePower,  new Attribute(0) },
            { Key.MaxSpeed,     new Attribute(0) },
            { Key.TargetSpeed,  new Attribute(0) },
            { Key.CurrentSpeed, new Attribute(0) },
        };

        public bool isMoving => attributes.GetValueOrDefault(Key.CurrentSpeed).value > 0;

        public VehicleAttributes(AppState appState, Vehicle vehicle) : base(appState)
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
                        .FilterByType(FurnitureBehavior.Key.Engine);

                // TODO - some engines produce more engine power than others
                result += engineBehaviorList.Count;
            }

            enginePower = result;
        }
        */
    }
}