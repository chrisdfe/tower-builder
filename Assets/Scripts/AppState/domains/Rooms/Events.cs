using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Buildings;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Validators;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.State.Rooms
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

            public delegate void RoomBlockEvent(Room room, RoomCells roomBlock);
            public RoomBlockEvent onRoomBlockAdded;
            public RoomBlockEvent onRoomBlockRemoved;

            public delegate void RoomBlockUpdatedEvent(Room room);
            public RoomBlockUpdatedEvent onRoomBlocksUpdated;

            public delegate void RoomConnectionsEvent(RoomConnections allRoomConnections, RoomConnections roomConnections);
            public RoomConnectionsEvent onRoomConnectionsAdded;
            public RoomConnectionsEvent onRoomConnectionsRemoved;

            public delegate void RoomConnectionsUpdatedEvent(RoomConnections allRoomConnections);
            public RoomConnectionsUpdatedEvent onRoomConnectionsUpdated;
        }
    }
}
