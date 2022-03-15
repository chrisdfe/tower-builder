using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Modules;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Rooms.Modules
{
    public class GameWorldElevatorCar
    {
        public GameWorldElevatorModule gameWorldElevatorModule;
        public ElevatorCar elevatorCar;

        GameObject elevatorCarPrefab;
        GameObject elevatorCarGameObject;

        public GameWorldElevatorCar(GameWorldElevatorModule gameWorldElevatorModule, ElevatorCar elevatorCar)
        {
            this.gameWorldElevatorModule = gameWorldElevatorModule;
            this.elevatorCar = elevatorCar;
        }

        public void Initialize()
        {
            elevatorCarPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/Modules/Elevator/ElevatorCar");
            elevatorCarGameObject = GameObject.Instantiate(elevatorCarPrefab);
            elevatorCarGameObject.transform.parent = gameWorldElevatorModule.gameWorldRoom.transform;

            PositionCar();
        }

        void PositionCar()
        {
            CellCoordinates cellCoordinates = GetTargetCellCoordinates();
            elevatorCarGameObject.transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(cellCoordinates);
        }

        CellCoordinates GetTargetCellCoordinates()
        {
            int floor;
            if (elevatorCar.currentPosition == ElevatorCarPosition.Top)
            {
                floor = gameWorldElevatorModule.gameWorldRoom.room.roomCells.GetHighestFloor();
            }
            else
            {
                // ElevatorCarPosition.Bottom
                floor = gameWorldElevatorModule.gameWorldRoom.room.roomCells.GetLowestFloor();
            }

            int x = gameWorldElevatorModule.gameWorldRoom.room.roomCells.GetLowestX();
            return new CellCoordinates(x, floor);
        }
    }
}