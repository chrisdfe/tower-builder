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
            UpdateRoomEntrances();
        }

        void Awake()
        {
            transform.localPosition = Vector3.zero;
            roomCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomCell");

            Registry.Stores.Map.onRoomAdded += OnRoomAdded;
            Registry.Stores.MapUI.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnInspectRoomUpdated;
            Registry.Stores.MapUI.buildToolSubState.onBlueprintRoomConnectionsUpdated += OnBlueprintRoomConnectionsUpdated;
        }

        void OnDestroy()
        {
            DestroyCells();

            Registry.Stores.Map.onRoomAdded -= OnRoomAdded;
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
        }

        // TODO - this is how to determine when this room goes from blueprint mode to getting added to the map
        void OnRoomAdded(Room room)
        {
            if (this.room.id == room.id)
            {
                OnBuild();
            }
        }

        void OnBlueprintRoomConnectionsUpdated(RoomConnections blueprintRoomConnections)
        {
            UpdateRoomEntrances();
        }

        void UpdateRoomEntrances()
        {
            Blueprint blueprint = Registry.Stores.MapUI.buildToolSubState.currentBlueprint;
            RoomConnections blueprintRoomConnections = Registry.Stores.MapUI.buildToolSubState.blueprintRoomConnections;

            RoomConnections connections = blueprintRoomConnections.FindConnectionsForRoom(room);

            Debug.Log("I am in room " + room.id);
            // find 
            foreach (GameWorldRoomCell roomCell in gameWorldRoomCells)
            {
                foreach (GameWorldRoomEntrance gameWorldRoomEntrance in roomCell.gameWorldRoomEntrances)
                {
                    RoomConnection roomEntranceConnection =
                        connections.FindConnectionForRoomEntrance(gameWorldRoomEntrance.roomEntrance);

                    if (roomEntranceConnection != null)
                    {
                        Debug.Log("I am connected from " + roomEntranceConnection.roomA.id);
                        Debug.Log("I am connected to " + roomEntranceConnection.roomB.id);
                        gameWorldRoomEntrance.SetConnectedColor();
                    }
                    else
                    {
                        gameWorldRoomEntrance.ResetColor();
                    }
                }
            }
        }

        // When this has been converted from a blueprint room to a actual room
        void OnBuild()
        {
            ResetCellColors();
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