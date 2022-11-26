using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoomList : MonoBehaviour
    {
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
            Registry.appState.Rooms.events.onRoomAdded += OnRoomAdded;
            Registry.appState.Rooms.events.onRoomBuilt += OnRoomBuilt;
            Registry.appState.Rooms.events.onRoomRemoved += OnRoomDestroyed;
            Registry.appState.Rooms.events.onRoomBlocksUpdated += OnRoomBlocksUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated += OnCurrentSelectedRoomBlockUpdated;

            Registry.appState.Tools.destroyToolState.events.onDestroySelectionUpdated += OnDestroySelectionUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Rooms.events.onRoomAdded -= OnRoomAdded;
            Registry.appState.Rooms.events.onRoomBuilt -= OnRoomBuilt;
            Registry.appState.Rooms.events.onRoomRemoved -= OnRoomDestroyed;
            Registry.appState.Rooms.events.onRoomBlocksUpdated -= OnRoomBlocksUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated -= OnCurrentSelectedRoomBlockUpdated;

            Registry.appState.Tools.destroyToolState.events.onDestroySelectionUpdated -= OnDestroySelectionUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /* 
         * Event Handlers
         */
        void OnRoomAdded(Room room)
        {
            CreateRoom(room);
        }

        void OnRoomBuilt(Room room)
        {
            GameWorldRoom gameWorldRoom = FindGameWorldRoomByRoom(room);
            if (gameWorldRoom == null) return;
            gameWorldRoom.OnBuild();
        }

        void OnRoomDestroyed(Room room)
        {
            RemoveRoom(room);
        }

        void OnRoomBlocksUpdated(Room room)
        {
            GameWorldRoom gameWorldRoom = FindGameWorldRoomByRoom(room);
            if (gameWorldRoom == null) return;
            gameWorldRoom.Reset();
        }

        void OnCurrentSelectedRoomUpdated(Room selectedRoom)
        {
            SetRoomCellColors();
        }

        void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock) { }

        void OnInspectRoomUpdated(Room currentInspectedRoom) { }

        void OnDestroySelectionUpdated()
        {
            SetRoomCellColors();
        }

        void OnCurrentSelectedEntityUpdated(EntityBase entity)
        {
            SetRoomCellColors();
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
    }
}