using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Rooms
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

            Registry.Stores.Map.onRoomAdded += OnRoomAdded;
            Registry.Stores.Map.onRoomDestroyed += OnRoomDestroyed;

            // Registry.Stores.MapUI.destroyToolSubState.onCurrentSelectedRoomUpdated += OnDestroyRoomUpdated;
            // Registry.Stores.MapUI.inspectToolSubState.onCurrentSelectedRoomUpdated += OnInspectHoverRoomUpdated;
            // Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnInspectRoomUpdated;
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
            GameObject roomGameObject = Instantiate<GameObject>(roomPrefab);
            roomGameObject.transform.parent = transform;
            GameWorldRoom gameWorldRoom = roomGameObject.GetComponent<GameWorldRoom>();
            gameWorldRoom.SetRoom(room);
            gameWorldRooms.Add(gameWorldRoom);
        }

        void RemoveRoom(Room room)
        {
            GameWorldRoom gameWorldRoom = gameWorldRooms.Find(otherRoom => otherRoom.room == room);
            gameWorldRooms.Remove(gameWorldRoom);
            Destroy(gameWorldRoom);
        }
    }
}