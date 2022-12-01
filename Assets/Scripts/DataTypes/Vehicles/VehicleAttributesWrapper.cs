using System.Linq;
using System.Threading;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Vehicles
{
    public class VehicleAttributesWrapper
    {
        public Vehicle vehicle { get; private set; }

        public int weight { get; private set; } = 0;

        public int fuel { get; private set; } = 0;
        public int enginePower { get; private set; } = 0;

        public bool isMoving { get; private set; }
        public float maxSpeed { get; private set; } = 10;
        public float currentSpeed { get; private set; } = 0;

        AppState appState;

        public VehicleAttributesWrapper(AppState appState, Vehicle vehicle)
        {
            this.appState = appState;
            this.vehicle = vehicle;
        }

        public void Setup()
        {
            RecalculateAll();
        }

        public void Teardown() { }

        public void StartMoving()
        {
            isMoving = true;
            // No accelleration for now, just go straight to max speed;
            currentSpeed = maxSpeed;
        }

        public void StopMoving()
        {
            isMoving = false;

            currentSpeed = 0;
        }

        public void RecalculateAll()
        {
            RecalculateWeight();
            RecalculateEnginePower();
        }

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
    }
}