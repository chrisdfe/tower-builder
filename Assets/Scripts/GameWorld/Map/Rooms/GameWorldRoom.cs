using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.GameWorld.Map.Rooms.Modules;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Modules;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Rooms
{
    public class GameWorldRoom : MonoBehaviour
    {
        public Room room { get; private set; }

        public List<GameWorldRoomCell> gameWorldRoomCells = new List<GameWorldRoomCell>();

        public List<GameWorldRoomModuleBase> modules = new List<GameWorldRoomModuleBase>();

        GameObject roomCellPrefab;

        public void SetRoom(Room room)
        {
            this.room = room;
            gameObject.name = $"Room {room.id}";
        }

        public void Initialize()
        {
            CreateCells();
            InitializeModules();
        }

        void Awake()
        {
            transform.localPosition = Vector3.zero;
            roomCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomCell");

            Registry.Stores.Map.onRoomAdded += OnRoomAdded;
        }

        void OnDestroy()
        {
            DestroyCells();
            Registry.Stores.Map.onRoomAdded -= OnRoomAdded;
        }

        // TODO - this is how to determine when this room goes from blueprint mode to getting added to the map
        void OnRoomAdded(Room room)
        {
            if (room.id == this.room.id)
            {
                OnBuild();
            }
        }

        // When this has been converted from a blueprint room to a actual room
        void OnBuild()
        {
            Debug.Log("I have been built");

            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                gameWorldRoomCell.isInBlueprintMode = false;
            }

            Registry.Stores.Map.onRoomAdded -= OnRoomAdded;
        }

        void CreateCells()
        {
            foreach (RoomCell roomCell in room.roomCells.cells)
            {
                GameObject roomCellGameObject = Instantiate<GameObject>(roomCellPrefab);
                GameWorldRoomCell gameWorldRoomCell = roomCellGameObject.GetComponent<GameWorldRoomCell>();
                gameWorldRoomCell.transform.parent = transform;
                gameWorldRoomCell.roomCell = roomCell;
                gameWorldRoomCell.Initialize();
                gameWorldRoomCells.Add(gameWorldRoomCell);
            }
        }

        void DestroyCells()
        {
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                Destroy(gameWorldRoomCell);
            }

            gameWorldRoomCells = new List<GameWorldRoomCell>();
        }

        void ResetCells()
        {
            DestroyCells();
            CreateCells();
        }

        void InitializeModules()
        {
            foreach (RoomModuleBase module in room.modules)
            {
                switch (module)
                {
                    case ElevatorModule elevatorModule:
                        GameWorldElevatorModule gameWorldElevatorModule = new GameWorldElevatorModule(this, elevatorModule);
                        modules.Add(gameWorldElevatorModule);
                        break;
                }
            }

            foreach (GameWorldRoomModuleBase module in modules)
            {
                module.Initialize();
            }
        }
    }
}