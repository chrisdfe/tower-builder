/* using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map.Rooms.Modules;
using UnityEngine;


namespace TowerBuilder.GameWorld.Map.Rooms.Modules
{
    public class GameWorldElevatorModule : GameWorldRoomModuleBase
    {
        public ElevatorModule elevatorModule;

        public List<GameWorldElevatorCar> elevatorCars = new List<GameWorldElevatorCar>();

        public GameWorldElevatorModule(GameWorldRoom gameWorldRoom, ElevatorModule elevatorModule) : base(gameWorldRoom)
        {
            this.elevatorModule = elevatorModule;
        }

        public override void Initialize()
        {
            foreach (ElevatorCar elevatorCar in elevatorModule.cars)
            {
                GameWorldElevatorCar gameWorldElevatorCar = new GameWorldElevatorCar(this, elevatorCar);
                gameWorldElevatorCar.Initialize();
                elevatorCars.Add(gameWorldElevatorCar);
            }
        }
    }
} */