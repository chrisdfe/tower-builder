using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.State;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoom : MonoBehaviour
    {
        public Room room { get; private set; }

        public List<GameWorldRoomCell> gameWorldRoomCells = new List<GameWorldRoomCell>();

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
            InitializeRoomEntrances();
        }

        void Awake()
        {
            transform.localPosition = Vector3.zero;
            roomCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomCell");
            gameWorldRoomEntrancePrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomEntrance");

            Registry.appState.Rooms.roomList.onItemAdded += OnRoomAdded;
            Registry.appState.Rooms.roomList.onRoomBlockRemoved += OnRoomBlockDestroyed;
            Registry.appState.Rooms.roomConnections.onItemsChanged += OnRoomConnectionsUpdated;

            Registry.appState.UI.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.onCurrentSelectedRoomBlockUpdated += OnCurrentSelectedRoomBlockUpdated;
            Registry.appState.UI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnInspectRoomUpdated;
            Registry.appState.UI.buildToolSubState.onBlueprintRoomConnectionsUpdated += OnBlueprintRoomConnectionsUpdated;
        }

        void OnDestroy()
        {
            DestroyCells();

            Registry.appState.Rooms.roomList.onItemAdded -= OnRoomAdded;
            Registry.appState.Rooms.roomList.onRoomBlockRemoved -= OnRoomBlockDestroyed;
            Registry.appState.Rooms.roomConnections.onItemsChanged -= OnRoomConnectionsUpdated;

            Registry.appState.UI.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.onCurrentSelectedRoomBlockUpdated -= OnCurrentSelectedRoomBlockUpdated;
            Registry.appState.UI.inspectToolSubState.onCurrentInspectedRoomUpdated -= OnInspectRoomUpdated;
            Registry.appState.UI.buildToolSubState.onBlueprintRoomConnectionsUpdated -= OnBlueprintRoomConnectionsUpdated;
        }

        void OnCurrentSelectedRoomUpdated(Room selectedRoom)
        {
            if (this.room.isInBlueprintMode)
            {
                return;
            }

            if (selectedRoom == null)
            {
                foreach (GameWorldRoomCell roomCell in gameWorldRoomCells)
                {
                    roomCell.ResetColor();
                }
                return;
            }

            // This is handled in OnCurrentSelectedRoomBlockUpdated
            if (Registry.appState.UI.toolState.value == ToolState.Destroy)
            {
                return;
            }

            if (selectedRoom == null)
            {
                ResetCellColors();
                return;
            }

            if (this.room.id == selectedRoom.id)
            {
                if (!IsInCurrentInspectedRoom())
                {
                    foreach (GameWorldRoomCell roomCell in gameWorldRoomCells)
                    {
                        roomCell.SetHoverColor();
                    }
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

        void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock)
        {
            if (room.isInBlueprintMode)
            {
                return;
            }

            if (roomBlock == null)
            {
                foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
                {
                    gameWorldRoomCell.ResetColor();
                }
                return;
            }

            if (Registry.appState.UI.toolState.value == ToolState.Destroy)
            {
                foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
                {
                    if (roomBlock.Contains(gameWorldRoomCell.roomCell))
                    {
                        gameWorldRoomCell.SetDestroyHoverColor();
                    }
                    else
                    {
                        gameWorldRoomCell.ResetColor();
                    }
                }
            }
        }

        bool IsInCurrentInspectedRoom()
        {
            Room currentInspectedRoom = Registry.appState.UI.inspectToolSubState.currentInspectedRoom;
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
            // if (this.room.id == room.id)
            // {
            //     OnBuild();
            // }
        }

        void OnRoomBlockDestroyed(RoomCells roomBlock)
        {
            List<GameWorldRoomCell> gameWorldRoomCellsCopy = new List<GameWorldRoomCell>(gameWorldRoomCells);
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                if (roomBlock.Contains(gameWorldRoomCell.roomCell))
                {
                    Destroy(gameWorldRoomCell.gameObject);
                    gameWorldRoomCellsCopy.Remove(gameWorldRoomCell);
                }
            }

            gameWorldRoomCells = gameWorldRoomCellsCopy;

            UpdateRoomCells();
        }

        void OnRoomConnectionsUpdated(List<RoomConnection> roomConnections)
        {
            UpdateRoomEntrances();
        }

        void OnBlueprintRoomConnectionsUpdated(RoomConnections blueprintRoomConnections)
        {
            UpdateRoomEntrances();
        }

        void UpdateRoomEntrances()
        {
            foreach (GameWorldRoomEntrance gameWorldRoomEntrance in gameWorldRoomEntrances)
            {
                RoomConnection roomEntranceConnection =
                   Registry.appState.Rooms.roomConnections.FindConnectionForRoomEntrance(gameWorldRoomEntrance.roomEntrance);

                bool isConnected = roomEntranceConnection != null;

                RoomConnection blueprintRoomEntranceConnection =
                    Registry.appState.UI.buildToolSubState.blueprintRoomConnections
                        .FindConnectionForRoomEntrance(gameWorldRoomEntrance.roomEntrance);

                if (blueprintRoomEntranceConnection != null)
                {
                    isConnected = true;
                }

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

        void UpdateRoomCells()
        {
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                gameWorldRoomCell.UpdateRoomCellMeshSegments();
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

            Registry.appState.Rooms.roomList.onItemAdded -= OnRoomAdded;
        }

        void CreateCells()
        {
            foreach (RoomCell roomCell in room.cells.items)
            {
                GameObject roomCellGameObject = Instantiate<GameObject>(roomCellPrefab);
                GameWorldRoomCell gameWorldRoomCell = roomCellGameObject.GetComponent<GameWorldRoomCell>();

                gameWorldRoomCell.transform.parent = transform;
                gameWorldRoomCell.roomCell = roomCell;
                gameWorldRoomCell.gameWorldRoom = this;
                gameWorldRoomCell.baseColor = room.color;

                gameWorldRoomCell.Initialize();
                gameWorldRoomCells.Add(gameWorldRoomCell);
            }
        }

        void DestroyCells()
        {
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                Destroy(gameWorldRoomCell.gameObject);
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
    }
}