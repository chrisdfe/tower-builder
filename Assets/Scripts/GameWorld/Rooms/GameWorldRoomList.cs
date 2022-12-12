using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoomList : MonoBehaviour
    {
        [HideInInspector]
        public List<GameWorldRoom> gameWorldRooms = new List<GameWorldRoom>();

        void Awake()
        {
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            Registry.appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            Registry.appState.Entities.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            Registry.appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;
            Registry.appState.Entities.Rooms.events.onRoomBlocksUpdated += OnRoomBlocksUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated += OnCurrentSelectedRoomBlockUpdated;

            Registry.appState.Tools.destroyToolState.events.onDestroySelectionUpdated += OnDestroySelectionUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            Registry.appState.Entities.Rooms.events.onItemsBuilt -= OnRoomsBuilt;
            Registry.appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;
            Registry.appState.Entities.Rooms.events.onRoomBlocksUpdated -= OnRoomBlocksUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated -= OnCurrentSelectedRoomBlockUpdated;

            Registry.appState.Tools.destroyToolState.events.onDestroySelectionUpdated -= OnDestroySelectionUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /*
         * Rooms - Internals
         */
        void CreateRoom(Room room)
        {
            GameWorldRoom gameWorldRoom = GameWorldRoom.Create(transform);
            gameWorldRoom.SetRoom(room);
            gameWorldRoom.Setup();
            gameWorldRooms.Add(gameWorldRoom);
        }

        void RemoveRoom(Room room)
        {
            GameWorldRoom gameWorldRoom = gameWorldRooms.Find(otherRoom => otherRoom.room == room);
            gameWorldRooms.Remove(gameWorldRoom);
            Destroy(gameWorldRoom.gameObject);
        }

        GameWorldRoom FindGameWorldRoomByRoom(Room room)
        {
            return gameWorldRooms.Find(gwr => gwr.room == room);
        }

        /*
         * Room Cells - Internals
         */
        void SetRoomCellColors()
        {
            gameWorldRooms.ForEach(gameWorldRoom => gameWorldRoom.SetRoomCellColors());
        }

        /* 
         * Event Handlers
         */
        void OnRoomsAdded(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                CreateRoom(room);
            });
        }

        void OnRoomsBuilt(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                GameWorldRoom gameWorldRoom = FindGameWorldRoomByRoom(room);
                if (gameWorldRoom == null) return;
                gameWorldRoom.OnBuild();
            });
        }

        void OnRoomsRemoved(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                RemoveRoom(room);
            });
        }

        void OnRoomBlocksUpdated(Room room, CellCoordinatesBlockList blocks)
        {
            GameWorldRoom gameWorldRoom = FindGameWorldRoomByRoom(room);

            if (gameWorldRoom != null)
            {
                gameWorldRoom.Reset();
            }
        }

        void OnCurrentSelectedRoomUpdated(Room selectedRoom)
        {
            SetRoomCellColors();
        }

        void OnCurrentSelectedRoomBlockUpdated(CellCoordinatesBlock roomBlock) { }

        void OnInspectRoomUpdated(Room currentInspectedRoom) { }

        void OnDestroySelectionUpdated()
        {
            SetRoomCellColors();
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            SetRoomCellColors();
        }
    }
}