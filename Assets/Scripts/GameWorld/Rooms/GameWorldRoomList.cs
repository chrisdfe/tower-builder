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
        public RoomList roomList { get; private set; }

        public List<GameWorldRoom> gameWorldRooms = new List<GameWorldRoom>();

        GameObject roomPrefab;

        void Awake()
        {
            roomPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/Room");
            ResetRooms();

            Registry.appState.Rooms.onRoomAdded += OnRoomAdded;
            Registry.appState.Rooms.onRoomDestroyed += OnRoomDestroyed;
        }

        void OnRoomAdded(Room room)
        {
            CreateRoom(room);
        }

        void OnRoomDestroyed(Room room)
        {
            RemoveRoom(room);
        }

        void ResetRooms()
        {

        }

        void CreateRoom(Room room)
        {
            Debug.Log(room);
            GameObject roomGameObject = Instantiate<GameObject>(roomPrefab);
            roomGameObject.transform.parent = transform;
            GameWorldRoom gameWorldRoom = roomGameObject.GetComponent<GameWorldRoom>();
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