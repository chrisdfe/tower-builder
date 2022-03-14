using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public class State
    {
        public RoomList rooms { get; private set; } = new RoomList();

        public delegate void RoomAddedEvent(Room mapRoom);
        public RoomAddedEvent onRoomAdded;

        public delegate void RoomDestroyedEvent(Room mapRoom);
        public RoomAddedEvent onRoomDestroyed;

        public void AddRoom(Room room)
        {
            if (room == null)
            {
                return;
            }

            rooms.Add(room);
            room.OnBuild();

            if (onRoomAdded != null)
            {
                onRoomAdded(room);
            }
        }

        public void DestroyRoom(Room room)
        {
            if (room == null)
            {
                return;
            }

            rooms.Remove(room);

            if (onRoomDestroyed != null)
            {
                onRoomDestroyed(room);
            }
        }
    }
}
