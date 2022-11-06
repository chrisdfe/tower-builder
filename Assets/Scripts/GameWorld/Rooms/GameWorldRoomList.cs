using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Entrances;
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
            Registry.appState.Rooms.events.onRoomConnectionsUpdated += OnRoomConnectionsUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated += OnCurrentSelectedRoomBlockUpdated;

            Registry.appState.Tools.inspectToolSubState.onCurrentInspectedRoomUpdated += OnInspectRoomUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Rooms.events.onRoomAdded -= OnRoomAdded;
            Registry.appState.Rooms.events.onRoomBuilt -= OnRoomBuilt;
            Registry.appState.Rooms.events.onRoomRemoved -= OnRoomDestroyed;
            Registry.appState.Rooms.events.onRoomBlocksUpdated -= OnRoomBlocksUpdated;
            Registry.appState.Rooms.events.onRoomConnectionsUpdated -= OnRoomConnectionsUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated -= OnCurrentSelectedRoomBlockUpdated;

            Registry.appState.Tools.inspectToolSubState.onCurrentInspectedRoomUpdated -= OnInspectRoomUpdated;
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

        void OnRoomConnectionsUpdated(RoomConnections roomConnections)
        {
            gameWorldRooms.ForEach(gameWorldRoom => gameWorldRoom.Reset());
        }

        void OnCurrentSelectedRoomUpdated(Room selectedRoom)
        {
            gameWorldRooms.ForEach(gameWorldRoom => gameWorldRoom.SetRoomCellColors());
        }

        void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock) { }

        void OnInspectRoomUpdated(Room currentInspectedRoom) { }

        /*
         * Rooms 
         */
        void CreateRoom(Room room)
        {
            GameWorldRoom gameWorldRoom = GameWorldRoom.Create(transform);
            gameWorldRoom.SetRoom(room);
            gameWorldRoom.Initialize();
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
    }
}