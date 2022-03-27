using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.GameWorld.Map.Rooms.Modules;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Connections;
using TowerBuilder.Stores.Map.Rooms.Modules;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Rooms
{
    public class GameWorldRoom : MonoBehaviour
    {
        public Room room { get; private set; }

        public List<GameWorldRoomCell> gameWorldRoomCells = new List<GameWorldRoomCell>();

        public List<GameWorldRoomModuleBase> modules = new List<GameWorldRoomModuleBase>();

        public List<GameWorldRoomEntrance> gameWorldRoomEntrances = new List<GameWorldRoomEntrance>();

        GameObject roomCellPrefab;
        GameObject gameWorldRoomEntrancePrefab;

        public void SetRoom(Room room)
        {
            this.room = room;
            gameObject.name = $"Room {room.id}";
        }

        public void Initialize()
        {
            CreateCells();
            InitializeModules();
            InitializeRoomEntrances();
        }

        void Awake()
        {
            transform.localPosition = Vector3.zero;
            roomCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomCell");
            gameWorldRoomEntrancePrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomEntrance");

            Registry.Stores.Map.onRoomAdded += OnRoomAdded;
            Registry.Stores.Map.onRoomConnectionsUpdated += OnRoomConnectionsUpdated;

            Registry.Stores.MapUI.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnInspectRoomUpdated;
            Registry.Stores.MapUI.buildToolSubState.onBlueprintRoomConnectionsUpdated += OnBlueprintRoomConnectionsUpdated;
        }

        void OnDestroy()
        {
            DestroyCells();

            Registry.Stores.Map.onRoomAdded -= OnRoomAdded;
            Registry.Stores.Map.onRoomConnectionsUpdated -= OnRoomConnectionsUpdated;

            Registry.Stores.MapUI.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated -= OnInspectRoomUpdated;
            Registry.Stores.MapUI.buildToolSubState.onBlueprintRoomConnectionsUpdated -= OnBlueprintRoomConnectionsUpdated;
        }

        void OnCurrentSelectedRoomUpdated(Room room)
        {
            if (room.isInBlueprintMode)
            {
                return;
            }

            if (room == null)
            {
                ResetCellColors();
                return;
            }

            if (this.room.id == room.id)
            {
                switch (Registry.Stores.MapUI.toolState)
                {
                    case ToolState.Destroy:
                        foreach (GameWorldRoomCell roomCell in gameWorldRoomCells)
                        {
                            roomCell.SetDestroyHoverColor();
                        }
                        break;
                    default:
                        if (!IsInCurrentInspectedRoom())
                        {
                            foreach (GameWorldRoomCell roomCell in gameWorldRoomCells)
                            {
                                roomCell.SetHoverColor();
                            }
                        }
                        break;
                }
            }
            else
            {
                foreach (GameWorldRoomCell roomCell in gameWorldRoomCells)
                {
                    roomCell.ResetColor();
                }
            }
        }

        bool IsInCurrentInspectedRoom()
        {
            Room currentInspectedRoom = Registry.Stores.MapUI.inspectToolSubState.currentInspectedRoom;
            return currentInspectedRoom != null && currentInspectedRoom.id == room.id;
        }

        void OnInspectRoomUpdated(Room currentInspectRoom)
        {
            if (room.isInBlueprintMode)
            {
                return;
            }

            foreach (GameWorldRoomCell roomCell in gameWorldRoomCells)
            {
                if (IsInCurrentInspectedRoom())
                {
                    roomCell.SetInspectColor();
                }
                else
                {
                    roomCell.ResetColor();
                }
            }

            UpdateRoomEntrances();
        }

        // TODO - this is how to determine when this room goes from blueprint mode to getting added to the map
        void OnRoomAdded(Room room)
        {
            if (this.room.id == room.id)
            {
                OnBuild();
            }
        }

        void OnRoomConnectionsUpdated(RoomConnections roomConnections)
        {
            UpdateRoomEntrances();
        }

        void OnBlueprintRoomConnectionsUpdated(RoomConnections blueprintRoomConnections)
        {
            UpdateRoomEntrances();
        }

        void UpdateRoomCells()
        {

        }

        void UpdateRoomEntrances()
        {
            foreach (GameWorldRoomEntrance gameWorldRoomEntrance in gameWorldRoomEntrances)
            {
                RoomConnection roomEntranceConnection =
                   Registry.Stores.Map.roomConnections.FindConnectionForRoomEntrance(gameWorldRoomEntrance.roomEntrance);

                bool isConnected = roomEntranceConnection != null;

                // if (!room.isInBlueprintMode)
                // {
                RoomConnection blueprintRoomEntranceConnection =
                    Registry.Stores.MapUI.buildToolSubState.blueprintRoomConnections
                        .FindConnectionForRoomEntrance(gameWorldRoomEntrance.roomEntrance);

                if (blueprintRoomEntranceConnection != null)
                {
                    isConnected = true;
                }
                // }

                if (isConnected)
                {
                    gameWorldRoomEntrance.SetConnectedColor();
                }
                else
                {
                    gameWorldRoomEntrance.ResetColor();
                }
            }
        }

        void InitializeRoomEntrances()
        {
            foreach (RoomEntrance roomEntrance in room.entrances)
            {
                GameObject roomEntranceGameObject = Instantiate<GameObject>(gameWorldRoomEntrancePrefab);
                roomEntranceGameObject.transform.parent = transform;

                GameWorldRoomEntrance gameWorldRoomEntrance = roomEntranceGameObject.GetComponent<GameWorldRoomEntrance>();
                gameWorldRoomEntrance.roomEntrance = roomEntrance;
                gameWorldRoomEntrance.Initialize();
                gameWorldRoomEntrances.Add(gameWorldRoomEntrance);
            }

            // UpdateRoomEntrances();
        }

        // When this has been converted from a blueprint room to a actual room
        void OnBuild()
        {
            ResetCellColors();
            UpdateRoomEntrances();

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
                gameWorldRoomCell.gameWorldRoom = this;
                gameWorldRoomCell.baseColor = room.roomDetails.color;

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

        void ResetCellColors()
        {
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                gameWorldRoomCell.ResetColor();
            }
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