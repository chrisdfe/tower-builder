using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.Stores.Rooms
{
    [Serializable]
    public class State
    {
        public RoomList rooms { get; private set; } = new RoomList();

        public delegate void RoomAddedEvent(Room room);
        public RoomAddedEvent onRoomAdded;

        public delegate void RoomDestroyedEvent(Room room);
        public RoomDestroyedEvent onRoomDestroyed;

        public delegate void RoomBlockDestroyedEvent(RoomCells roomBlock);
        public RoomBlockDestroyedEvent onRoomBlockDestroyed;

        public RoomConnections roomConnections { get; private set; } = new RoomConnections();

        public delegate void RoomConnectionsEvent(RoomConnections roomConnections);
        public RoomConnectionsEvent onRoomConnectionsUpdated;

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

            RemoveRoomConnectionsForRoom(room);

            rooms.Remove(room);

            if (onRoomDestroyed != null)
            {
                onRoomDestroyed(room);
            }
        }

        public void DestroyRoomBlock(Room room, RoomCells roomBlock)
        {
            // TODO - check if doing this is going to divide the room into 2
            // if so, create another room right here

            room.RemoveBlock(roomBlock);
            room.ResetRoomCellOrientations();

            // TODO - destroy connections block may have had

            if (room.blocks.Count == 0)
            {
                DestroyRoom(room);
            }
            else
            {
                if (onRoomBlockDestroyed != null)
                {
                    onRoomBlockDestroyed(roomBlock);
                }
            }
        }

        public void AddRoomConnections(RoomConnections newRoomConnections)
        {
            roomConnections.Add(newRoomConnections);

            if (onRoomConnectionsUpdated != null)
            {
                onRoomConnectionsUpdated(roomConnections);
            }
        }

        public void RemoveRoomConnectionsForRoom(Room roomBeingDestroyed)
        {
            roomConnections.RemoveConnectionsForRoom(roomBeingDestroyed);

            if (onRoomConnectionsUpdated != null)
            {
                onRoomConnectionsUpdated(roomConnections);
            }
        }
    }
}
