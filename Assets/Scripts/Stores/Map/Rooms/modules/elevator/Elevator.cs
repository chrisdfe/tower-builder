using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Modules
{
    public class Elevator : RoomModuleBase
    {
        public List<ElevatorCar> cars;

        public Elevator(Room room) : base(room) { }

        public override void Initialize()
        {
            // Elevators start off with 1 car
            AddCar();
        }

        public void AddCar()
        {
            cars.Add(new ElevatorCar(this));
        }

        public void RemoveCar(ElevatorCar carToDelete)
        {
            cars.Remove(carToDelete);
        }
    }
}