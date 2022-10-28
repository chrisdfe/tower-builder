using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
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

        public void Setup()
        {
            Registry.appState.Rooms.onRoomAdded += OnRoomAdded;
            Registry.appState.Rooms.onRoomRemoved += OnRoomDestroyed;
        }

        public void Teardown()
        {
            Registry.appState.Rooms.onRoomAdded -= OnRoomAdded;
            Registry.appState.Rooms.onRoomRemoved -= OnRoomDestroyed;
        }

        void OnRoomAdded(Room room)
        {
            CreateRoom(room);
        }

        void OnRoomDestroyed(Room room)
        {
            RemoveRoom(room);
        }

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
    }
}