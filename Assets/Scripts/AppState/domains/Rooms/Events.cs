using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Validators;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Rooms
{
    public partial class State
    {
        public class Events
        {
            public delegate void RoomEvent(Room room);
            public RoomEvent onRoomAdded;
            public RoomEvent onRoomRemoved;
            public RoomEvent onRoomBuilt;

            public delegate void RoomListEvent(RoomList roomList);
            public RoomListEvent onRoomListUpdated;

            public delegate void RoomBlocksEvent(Room room, RoomBlocks roomBlocks);
            public RoomBlocksEvent onRoomBlocksAdded;
            public RoomBlocksEvent onRoomBlocksRemoved;

            public delegate void RoomBlockUpdatedEvent(Room room);
            public RoomBlockUpdatedEvent onRoomBlocksUpdated;

            public delegate void RoomConnectionsEvent(RoomConnectionList allRoomConnections, RoomConnectionList roomConnections);
            public RoomConnectionsEvent onRoomConnectionsAdded;
            public RoomConnectionsEvent onRoomConnectionsRemoved;

            public delegate void RoomConnectionsUpdatedEvent(RoomConnectionList allRoomConnections);
            public RoomConnectionsUpdatedEvent onRoomConnectionListUpdated;
        }
    }
}
