using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Connections;
using TowerBuilder.Stores.Map.Rooms.Modules;
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

        public delegate void ElevatorCarPositionEvent(ElevatorCar elevatorCar, ElevatorCarPosition destinationPosition);
        public ElevatorCarPositionEvent onElevatorCarPositionChanged;

        public RoomConnections roomConnections { get; private set; } = new RoomConnections();

        public delegate void RoomConnectionEvent(RoomConnection roomConnection);
        public RoomConnectionEvent onRoomConnectionAdded;
        public RoomConnectionEvent onRoomConnectionRemoved;

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

        public void SetElevatorCarDestination(ElevatorCar elevatorCar, ElevatorCarPosition destinationPosition)
        {
            elevatorCar.currentPosition = destinationPosition;

            if (onElevatorCarPositionChanged != null)
            {
                onElevatorCarPositionChanged(elevatorCar, destinationPosition);
            }
        }
    }
}
