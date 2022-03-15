using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Modules
{
    public class ElevatorModule : RoomModuleBase
    {
        public override RoomModuleKey key { get { return RoomModuleKey.Elevator; } }

        public List<ElevatorCar> cars = new List<ElevatorCar>();

        public ElevatorModule(Room room) : base(room) { }

        public override void Initialize()
        {
            // Elevators start off with 1 car
            AddCar();
        }

        public override void OnDestroy()
        {

        }

        void AddCar()
        {
            ElevatorCar newElevatorCar = new ElevatorCar(this);
            newElevatorCar.Initialize();
            cars.Add(newElevatorCar);
        }

        void RemoveCar(ElevatorCar carToDelete)
        {
            cars.Remove(carToDelete);
        }

        void RemoveAllCars()
        {
            foreach (ElevatorCar car in cars)
            {
                car.OnDestroy();
            }

            cars = new List<ElevatorCar>();
        }
    }
}