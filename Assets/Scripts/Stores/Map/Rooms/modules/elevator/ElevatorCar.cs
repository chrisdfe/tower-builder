using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Modules
{
    public class ElevatorCar
    {
        Modules.Elevator elevator;

        ElevatorCarPosition currentPosition = ElevatorCarPosition.Top;

        public ElevatorCar(Modules.Elevator elevator)
        {
            this.elevator = elevator;
        }

        public void Initialize() { }
        public void OnDestroy() { }

        public CellCoordinates GetCarFloor()
        {
            int x = elevator.room.roomCells.GetLowestX();
            int floor;

            if (currentPosition == ElevatorCarPosition.Top)
            {
                floor = elevator.room.roomCells.GetHighestFloor();
            }
            else
            {
                // ElevatorCarPosition.Bottom
                floor = elevator.room.roomCells.GetLowestFloor();
            }

            return new CellCoordinates(x, floor);
        }
    }
}