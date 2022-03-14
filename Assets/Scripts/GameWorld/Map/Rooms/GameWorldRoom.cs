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
    public class GameWorldRoom : MonoBehaviour
    {
        public Room room { get; private set; }

        public List<GameWorldRoomCell> gameWorldRoomCells = new List<GameWorldRoomCell>();

        GameObject roomCellPrefab;

        void Awake()
        {
            roomCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomCell");
        }

        public void SetRoom(Room room)
        {
            this.room = room;
        }

        void ResetCells()
        {
            foreach (RoomCell roomCell in room.roomCells.cells)
            {
                GameObject roomCellGameObject = Instantiate<GameObject>(roomCellPrefab);
                GameWorldRoomCell gameWorldRoomCell = roomCellGameObject.GetComponent<GameWorldRoomCell>();
                gameWorldRoomCell.SetRoomCell(roomCell);
                gameWorldRoomCells.Add(gameWorldRoomCell);
            }
        }
    }
}